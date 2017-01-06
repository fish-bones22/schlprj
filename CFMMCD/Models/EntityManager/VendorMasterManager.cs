using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class VendorMasterManager
    {
        /*
         * Combined Create and Update Menu Item method.
         * Creates a INVVEMP0 instance (which will be a new table row)
         * and instantiates each VMViewModel property to the respective property of the former.
         * Also checks if the given Vendor Number is already in the table,
         * if true, the method performs an update, otherwise, creation. 
         *
         * Returns true if the operation is successful.
         * */
        public bool UpdateVendor( VendorMasterViewModel VMViewModel, string user )
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                INVVEMP0 VMRow;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any())
                    VMRow = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));
                else
                    VMRow = new INVVEMP0();

                if (VMViewModel.VEMVEN != null)
                    VMRow.VEMVEN = int.Parse(VMViewModel.VEMVEN);
                else return false;

                if (VMViewModel.VEMDS1 != null)
                    VMRow.VEMDS1 = VMViewModel.VEMDS1.Trim();     // Trim() makes sure no additional whitespace at the start end end of the string
                else return false;

                VMRow.VEMLOC = VMViewModel.VEMLOC.Trim();
                VMRow.Region = VMViewModel.Region.Trim();
                VMRow.Province = VMViewModel.Province.Trim();
                VMRow.City = VMViewModel.City.Trim();
                VMRow.Store = VMViewModel.Store.Trim();

                if (VMViewModel.SelectAllCb)
                {
                    VMRow.Store = "ALL";
                }
                if (VMViewModel.SelectExceptCb)
                {
                    VMRow.Store = "ALL";
                    VMRow.Except_Store = VMViewModel.Store.Trim();
                }
                else
                {
                    VMRow.Except_Store = null;
                }

                VMRow.VEMDAT = DateTime.Now;
                VMRow.VEMUSR = user.Substring(0, 3).ToUpper();

                try
                {    // Perform an update if Vendor number already exists
                    if ( db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any() )
                    {
                       // INVVEMP0 rowToDelete = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));
                        VMRow.STATUS = "E";
                        //db.INVVEMP0.Remove(rowToDelete);
                        //db.INVVEMP0.Add(VMRow);
                    }
                    else
                    {
                        VMRow.STATUS = "A";
                        db.INVVEMP0.Add(VMRow);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
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
        /*
         * Deletes the specified row given in the ViewModel properties.
         * 
         * Returns true if operation is successful.
         * */
        public bool DeleteVendor( VendorMasterViewModel VMViewModel )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                try
                {
                    if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any())
                    {
                        INVVEMP0 rowToDelete = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));
                        db.INVVEMP0.Remove(rowToDelete);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch ( Exception e )
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

        public List<VendorMasterViewModel> SearchVendors (string SearchItem)
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<VendorMasterViewModel> VMList = new List<VendorMasterViewModel>();
                List<INVVEMP0> VMRowList;

                if ( SearchItem == null || SearchItem.Equals("") )
                    return null;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).ToList();
                else if (SearchItem.ToUpper().Equals("ALL"))
                    VMRowList = db.INVVEMP0.ToList();
                else if (db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(SearchItem)).ToList();
                else
                    return null;
                foreach ( INVVEMP0 VMRow in VMRowList )
                {
                    VendorMasterViewModel vm = SearchSingleVendor(VMRow.VEMVEN.ToString());
                    if (vm != null)
                        VMList.Add(vm);
                }
                if (VMList == null || VMList.ElementAt(0) == null)
                    return null;
                return VMList;
            }
        }

        public VendorMasterViewModel SearchSingleVendor(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                INVVEMP0 VMRow;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(SearchItem)).Any())
                    VMRow = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(SearchItem));
                else
                    return null;
                VendorMasterViewModel vm = new VendorMasterViewModel();
                vm.VEMVEN = VMRow.VEMVEN.ToString();
                vm.VEMDS1 = VMRow.VEMDS1.Trim();
                vm.VEMLOC = VMRow.VEMLOC;
                vm.Store = VMRow.Store;
                vm.Region = VMRow.Region;
                vm.Province = VMRow.Province;
                vm.City = VMRow.City;
                if ((VMRow.Store != null) && VMRow.Store.Equals("ALL"))
                {
                    vm.SelectAllCb = true;
                    vm.Store = "";
                }
                if ((VMRow.Except_Store != null) && !(VMRow.Except_Store.Equals("")))
                    vm.SelectExceptCb = true;
                return vm;
            }
        }
    }
}