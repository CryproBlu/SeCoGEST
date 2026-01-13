using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;

namespace SeCoGEST.Infrastructure
{
    public class InformazioniSessione
    {
        /// <summary>
        /// Verifica la presenza del file di blocco degli accessi, se è esiste torno true
        /// </summary>
        /// <returns></returns>
        public static bool BloccaAccessi()
        {
            bool bloccaAccessi = File.Exists(ConfigurationKeys.PERCORSO_FILE_BLOCCO_ACCESSI);
            return bloccaAccessi;
        }


        public static void SetCacheData(string key, object data, int slidingExpirationMinutes)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItem item = cache.GetCacheItem(key);

            if (item == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes);
                item = new CacheItem(key, data);
                cache.Set(item, policy);
            }
        }
        public static object GetCacheData(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItem item = cache.GetCacheItem(key);

            if (item != null)
            {
                return item.Value;
            }
            return null;
        }

    }
}
