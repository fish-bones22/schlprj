using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ValueMealManager
    {
        public List<ValueMeal> SearchValueMeals(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ValueMeal> VMList = new List<ValueMeal>();
                List<CSHVMLP0> MIVMowList;
                if (SearchItem.ToUpper().Equals("ALL"))
                {
                    MIVMowList = db.CSHVMLP0.ToList();
                }
                else if (db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(SearchItem)).Any())
                {
                    MIVMowList = db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(SearchItem)).ToList();
                }
                else if (db.CSHVMLP0.Where(o => o.VMLNAM.Trim().Equals(SearchItem)).Any())
                {
                    MIVMowList = db.CSHVMLP0.Where(o => o.VMLNAM.Trim().Equals(SearchItem)).ToList();
                }
                else
                    return null;
                string previousVML = "";
                foreach (CSHVMLP0 rim in MIVMowList.OrderBy(o => o.VMLNAM))
                {
                    if (previousVML.Equals(rim.VMLNAM.Trim()))
                        continue;
                    ValueMeal vm = new ValueMeal();
                    vm.VMLID = rim.VMLID.Trim();
                    vm.VMLNUM = rim.VMLNUM.ToString();
                    vm.VMLNAM = rim.VMLNAM.Trim();
                    VMList.Add(vm);
                    previousVML = rim.VMLNAM.Trim();
                }
                if (VMList == null || VMList.Count == 0)
                    return null;
                return VMList;
            }
        }

        public ValueMealViewModel SearchSingleValueMeal( string SearchItem )
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHVMLP0 VMRow;
                if (db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(SearchItem)).Any())
                {
                    VMRow = db.CSHVMLP0.FirstOrDefault(o => o.VMLNUM.ToString().Equals(SearchItem));
                }
                else if (db.CSHVMLP0.Where(o => o.VMLID.ToString().Equals(SearchItem)).Any())
                {
                    VMRow = db.CSHVMLP0.FirstOrDefault(o => o.VMLID.ToString().Equals(SearchItem));
                }
                else return null;
                ValueMealViewModel vm = new ValueMealViewModel();
                vm.VMLNUM = VMRow.VMLNUM.ToString();
                vm.VMLNAM = VMRow.VMLNAM.Trim();
                vm.VMLPRI = VMRow.VMLPRI.ToString();
                vm.VMLPRO = VMRow.VMLPRI.ToString();
                vm.MenuItemList = SearchVMMenuItem(vm.VMLNUM);
                return vm;
            }
        }

        // Searches Menu Items included in a value meal
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
                    string id = mi.MIMMIC + "A";
                    if (db.MIM_Price.Where(o => o.Id.Equals(id)).Any())
                    {
                        mi.MIMPRI = db.MIM_Price.Single(o => o.Id.Equals(id)).MIMPRI.ToString();
                        mi.MIMPRO = db.MIM_Price.Single(o => o.Id.Equals(id)).MIMPRO.ToString();
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
                    // Delete overwritten MIMMIC entry
                    if (!v.PreviousMIMMIC.Equals(v.MIMMIC) && db.CSHVMLP0.Where(o => o.VMLNUM.ToString().Equals(VMViewModel.VMLNUM)).Any())
                    {
                        CSHVMLP0 VMRowToDelete = db.CSHVMLP0.Single(o => o.VMLID.Equals(VMViewModel.VMLNUM + v.PreviousMIMMIC));
                        db.CSHVMLP0.Remove(VMRowToDelete);
                        db.SaveChanges();
                    }
                    // Skip if MIMMIC is null
                    if (v.MIMMIC == null || v.MIMMIC.Equals(""))
                        continue;
                    CSHVMLP0 VMRow = new CSHVMLP0();
                    VMRow.VMLID = VMViewModel.VMLNUM + v.MIMMIC;
                    VMRow.VMLNUM = int.Parse(VMViewModel.VMLNUM);
                    VMRow.VMLNAM = VMViewModel.VMLNAM.ToString();
                    VMRow.VMLMIC = int.Parse(v.MIMMIC);
                    if (v.VMLQUA != null && !v.VMLQUA.Equals(""))
                        VMRow.VMLQUA = int.Parse(v.VMLQUA);
                    VMRow.VMLPRI = double.Parse(VMViewModel.VMLPRI);
                    VMRow.VMLPRO = double.Parse(VMViewModel.VMLPRO);
                    try
                    {
                        if (db.CSHVMLP0.Where(o => o.VMLID.Equals(VMRow.VMLID)).Any())
                        {
                            CSHVMLP0 rowToDelete = db.CSHVMLP0.Single(o => o.VMLID.Equals(VMRow.VMLID));
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
                // New
                if (!VMViewModel.MIMMIC[0].Equals(""))
                    for (int i = 0; i < VMViewModel.MIMMIC.Count(); i++)
                    {
                        CSHVMLP0 VMRow = new CSHVMLP0();
                        VMRow.VMLID = VMViewModel.VMLNUM + VMViewModel.MIMMIC[i];
                        VMRow.VMLNUM = int.Parse(VMViewModel.VMLNUM);
                        VMRow.VMLNAM = VMViewModel.VMLNAM.ToString();
                        VMRow.VMLMIC = int.Parse(VMViewModel.MIMMIC[i]);
                        if (VMViewModel.VMLQUA[i] != null && !VMViewModel.VMLQUA[i].Equals(""))
                            VMRow.VMLQUA = int.Parse(VMViewModel.VMLQUA[i]);
                        VMRow.VMLPRI = double.Parse(VMViewModel.VMLPRI);
                        VMRow.VMLPRO = double.Parse(VMViewModel.VMLPRO);
                        try
                        {
                            if (db.CSHVMLP0.Where(o => o.VMLID.Equals(VMRow.VMLID)).Any())
                            {
                                CSHVMLP0 rowToDelete = db.CSHVMLP0.Single(o => o.VMLID.Equals(VMRow.VMLID));
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
                return true;
            }
        }
    }
}