using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class ItemGroupViewModel
    {
        public ItemGroupViewModel()
        {
            ItemList = new List<Item>();
            AllStoreList = new List<StoreInformation>();
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }
        public int StoreNo { get; set; }
        public string StoreName { get; set; }

        public List<Item> ItemList { get; set; } 
        public List<StoreInformation> AllStoreList { get; set; }
        public List<MenuItem> MenuItemList { get; set; }
        public List<RawItem> RawItemList { get; set; }
        public List<ItemGroupViewModel> GroupList { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}