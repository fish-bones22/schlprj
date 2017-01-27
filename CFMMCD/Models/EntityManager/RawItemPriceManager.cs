using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CFMMCD.Models.EntityManager
{
    public class RawItemPriceManager
    {
        /*
         * Overload method to accept List as parameter.
         * */
        public static bool UpdateRawItemPrice(List<RawItemPriceUpdateViewModel> RIPViewModelList)
        {
            bool result;
            foreach (var vm in RIPViewModelList)
            {
                result = UpdateRawItemPrice(vm);
                if (!result) return result;
            }
            return true;
        }

        public static bool UpdateRawItemPrice(RawItemPriceUpdateViewModel RIPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                RIM_VEM_Lookup RVLRow;
                if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID)).Any())
                    RVLRow = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID));
                else
                {
                    RVLRow = new RIM_VEM_Lookup();
                }

                RVLRow.RIM_VEM_ID = RIPViewModel.RIM_VEM_ID;
                RVLRow.RIMRIC = int.Parse(RIPViewModel.RIMRIC);
                RVLRow.VEMVEN = int.Parse(RIPViewModel.VEMVEN);
                RVLRow.VEMDS1 = RIPViewModel.VEMDS1.Trim();
                RVLRow.RIMCPR = double.Parse(RIPViewModel.RIMCPR);
                RVLRow.RIMRID = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(RIPViewModel.RIMRIC)).RIMRID;
                //RVLRow.RIMPDT = DateTime.Parse(RIPViewModel.RIMPDT);
                //RVLRow.RIMCPN = double.Parse(RIPViewModel.RIMCPN);

                try
                {
                    if (!db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID)).Any())
                        db.RIM_VEM_Lookup.Add(RVLRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        }
        public static bool DeleteRawItemPrice(RawItemPriceUpdateViewModel RIPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                try
                {
                    if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID)).Any())
                    {
                        RIM_VEM_Lookup rowToRemove = db.RIM_VEM_Lookup.FirstOrDefault(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID));
                        db.RIM_VEM_Lookup.Remove(rowToRemove);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        }
        public static List<RawItemPriceUpdateViewModel> SearchRawItemPrice(RawItemPriceUpdateViewModel RIPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItemPriceUpdateViewModel> RIPList = new List<RawItemPriceUpdateViewModel>();
                List<RIM_VEM_Lookup> RIPRowList;
                if (RIPViewModel.SearchItem == null || RIPViewModel.SearchItem.Equals(""))
                    return null;
                if (db.RIM_VEM_Lookup.Where(o => o.RIMRID.Contains(RIPViewModel.SearchItem)).Any())
                    RIPRowList = db.RIM_VEM_Lookup.Where(o => o.RIMRID.Equals(RIPViewModel.SearchItem)).ToList();
                else if (db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(RIPViewModel.SearchItem)).Any())
                    RIPRowList = db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(RIPViewModel.SearchItem)).ToList();
                else if (RIPViewModel.SearchItem.ToUpper().Equals("ALL"))
                    RIPRowList = db.RIM_VEM_Lookup.ToList();
                else
                    return null;
                foreach (RIM_VEM_Lookup rim in RIPRowList)
                {
                    RawItemPriceUpdateViewModel vm = new RawItemPriceUpdateViewModel();
                    vm.RIM_VEM_ID = rim.RIM_VEM_ID;
                    if (rim.RIMRIC != null)
                        vm.RIMRIC = rim.RIMRIC.ToString();
                    if (rim.VEMVEN != null)
                        vm.VEMVEN = rim.VEMVEN.ToString();
                    if (rim.VEMDS1 != null)
                        vm.VEMDS1 = rim.VEMDS1.Trim();
                    if (rim.RIMCPR != null)
                        vm.RIMCPR = rim.RIMCPR.ToString();
                    if (rim.RIMRID != null)
                        vm.RIMRID = rim.RIMRID.ToString();
                    string Status = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(rim.RIMRIC.ToString())).RIMSTA;
                    if (Status != null && Status.Equals("0"))
                        vm.RIMSTA = true;

                    RIPList.Add(vm);
                }
                if (RIPList == null || RIPList.ElementAt(0) == null)
                    return null;
                return RIPList;
            }
        }
        public static bool ImportExcel(Stream file)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                using (XLWorkbook workBook = new XLWorkbook(file))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    
                    var RIPViewModelList = new List<RawItemPriceUpdateViewModel>();
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        if (firstRow)
                        {
                            firstRow = false;
                        }
                        else
                        {
                            if (row.Cells() == null || row.Cells().Count() <= 0)
                                continue;
                            RawItemPriceUpdateViewModel vm = new RawItemPriceUpdateViewModel();
                            vm.RIMRIC = row.Cells().ElementAt(0).Value.ToString();
                            vm.RIMRID = row.Cells().ElementAt(1).Value.ToString();
                            vm.RIMCPR = row.Cells().ElementAt(2).Value.ToString();
                            vm.VEMDS1 = row.Cells().ElementAt(3).Value.ToString();
                            if (!db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(vm.RIMRIC)).Any())
                                continue;
                            if (!db.INVVEMP0.Where(o => o.VEMDS1.Equals(vm.VEMDS1)).Any())
                                continue;
                            vm.VEMVEN = db.INVVEMP0.FirstOrDefault(o => o.VEMDS1.Equals(vm.VEMDS1)).VEMVEN.ToString();
                            vm.RIM_VEM_ID = vm.RIMRIC + vm.VEMVEN;
                            if (!db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(vm.RIM_VEM_ID)).Any())
                                continue;
                            RIPViewModelList.Add(vm);
                        }
                    }
                    return UpdateRawItemPrice(RIPViewModelList);
                }
            }
        }
    }
}