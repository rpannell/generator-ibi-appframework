using System.Text;

namespace IBI.<%= Name %>.Application.Utils.Core.Models
{
    internal class EncodedKey
    {
        #region Fields

        private int? _hashCode;
        private string _keyString;

        #endregion Fields

        #region Constructors

        public EncodedKey(byte[] key)
        {
            KeyBytes = key;
        }

        internal EncodedKey(string key)
        {
            _keyString = key;
            KeyBytes = Encoding.UTF8.GetBytes(key);
        }

        #endregion Constructors

        #region Properties

        internal byte[] KeyBytes { get; private set; }

        internal string KeyString
        {
            get
            {
                if (_keyString == null)
                {
                    _keyString = Encoding.UTF8.GetString(KeyBytes, 0, KeyBytes.Length);
                }
                return _keyString;
            }
        }

        #endregion Properties

        #region Methods

        public override bool Equals(object obj)
        {
            var otherKey = obj as EncodedKey;
            if (otherKey == null)
            {
                return false;
            }
            if (KeyBytes.Length != otherKey.KeyBytes.Length)
            {
                return false;
            }
            if (_hashCode.HasValue && otherKey._hashCode.HasValue
                && _hashCode.Value != otherKey._hashCode.Value)
            {
                return false;
            }
            for (int i = 0; i < KeyBytes.Length; i++)
            {
                if (KeyBytes[i] != otherKey.KeyBytes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (!_hashCode.HasValue)
            {
                _hashCode = SipHash.GetHashCode(KeyBytes);
            }
            return _hashCode.Value;
        }

        public override string ToString()
        {
            return KeyString;
        }

        #endregion Methods
    }
}