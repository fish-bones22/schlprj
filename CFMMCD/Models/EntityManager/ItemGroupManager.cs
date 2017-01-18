using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ItemGroupManager
    {
        public static List<ItemGroupViewModel> GetGroup(ItemGroupViewModel IGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ITMGRP> IGRows;
                List<ItemGroupViewModel> IGList = IGViewModel.GroupList;
                if (db.ITMGRPs.Where(o => o.Item_Type == IGViewModel.ItemType).Any())
                {
                    IGRows = db.ITMGRPs.Where(o => o.Item_Type == IGViewModel.ItemType).ToList();
                    for (int i = 0; i < IGRows.Count(); i++)
                    {
                        ItemGroupViewModel vm = new ItemGroupViewModel();
                    }
                }
                return IGViewModel.GroupList;
            }
        }
    }
}