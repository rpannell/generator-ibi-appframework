using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Cache
{
    public class WebserviceCache : IDistributedCache
    {
        #region Fields

        private readonly WebserviceCacheOptions cacheOptions;
        private readonly CacheService cacheService;
        private readonly MemoryCache memoryCache;

        #endregion Fields

        #region Constructors

        public WebserviceCache(IOptions<WebserviceCacheOptions> cacheOptions, IMemoryCache memoryCache)
        {
            this.memoryCache = new MemoryCache(new MemoryCacheOptions() { });
            this.cacheOptions = cacheOptions.Value;
            this.cacheService = new CacheService(this.cacheOptions.WebServiceURL);
        }

        #endregion Constructors

        #region Methods

        public byte[] Get(string key)
        {
            return this.cacheService.Get(this.GetKey(key));
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return await this.cacheService.Get(this.GetKey(key), token);
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            this.cacheService.Delete(this.GetKey(key));
        }

        public async Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            await this.cacheService.Delete(this.GetKey(key), token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            this.cacheService.Set(this.GetKey(key), value);
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            await this.SetAsync(this.GetKey(key), value, token);
        }

        #endregion Methods

        private string GetKey(string key)
        {
            return string.Format("{0}:{1}", this.cacheOptions.InstanceName, key);
        }
    }
}