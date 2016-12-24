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
                INVVEMP0 VMRow = new INVVEMP0();
                VMRow.VEMVEN = int.Parse(VMViewModel.VEMVEN);
                VMRow.VEMWSI = int.Parse(VMViewModel.VEMWSI);
                VMRow.VEMDS1 = VMViewModel.VEMDS1.Trim();       // Trim() makes sure no additional whitespace at the start end end of the string
                VMRow.VEMDS2 = "";
                VMRow.VEMCCD = VMViewModel.VEMCCD.Trim();
                VMRow.VEMZIP = VMViewModel.VEMZIP.Trim();
                VMRow.VEMCTY = VMViewModel.VEMCTY.Trim();
                VMRow.VEMSTR = VMViewModel.VEMSTR.Trim();
                VMRow.VEMTEL = VMViewModel.VEMTEL.Trim();
                VMRow.VEMSTN = VMViewModel.VEMSTN.Trim();
                VMRow.VEMLOC = VMViewModel.VEMLOC;
                VMRow.VEMDAY = int.Parse(VMViewModel.VEMDAY);
                VMRow.VEMTID = VMViewModel.VEMTID.Trim();
                VMRow.VEMSTA = VMViewModel.VEMSTA;
                VMRow.VEMDAT = DateTime.Now;
                VMRow.VEMUSR = user.Substring(0, 3).ToUpper();
                VMRow.VEMADE = VMViewModel.VEMADE.Trim();
                VMRow.VEMDEL = VMViewModel.VEMDEL.Trim();

                try
                {    // Perform an update if Vendor number already exists
                    if ( db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN)).Any() )
                    {
                        INVVEMP0 rowToDelete = db.INVVEMP0.Single(o => o.VEMVEN.ToString().Equals(VMViewModel.VEMVEN));
                        VMRow.STATUS = "E";
                        db.INVVEMP0.Remove(rowToDelete);
                        db.INVVEMP0.Add(VMRow);
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
                    return false;
                }
            }
        }

        public List<VendorMasterViewModel> SearchVendor ( VendorMasterViewModel VMViewModel )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<VendorMasterViewModel> VMList = new List<VendorMasterViewModel>();
                List<INVVEMP0> VMRowList;

                if ( VMViewModel.SearchItem == null || VMViewModel.SearchItem.Equals("") )
                    return null;
                if (db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMVEN.ToString().Equals(VMViewModel.SearchItem)).ToList();
                else if (db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(VMViewModel.SearchItem)).Any())
                    VMRowList = db.INVVEMP0.Where(o => o.VEMDS1.ToString().Contains(VMViewModel.SearchItem)).ToList();
                else
                    return null;
                foreach ( INVVEMP0 VMRow in VMRowList )
                {
                    // Check if 'Include inactive items' is checked
                    if (VMViewModel.InactiveItemsCb || VMRow.VEMSTA.Equals("0"))
                    {
                        VendorMasterViewModel vm = new VendorMasterViewModel();
                        vm.VEMVEN = VMRow.VEMVEN.ToString();
                        vm.VEMWSI = VMRow.VEMWSI.ToString();
                        vm.VEMDS1 = VMRow.VEMDS1.Trim();
                        vm.VEMCCD = VMRow.VEMCCD.Trim();
                        vm.VEMZIP = VMRow.VEMZIP.Trim();
                        vm.VEMCTY = VMRow.VEMCTY.Trim();
                        vm.VEMSTR = VMRow.VEMSTR.Trim();
                        vm.VEMTEL = VMRow.VEMTEL.Trim();
                        vm.VEMSTN = VMRow.VEMSTN.Trim();
                        vm.VEMLOC = VMRow.VEMLOC;
                        vm.VEMDAY = VMRow.VEMDAY.ToString();
                        vm.VEMTID = VMRow.VEMTID.Trim();
                        vm.VEMSTA = VMRow.VEMSTA;
                        vm.VEMDAT = ((DateTime)VMRow.VEMDAT).ToString("yyyy-MM-dd");
                        vm.VEMADE = VMRow.VEMADE.Trim();
                        vm.VEMDEL = VMRow.VEMDEL.Trim();
                        VMList.Add(vm);
                    }
                }
                if (VMList == null || VMList.ElementAt(0) == null)
                    return null;
                return VMList;
            }
        }
    }
}