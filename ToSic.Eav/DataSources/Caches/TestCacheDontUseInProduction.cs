using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToSic.Eav.DataSources.Caches
{
    class TestCacheDontUseInProduction: BaseDataSource
    {
        // todo:
        // * ensure that it's cached in a "system cache' not directly in the application, to make sure that it's in the right system no matter what cache is officially used
        // * ensure that it knows when the cache was refreshed last
        // * its own cache entry must also have a time-stamp
        // * find a clear cache-key concept
        // * ensure that the cache-key can receive app-id and zone-id

        #region Cache Configuration
        private bool _cacheEnabled = false;
        private string _cacheEnabledKey = "CacheEnabled";
        public bool CacheEnabled
        {
            get { return Convert.ToBoolean(Configuration[_cacheEnabledKey]); }
            set { Configuration[_cacheEnabledKey] = value.ToString(); }
        }

        private string _defaultCachedStreams = "Default";
        private string _cacheStreamsKey = "CachedStreams";
        public string CachedStreams
        {
            get { return Configuration[_cacheStreamsKey]; }
            set { Configuration[_cacheStreamsKey] = value; }

        }
        #endregion

        #region CacheRefreshConfiguration

        private int _defaultCacheRefreshInterval = 60 * 60 * 24; // 1 day in seconds
        private string _cacheRefreshIntervalKey = "CacheRefreshInterval";
        /// <summary>
        /// Refresh interval for the cache - in seconds
        /// </summary>
        public int CacheRefreshInterval
        {
            get { return Convert.ToInt32(Configuration[_cacheRefreshIntervalKey]); }
            set { Configuration[_cacheRefreshIntervalKey] = value.ToString(); }
        }

        private bool _defaultCacheRefreshIfSourceIsRefreshed = false;    // this is up to you...
        private string _cacheRefreshWithSourceKey = "RefreshCacheIfSourceIsRefreshed";
        public bool RefreshCacheIfSourceIsRefreshed
        {
            get { return Convert.ToBoolean(Configuration[_cacheRefreshWithSourceKey]); }
            set { Configuration[_cacheRefreshWithSourceKey] = value.ToString(); }
        }
        #endregion

        #region CacheKey System
        // The cache Key is built of various parameters which might not always be the same
        // ...because it's critical that the cache works, but also that differenc caches, like
        // ...caches that vary by URL param, still work too...
        // Elements of the cache key
        // 1. An initial prefix "ToSic.Eav.Cache.Fragment:" to ensure that it will never conflict with anything else in the cache
        // 2. the name of this data source - so that inherited caches will use another naming pattern
        // App/Zone info (99.9% this should be the case...)
        // 2. ...the rest...

        // Typically "the rest" could be any parts or combinations of...
        // * ModuleId (when something varies by module, like an RSS-Feed App; but not always, when each view needs the same cache...)
        // * Certain URL parameters (based on a filter - this could either be one cache, and the filter aplied at the end, or multiple caches)
        // * User-information (user ID...)
        // * A value from the configuration-entity
        // * something custom

        private string _cacheKeyPatternKey = "CacheKeyPattern";
        private string _cacheKeyPrefix = "ToSic.Eav.Cache.Fragment:";
        private string _cacheKeyPattern = "[App:ZoneId]-[App:AppId]:[Module:ModuleId]";
        public string CacheKeyPattern
        {
            get { return Configuration[_cacheKeyPatternKey]; }
            set { Configuration[_cacheKeyPatternKey] = value; }

        }

        private string _cacheKey;
        /// <summary>
        /// The internal key used in the cache. Must be unique, but must be the same if every another code needs the same cache
        /// </summary>
        public string CacheKey
        {
            get
            {
                if (string.IsNullOrEmpty(_cacheKey))
                    _cacheKey = _cacheKeyPrefix + ":" + Name + ":" + ConfigurationProvider.LoadConfiguration(CacheKeyPattern);
                return _cacheKey;

            }
        }
        #endregion


        public bool CachePopulated
        {
            get { return false; }
        }

        public DateTime CacheTimestamp { get; private set; }

        private bool enforceRefresh = false;
        public bool CacheMustRefresh
        {
            get
            {
                // if manually requsting refresh, do it
                if (enforceRefresh)
                    return true;
                // todo: check if chache content is too old

                // todo: check if upstream-check enabled, and if 
                // if (RefreshCacheIfSourceIsRefreshed)
                //   if(...)



                return true;
            }
            set { enforceRefresh = value; }
        }

        public bool RefreshCache()
        {
            return PopulateCache();
        }

        /// <summary>
        /// Loads all configured streams into the cache
        /// </summary>
        public TestCacheDontUseInProduction()
		{
			Out.Add(DataSource.DefaultStreamName, new DataStream(this, DataSource.DefaultStreamName, GetEntities));
			Configuration.Add(_cacheKeyPatternKey, _cacheKeyPattern);
		    CacheKeyPattern = _cacheKeyPattern;
		    CacheRefreshInterval = _defaultCacheRefreshInterval;
            RefreshCacheIfSourceIsRefreshed = _defaultCacheRefreshIfSourceIsRefreshed;
            CachedStreams = _defaultCachedStreams;
            CacheEnabled = _cacheEnabled;
		}
        /// <returns></returns>
        public bool PopulateCache()
        {
            // todo: load all parent data and place in cache...
            CacheTimestamp = DateTime.Now;
            return true;
        }

        public bool ClearCache(){
        
            // todo: delete stuff in cache
            CacheMustRefresh = true;
            CacheTimestamp = DateTime.MinValue;
            return true;
        }
        
		/// <summary>
		/// Constructs a new EntityTypeFilter
		/// </summary>

        private IDictionary<int, IEntity> GetEntities()
        {
            EnsureConfigurationIsLoaded();

            var outStreamName = true ? BaseCache.DraftsStreamName : BaseCache.PublishedStreamName;
            return In[outStreamName].List;
        }

        // todo
        // basically work like a passthrough - (on which streams?)
        // but cache everything as soon as possible (where to? maybe put it in some global cache?)
        // make sure I have some kind of unique ID to know which cache to access

        // then always retrieve cache...
        // unless...
        // ...time has run out
        // ...or the upstream says it's over...


        // Unique cache ID should consist of 
        // ...either a fixed string (so the cache could be re-used by multiple locations)
        // ...or a fixed setup
        // ...a fix section to say "TemporaryCache"
        // ...some parameters like
        // "ToSic.Eav.Cache:[Module:ModuleId]-[UpstreamName]"
    }
}
