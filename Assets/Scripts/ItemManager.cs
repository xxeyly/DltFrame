using System.Collections.Generic;
using LitJson;

namespace DefaultNamespace
{
    public class ItemManager
    {
        public List<CatalogItemsInfo> CatalogItemsInfos = new List<CatalogItemsInfo>();

        public void Start()
        {
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 3, Items = new List<Item>()});
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 4, Items = new List<Item>()});
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 5, Items = new List<Item>()});
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 6, Items = new List<Item>()});
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 7, Items = new List<Item>()});
            CatalogItemsInfos.Add(new CatalogItemsInfo() {itemParentIndex = 1, itemGroupIndex = 8, Items = new List<Item>()});
            Item item = new Item();

            string json = JsonMapper.ToJson(item);

            item = JsonMapper.ToObject<Item>(json);
        }
    }

    public struct CatalogItemsInfo
    {
        public int itemParentIndex;
        public int itemGroupIndex;
        public List<Item> Items;
    }

    public class Item
    {
        public int itemParentIndex;
        public int itemIndex;
    }
}