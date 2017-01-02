using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ValueMealManager
    {
        public List<ValueMealViewModel> SearchValueMeal(ValueMealViewModel VMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ValueMealViewModel> VMList = new List<ValueMealViewModel>();
                List<CSHVMLP0> MIVMowList;
                if (VMViewModel.SearchItem == null || VMViewModel.SearchItem.Equals(""))
                    return null;
                if (db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(VMViewModel.SearchItem)).Any())
                {
                    MIVMowList = db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(VMViewModel.SearchItem)).ToList();
                }
                else if (db.CSHVMLP0.Where(o => o.VMLNAM.Trim().Equals(VMViewModel.SearchItem)).Any())
                {
                    MIVMowList = db.CSHVMLP0.Where(o => o.VMLNAM.Trim().Equals(VMViewModel.SearchItem)).ToList();
                }
                else
                    return null;
                foreach (CSHVMLP0 rim in MIVMowList)
                {
                    ValueMealViewModel vm = new ValueMealViewModel();
                    vm.VMLNUM = rim.VMLNUM.ToString();
                    vm.VMLNAM = rim.VMLNAM.Trim();
                    vm.VMLPRI = rim.VMLPRI.ToString();
                    vm.VMLPRO = rim.VMLPRI.ToString();
                    vm.MenuItemList = SearchVMMenuItem(vm.VMLNUM);
                    VMList.Add(vm);
                }
                if (VMList == null || VMList.Count == 0)
                    return null;
                return VMList;
            }
        }

        public List<VMMenuItem> SearchVMMenuItem(string ValueMealNumber)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHVMLP0> VMRowList = new List<CSHVMLP0>();
                List<VMMenuItem> MIList = new List<VMMenuItem>();
                if (db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(ValueMealNumber)).Any())
                {
                    VMRowList = db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(ValueMealNumber)).ToList();
                }
                foreach (var v in VMRowList)
                {
                    VMMenuItem mi = new VMMenuItem();
                    mi.MIMMIC = v.VMLMIC.ToString();
                    mi.MIMNAM = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(mi.MIMMIC)).MIMNAM;
                    if (db.Tier_Lookup.Where(o => o.MIMMIC.ToString().Equals(mi.MIMMIC)).Any())
                    {
                        mi.MIMPRI = db.Tier_Lookup.Single(o => o.MIMMIC.ToString().Equals(mi.MIMMIC)).OLDPRA.ToString();
                        mi.MIMPRO = db.Tier_Lookup.Single(o => o.MIMMIC.ToString().Equals(mi.MIMMIC)).OLDPAO.ToString();
                    }
                    else
                    {
                        mi.MIMPRI = "";
                        mi.MIMPRO = "";
                    }
                    mi.VMLQUA = v.VMLQUA.ToString();
                    MIList.Add(mi);
                }
                return MIList;
            }
        }

        public bool UpdateValueMeal(ValueMealViewModel VMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (VMViewModel.VMLNUM == null || VMViewModel.VMLNUM.Equals(""))
                    return false;
                // Existing
                foreach (var v in VMViewModel.MenuItemList)
                {
                    CSHVMLP0 VMRow = new CSHVMLP0();
                    VMRow.VMLID = VMViewModel.VMLNUM + v.MIMMIC;
                    VMRow.VMLNUM = int.Parse(VMViewModel.VMLNUM);
                    VMRow.VMLNAM = VMViewModel.VMLNAM.ToString();
                    VMRow.VMLMIC = int.Parse(v.MIMMIC);
                    VMRow.VMLQUA = int.Parse(v.VMLQUA);
                    VMRow.VMLPRI = double.Parse(VMViewModel.VMLPRI);
                    VMRow.VMLPRO = double.Parse(VMViewModel.VMLPRO);
                    try
                    {
                        if (db.CSHVMLP0.Where(o => o.VMLID == VMRow.VMLID).Any())
                        {
                            CSHVMLP0 rowToDelete = db.CSHVMLP0.Single(o => o.VMLID == VMRow.VMLID);
                            db.CSHVMLP0.Remove(rowToDelete);
                            VMRow.STATUS = "E";
                            db.CSHVMLP0.Add(VMRow);
                        }
                        else
                        {
                            VMRow.STATUS = "A";
                            db.CSHVMLP0.Add(VMRow);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                // New
                for (int i = 0; i < VMViewModel.MIMMIC.Count(); i++)
                {
                    CSHVMLP0 VMRow = new CSHVMLP0();
                    VMRow.VMLID = VMViewModel.VMLNUM + VMViewModel.MIMMIC[i];
                    VMRow.VMLNUM = int.Parse(VMViewModel.VMLNUM);
                    VMRow.VMLNAM = VMViewModel.VMLNAM.ToString();
                    VMRow.VMLMIC = int.Parse(VMViewModel.MIMMIC[i]);
                    VMRow.VMLQUA = int.Parse(VMViewModel.VMLQUA[i]);
                    VMRow.VMLPRI = double.Parse(VMViewModel.VMLPRI);
                    VMRow.VMLPRO = double.Parse(VMViewModel.VMLPRO);
                    try
                    {
                        if (db.CSHVMLP0.Where(o => o.VMLID == VMRow.VMLID).Any())
                        {
                            CSHVMLP0 rowToDelete = db.CSHVMLP0.Single(o => o.VMLID == VMRow.VMLID);
                            db.CSHVMLP0.Remove(rowToDelete);
                            VMRow.STATUS = "E";
                            db.CSHVMLP0.Add(VMRow);
                        }
                        else
                        {
                            VMRow.STATUS = "A";
                            db.CSHVMLP0.Add(VMRow);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}