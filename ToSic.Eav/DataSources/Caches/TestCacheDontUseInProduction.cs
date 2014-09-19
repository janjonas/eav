using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToSic.Eav.DataSources.Caches
{
    class TestCacheDontUseInProduction: BaseDataSource
    {
        private string CacheKeyPrefix = "ToSic.Eav.Cache.Fragment:";
        private string CacheKey = "[Module:ModuleId]";
        private string StreamsToCache = "Default";

        private int RefreshInterval = 60*60*24; // 1 day in seconds
        private bool RefreshIfSourceIsRefreshed = false;    // this is up to you...


        
		/// <summary>
		/// Constructs a new EntityTypeFilter
		/// </summary>
        public TestCacheDontUseInProduction()
		{
			Out.Add(DataSource.DefaultStreamName, new DataStream(this, DataSource.DefaultStreamName, GetEntities));
			// Configuration.Add(ShowDraftsKey, "[Settings:ShowDrafts]");
		}

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
