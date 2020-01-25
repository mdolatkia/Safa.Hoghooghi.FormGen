using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCacheManager
{
    public class CacheManager
    {
        static CacheManager _CacheManager = null;
        public static CacheManager GetCacheManager()
        {
            if (_CacheManager == null)
                _CacheManager = new MyCacheManager.CacheManager();
            return _CacheManager;
        }

        private List<CacheItem> cacheItems = new List<CacheItem>();
        public void AddCacheItem(object value, CacheItemType itemType, string itemName, params string[] keys)
        {
            CacheItem cacheItem = null;
            if (keys.Count(x => !string.IsNullOrEmpty(x)) > 0)
                cacheItem = cacheItems.ToList().FirstOrDefault(x => x.ItemType == itemType && x.ItemName == itemName && keys.Where(k => !string.IsNullOrEmpty(k)).All(k => x.IncludingAttributes.Any(z => z == k)));
            else
                cacheItem = cacheItems.ToList().FirstOrDefault(x => x.ItemType == itemType && x.ItemName == itemName);
            if (cacheItem == null)
            {
                CacheItem newItem = new MyCacheManager.CacheItem(itemType, itemName, value, keys);
                cacheItems.Add(newItem);
            }
            else
                cacheItem.Value = value;
        }

        public object GetCachedItem(CacheItemType itemType, string itemName, params string[] keys)
        {
           
            if (keys.Count(x => !string.IsNullOrEmpty(x)) > 0)
                return cacheItems.FirstOrDefault(x => x.ItemType == itemType && x.ItemName == itemName && keys.Where(k => !string.IsNullOrEmpty(k)).All(y => x.IncludingAttributes.Any(z => z == y)))?.Value;
            else
                return cacheItems.FirstOrDefault(x => x.ItemType == itemType && x.ItemName == itemName)?.Value;
        }
    }
    public class CacheItem
    {
        public CacheItem(CacheItemType itemType, string itemName, object value, params string[] includingAttributes)
        {
            ItemType = itemType;
            ItemName = itemName;
            IncludingAttributes = includingAttributes.ToList();
            Value = value;
        }
        public List<string> IncludingAttributes { set; get; }

        public CacheItemType ItemType { set; get; }

        public string ItemName { set; get; }
        public object Value { set; get; }
    }
    public enum CacheItemType
    {
        Entity,
        EntityUICompositionTree,
        Column,
        Relationship,
        Permission,
        ConditionalPermission,
        EntityDirectSecurity,
        EntityGeneralDirectSecurity,
        DataItems,
        Validation,
        Command


    }
}
