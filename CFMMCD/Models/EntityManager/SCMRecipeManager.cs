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
        public static SCMRecipeViewModel SearchSCMRecipe(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                SCMRecipeViewModel CSMList = new SCMRecipeViewModel();
                if (SearchItem == null || SearchItem.Equals(""))
                    return CSMList;

                CSMList.CSMDES = SearchItem;
                CSMList.RawItemList = SearchSCMRawItem(SearchItem);
                return CSMList;
            }
        }

        public static List<SCMRawItem> SearchSCMRawItem(string SCMDescription)
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
                    if (db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).Any())
                        mi.RIMCPR = db.RIM_VEM_Lookup.FirstOrDefault(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).RIMCPR.ToString();
                    // Get Primary vendor of RI
                    int? primVendor = db.INVRIMP0.FirstOrDefault(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).RIMPVN;
                    if (primVendor != null) 
                        if (db.RIM_VEM_Lookup.Where(o => (o.RIMRIC.ToString().Equals(mi.RIMRIC) && o.VEMVEN == primVendor)).Any())
                            mi.RIMCPR = db.RIM_VEM_Lookup.FirstOrDefault(o => ( o.RIMRIC.ToString().Equals(mi.RIMRIC) && o.VEMVEN == primVendor )).RIMCPR.ToString();
                    mi.CSMCWC = v.CSMCWC;
                    mi.StoAtt = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(mi.RIMRIC)).Store_Attrib;
                    mi.CSMID = v.CSMID;
                    MIList.Add(mi);
                }
                return MIList;
            }
        }

        public static bool UpdateSCMRecipe(SCMRecipeViewModel CSMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (CSMViewModel.CSMDES == null || CSMViewModel.CSMDES.Equals(""))
                    return false;
                // Existing
                foreach (var v in CSMViewModel.RawItemList)
                {
                    // Delete overwritten RIMRIC entry
                    if (!v.PreviousRIMRIC.Equals(v.RIMRIC) && db.CSM_Master_Recipe.Where(o => o.CSMDES.ToString().Equals(CSMViewModel.CSMDES)).Any())
                    {
                        SCM_Master_Recipe CSMRowToDelete = db.SCM_Master_Recipe.Single(o => o.CSMID.Equals(v.CSMID));
                        db.SCM_Master_Recipe.Remove(CSMRowToDelete);
                        db.SaveChanges();
                    }
                    SCM_Master_Recipe CSMRow = new SCM_Master_Recipe();
                    CSMRow.CSMID =  v.CSMID;
                    CSMRow.CSMDES = CSMViewModel.CSMDES;
                    CSMRow.RIMRIC = int.Parse(v.RIMRIC);
                    if (v.CSMSFQ != null)
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
                if (!CSMViewModel.RIMRIC[0].Equals("") && !CSMViewModel.CSMDES.Equals(""))
                    for (int i = 0; i < CSMViewModel.RIMRIC.Count(); i++)
                    {
                        string rimric = CSMViewModel.RIMRIC[i];
                        if (db.SCM_Master_Recipe.Where(o => (o.RIMRIC.ToString().Equals(rimric) && o.CSMDES.Equals(CSMViewModel.CSMDES))).Any())
                            continue;
                        SCM_Master_Recipe CSMRow = new SCM_Master_Recipe();
                        CSMRow.CSMID = (new Random().Next(1, 999)).ToString() + CSMViewModel.RIMRIC[i];
                        CSMRow.CSMDES = CSMViewModel.CSMDES;
                        CSMRow.RIMRIC = int.Parse(CSMViewModel.RIMRIC[i]);
                        if (CSMViewModel.CSMSFQ[i] != null && !CSMViewModel.CSMSFQ[i].Equals(""))
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