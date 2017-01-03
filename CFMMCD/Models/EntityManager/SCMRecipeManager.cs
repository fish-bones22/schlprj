using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class SCMRecipeManager
    {
        public List<SCMRecipeViewModel> SearchSCMRecipe(SCMRecipeViewModel CSMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<SCMRecipeViewModel> CSMList = new List<SCMRecipeViewModel>();
                List<SCM_Master_Recipe> MICSMowList;
                if (CSMViewModel.SearchItem == null || CSMViewModel.SearchItem.Equals(""))
                    return null;
                if (db.SCM_Master_Recipe.Where(o => o.CSMDES.Contains(CSMViewModel.SearchItem)).Any())
                {
                    MICSMowList = db.SCM_Master_Recipe.Where(o => o.CSMDES.Contains(CSMViewModel.SearchItem)).ToList();
                }
                else
                    return null;
                foreach (SCM_Master_Recipe scm in MICSMowList)
                {
                    SCMRecipeViewModel vm = new SCMRecipeViewModel();
                    vm.CSMDES = scm.CSMDES;
                    vm.RawItemList = SearchSCMRawItem(vm.CSMDES);
                    CSMList.Add(vm);
                }
                if (CSMList == null || CSMList.Count == 0)
                    return null;
                return CSMList;
            }
        }

        public List<SCMRawItem> SearchSCMRawItem(string SCMDescription)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<SCM_Master_Recipe> CSMRowList = new List<SCM_Master_Recipe>();
                List<SCMRawItem> MIList = new List<SCMRawItem>();
                if (db.SCM_Master_Recipe.Where(o => o.CSMDES.ToString().Equals(SCMDescription)).Any())
                {
                    CSMRowList = db.SCM_Master_Recipe.Where(o => o.CSMDES.ToString().Equals(SCMDescription)).ToList();
                }
                foreach (var v in CSMRowList)
                {
                    SCMRawItem mi = new SCMRawItem();
                    mi.RIMRIC = v.RIMRIC.ToString();
                    mi.RIMRID = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).RIMRID;
                    mi.CSMSFQ = v.CSMSFQ.ToString();
                    mi.RIMCPR = db.RIM_VEM_Lookup.Single(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).RIMCPR.ToString();
                    mi.CSMCWC = v.CSMCWC;
                    mi.StoAtt = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).Store_Attrib;
                    mi.CSMID = v.CSMID;
                    MIList.Add(mi);
                }
                return MIList;
            }
        }

        public bool UpdateSCMRecipe(SCMRecipeViewModel CSMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (CSMViewModel.CSMDES == null || CSMViewModel.CSMDES.Equals(""))
                    return false;
                // Existing
                foreach (var v in CSMViewModel.RawItemList)
                {
                    SCM_Master_Recipe CSMRow = new SCM_Master_Recipe();
                    CSMRow.CSMID =  v.CSMID;
                    CSMRow.CSMDES = CSMViewModel.CSMDES;
                    CSMRow.RIMRIC = int.Parse(v.RIMRIC);
                    CSMRow.CSMSFQ = double.Parse(v.CSMSFQ);
                    CSMRow.CSMCWC = v.CSMCWC;
                    try
                    {
                        if (db.SCM_Master_Recipe.Where(o => o.CSMID.Equals(CSMRow.CSMID)).Any())
                        {
                            SCM_Master_Recipe rowToDelete = db.SCM_Master_Recipe.Single(o => o.CSMID.Equals(CSMRow.CSMID));
                            db.SCM_Master_Recipe.Remove(rowToDelete);
                            db.SCM_Master_Recipe.Add(CSMRow);
                        }
                        else
                        {
                            db.SCM_Master_Recipe.Add(CSMRow);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Source);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        System.Diagnostics.Debug.WriteLine(e.Data);
                        foreach (var eve in ((DbEntityValidationException)e).EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                    ve.PropertyName,
                                    eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                    ve.ErrorMessage);
                            }
                        }
                        return false;
                    }
                }
                // New
                if (!CSMViewModel.RIMRIC[0].Equals("") && !CSMViewModel.CSMSFQ[0].Equals("") && !CSMViewModel.CSMDES.Equals(""))
                    for (int i = 0; i < CSMViewModel.RIMRIC.Count(); i++)
                    {
                        SCM_Master_Recipe CSMRow = new SCM_Master_Recipe();
                        CSMRow.CSMID = (new Random().Next(999)).ToString() + CSMViewModel.RIMRIC[i];
                        CSMRow.CSMDES = CSMViewModel.CSMDES;
                        CSMRow.RIMRIC = int.Parse(CSMViewModel.RIMRIC[i]);
                        CSMRow.CSMSFQ = double.Parse(CSMViewModel.CSMSFQ[i]);
                        CSMRow.CSMCWC = CSMViewModel.CSMCWC[i];
                        try
                        {
                            if (db.SCM_Master_Recipe.Where(o => o.CSMID.Equals(CSMRow.CSMID)).Any())
                            {
                                SCM_Master_Recipe rowToDelete = db.SCM_Master_Recipe.Single(o => o.CSMID.Equals(CSMRow.CSMID));
                                db.SCM_Master_Recipe.Remove(rowToDelete);
                                db.SCM_Master_Recipe.Add(CSMRow);
                            }
                            else
                            {
                                db.SCM_Master_Recipe.Add(CSMRow);
                            }
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Source);
                            System.Diagnostics.Debug.WriteLine(e.Message);
                            System.Diagnostics.Debug.WriteLine(e.StackTrace);
                            System.Diagnostics.Debug.WriteLine(e.Data);
                            foreach (var eve in ((DbEntityValidationException)e).EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                        ve.PropertyName,
                                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                        ve.ErrorMessage);
                                }
                            }
                            return false;
                        }
                    }
                return true;
            }
        }
    }
}