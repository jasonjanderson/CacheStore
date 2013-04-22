#region "Imports"

using System;

#endregion

namespace CacheStore
{
    public abstract class CacheKey
    {
        private string _cacheName;
        private DateTimeOffset _absoluteExpiration;
        private TimeSpan _slidingExpiration;

        #region "Properties"

        public string CacheName
        {
            get { return _cacheName; }
            set { _cacheName = value; }
        }

        public DateTimeOffset AbsoluteExpiration
        {
            get { return _absoluteExpiration; }
            set { _absoluteExpiration = value; }
        }

        public TimeSpan SlidingExpiration
        {
            get { return _slidingExpiration; }
            set { _slidingExpiration = value; }
        }

        #endregion

        #region "Constructors"

        public CacheKey()
        {
            _absoluteExpiration = DateTimeOffset.MaxValue;
            _slidingExpiration = TimeSpan.MaxValue;
        }

        public CacheKey(string cacheName) : this()
        {
            _cacheName = cacheName;
        }

        #endregion

        public abstract string GenerateKey();
    }
}
