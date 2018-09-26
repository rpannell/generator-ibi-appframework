using IBI.<%= Name %>.Application.Utils.Core.Models;
using IBI.<%= Name %>.Application.Utils.Session.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    public class WebserviceSession : ISession
    {
        #region Fields

        private const int IdByteCount = 16;
        private const int KeyLengthLimit = ushort.MaxValue;
        private const byte SerializationRevision = 2;
        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();
        private readonly TimeSpan idleTimeout;
        private readonly TimeSpan ioTimeout;
        private readonly ILogger logger;
        private readonly string sessionKey;
        private readonly Func<bool> tryEstablishSession;
        private bool isAvailable;
        private bool isModified;
        private bool isNewSessionKey;
        private bool loaded;
        private string sessionId;
        private byte[] sessionIdBytes;
        private ISessionService sessionService;
        private IDictionary<EncodedKey, byte[]> store;

        #endregion Fields

        #region Constructors

        public WebserviceSession(
            string sessionKey,
            TimeSpan idleTimeout,
            TimeSpan ioTimeout,
            Func<bool> tryEstablishSession,
            ILoggerFactory loggerFactory,
            bool isNewSessionKey,
            ISessionService sessionService)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentException("ArgumentCannotBeNullOrEmpty", nameof(sessionKey));
            }

            if (tryEstablishSession == null)
            {
                throw new ArgumentNullException(nameof(tryEstablishSession));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.sessionKey = sessionKey;
            this.idleTimeout = idleTimeout;
            this.ioTimeout = ioTimeout;
            this.tryEstablishSession = tryEstablishSession;
            this.store = new Dictionary<EncodedKey, byte[]>();
            this.logger = loggerFactory.CreateLogger<WebserviceSession>();
            this.isNewSessionKey = isNewSessionKey;
            this.sessionService = sessionService;
        }

        #endregion Constructors

        #region Properties

        public string Id
        {
            get
            {
                Load();
                if (this.sessionId == null)
                {
                    this.sessionId = new Guid(IdBytes).ToString();
                }
                return this.sessionId;
            }
        }

        public bool IsAvailable
        {
            get
            {
                Load();
                return this.isAvailable;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                Load();
                return this.store.Keys.Select(key => key.KeyString);
            }
        }

        #endregion Properties

        #region Methods

        public void Clear()
        {
            Load();
            this.isModified |= this.store.Count > 0;
            this.store.Clear();
        }

        #endregion Methods

        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var timeout = new CancellationTokenSource(this.ioTimeout))
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(timeout.Token, cancellationToken);
                if (this.isModified)
                {
                    if (this.logger.IsEnabled(LogLevel.Information))
                    {
                        // This operation is only so we can log if the session already existed.
                        // Log and ignore failures.
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            var data = await this.sessionService.Get(this.sessionKey, cts.Token);
                            if (data == null)
                            {
                                //this.logger.SessionStarted(this.sessionKey, Id);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        catch (Exception)
                        {
                            //this.logger.SessionCacheReadException(this.sessionKey, exception);
                        }
                    }

                    var stream = new MemoryStream();
                    Serialize(stream);

                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await this.sessionService.Set(this.sessionKey, stream.ToArray(), cts.Token);
                        this.isModified = false;
                        //this.logger.SessionStored(this.sessionKey, Id, this.store.Count);
                    }
                    catch (OperationCanceledException oex)
                    {
                        if (timeout.Token.IsCancellationRequested)
                        {
                            //this.logger.SessionCommitTimeout();
                            throw new OperationCanceledException("Timed out committing the session.", oex, timeout.Token);
                        }
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        await this.sessionService.Refresh(this.sessionKey, cts.Token);
                    }
                    catch (OperationCanceledException oex)
                    {
                        if (timeout.Token.IsCancellationRequested)
                        {
                            //this.logger.SessionRefreshTimeout();
                            throw new OperationCanceledException("Timed out refreshing the session.", oex, timeout.Token);
                        }
                        throw;
                    }
                }
            }
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!this.loaded)
            {
                using (var timeout = new CancellationTokenSource(this.ioTimeout))
                {
                    var cts = CancellationTokenSource.CreateLinkedTokenSource(timeout.Token, cancellationToken);
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        var data = await this.sessionService.Get(sessionKey, cts.Token);
                        if (data != null)
                        {
                            Deserialize(new MemoryStream(data));
                        }
                        else if (!this.isNewSessionKey)
                        {
                            // _logger.AccessingExpiredSession(this.sessionKey);
                        }
                    }
                    catch (OperationCanceledException oex)
                    {
                        if (timeout.Token.IsCancellationRequested)
                        {
                            //this.logger.SessionLoadingTimeout();
                            throw new OperationCanceledException("Timed out loading the session.", oex, timeout.Token);
                        }
                        throw;
                    }
                }
                this.isAvailable = true;
                this.loaded = true;
            }
        }

        public void Remove(string key)
        {
            Load();
            this.isModified |= this.store.Remove(new EncodedKey(key));
        }

        public void Set(string key, byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (IsAvailable)
            {
                var encodedKey = new EncodedKey(key);
                if (encodedKey.KeyBytes.Length > KeyLengthLimit)
                {
                    throw new ArgumentOutOfRangeException(nameof(key), "Exception_KeyLengthIsExceeded");
                }

                if (!this.tryEstablishSession())
                {
                    throw new InvalidOperationException("Exception_InvalidSessionEstablishment");
                }
                this.isModified = true;
                byte[] copy = new byte[value.Length];
                Buffer.BlockCopy(src: value, srcOffset: 0, dst: copy, dstOffset: 0, count: value.Length);
                this.store[encodedKey] = copy;
            }
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            Load();
            return this.store.TryGetValue(new EncodedKey(key), out value);
        }

        //        #endregion Methods

        #region Properties

        private byte[] IdBytes
        {
            get
            {
                if (this.IsAvailable && this.sessionIdBytes == null)
                {
                    this.sessionIdBytes = new byte[IdByteCount];
                    CryptoRandom.GetBytes(this.sessionIdBytes);
                }
                return this.sessionIdBytes;
            }
        }

        #endregion Properties

        #region Methods

        private void Deserialize(Stream content)
        {
            if (content == null || content.ReadByte() != SerializationRevision)
            {
                // Replace the un-readable format.
                this.isModified = true;
                return;
            }

            int expectedEntries = DeserializeNumFrom3Bytes(content);
            this.sessionIdBytes = ReadBytes(content, IdByteCount);

            for (int i = 0; i < expectedEntries; i++)
            {
                int keyLength = DeserializeNumFrom2Bytes(content);
                var key = new EncodedKey(ReadBytes(content, keyLength));
                int dataLength = DeserializeNumFrom4Bytes(content);
                this.store[key] = ReadBytes(content, dataLength);
            }

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.sessionId = new Guid(this.sessionIdBytes).ToString();
                //this.logger.SessionLoaded(_sessionKey, _sessionId, expectedEntries);
            }
        }

        private int DeserializeNumFrom2Bytes(Stream content)
        {
            return content.ReadByte() << 8 | content.ReadByte();
        }

        private int DeserializeNumFrom3Bytes(Stream content)
        {
            return content.ReadByte() << 16 | content.ReadByte() << 8 | content.ReadByte();
        }

        private int DeserializeNumFrom4Bytes(Stream content)
        {
            return content.ReadByte() << 24 | content.ReadByte() << 16 | content.ReadByte() << 8 | content.ReadByte();
        }

        private void Load()
        {
            if (!this.loaded)
            {
                try
                {
                    var data = this.sessionService.Get(this.sessionKey);
                    if (data.Result != null)
                    {
                        Deserialize(new MemoryStream(data.Result));
                    }
                    else if (!this.isNewSessionKey)
                    {
                        //this.logger.AccessingExpiredSession(this.sessionKey);
                    }
                    this.isAvailable = true;
                }
                catch (Exception)
                {
                    //this.logger.SessionCacheReadException(_sessionKey, exception);
                    this.isAvailable = false;
                    this.sessionId = string.Empty;
                    this.sessionIdBytes = null;
                }
                finally
                {
                    this.loaded = true;
                }
            }
        }

        private byte[] ReadBytes(Stream stream, int count)
        {
            var output = new byte[count];
            int total = 0;
            while (total < count)
            {
                var read = stream.Read(output, total, count - total);
                if (read == 0)
                {
                    throw new EndOfStreamException();
                }
                total += read;
            }
            return output;
        }

        private void Serialize(Stream output)
        {
            output.WriteByte(SerializationRevision);
            SerializeNumAs3Bytes(output, this.store.Count);
            output.Write(this.IdBytes, 0, IdByteCount);

            foreach (var entry in this.store)
            {
                var keyBytes = entry.Key.KeyBytes;
                SerializeNumAs2Bytes(output, keyBytes.Length);
                output.Write(keyBytes, 0, keyBytes.Length);
                SerializeNumAs4Bytes(output, entry.Value.Length);
                output.Write(entry.Value, 0, entry.Value.Length);
            }
        }

        private void SerializeNumAs2Bytes(Stream output, int num)
        {
            if (num < 0 || ushort.MaxValue < num)
            {
                //throw new ArgumentOutOfRangeException(nameof(num), Resources.Exception_InvalidToSerializeIn2Bytes);
                throw new ArgumentOutOfRangeException(nameof(num), "Exception_InvalidToSerializeIn3Bytes");
            }
            output.WriteByte((byte)(num >> 8));
            output.WriteByte((byte)(0xFF & num));
        }

        private void SerializeNumAs3Bytes(Stream output, int num)
        {
            if (num < 0 || 0xFFFFFF < num)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "Exception_InvalidToSerializeIn3Bytes");
            }
            output.WriteByte((byte)(num >> 16));
            output.WriteByte((byte)(0xFF & (num >> 8)));
            output.WriteByte((byte)(0xFF & num));
        }

        private void SerializeNumAs4Bytes(Stream output, int num)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "Exception_NumberShouldNotBeNegative");
                //throw new ArgumentOutOfRangeException(nameof(num), Resources.Exception_NumberShouldNotBeNegative);
            }
            output.WriteByte((byte)(num >> 24));
            output.WriteByte((byte)(0xFF & (num >> 16)));
            output.WriteByte((byte)(0xFF & (num >> 8)));
            output.WriteByte((byte)(0xFF & num));
        }

        #endregion Methods
    }
}