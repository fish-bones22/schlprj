using CFMMCD.Models.EntityManager;
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
            StoreList = new List<Item>();
            AllStoreList = new StoreProfileManager().SearchStores("ALL");
            MenuItemList = MenuItemMasterManager.SearchMenuItems("ALL");
            RawItemList = RawItemMasterManager.GetRawItems("ALL");
            RecipeList = MenuRecipeManager.SearchMenuItem("ALL");
            GroupList = new List<ItemGroupViewModel>();
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupType { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }

        public List<Item> ItemList { get; set; }
        public List<Item> StoreList { get; set; }
        public List<Store> AllStoreList { get; set; }
        public List<MenuItem> MenuItemList { get; set; }
        public List<RawItem> RawItemList { get; set; }
        public List<MenuItem> RecipeList { get; set; }
        public List<ItemGroupViewModel> GroupList { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int ItemId { get; set; }
        public int ItemType { get; set; }
        public int Id { get; set; }

    }
}