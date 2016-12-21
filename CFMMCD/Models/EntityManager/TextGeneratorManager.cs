using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class TextGeneratorManager
    {
        /*
         * Generates text from database
         * 
         * Currently, yung unang Row lang po sa store ang nagagawan ng text kasi hindi pa po  namin alam
         * kung ano ang mechanics / functionality nung table sa Text gemeration page.
         */
        public bool GeneratePackets(TextGeneratorViewModel TGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                StringBuilder sb = new StringBuilder();
                string StoreName = db.Store_Profile.First().STORE_NAME;
                string StoreNo = db.Store_Profile.First().STORE_NO.ToString();
                // Pad zeroes to left side of StoreNo
                for (int i = 0; i < 5-StoreNo.Length+1; i++)
                    StoreNo = "0" + StoreNo;
                sb.Append("01," + StoreNo + "," + StoreName + "," + DateTime.Now.ToString() + "," + TGViewModel.PromoTitle + "\n");
                if (TGViewModel.IncludeAll || TGViewModel.IncludeMIM)
                {
                    foreach (CSHMIMP0 c in db.CSHMIMP0)
                    {
                        string STATUS = c.STATUS;
                        DateTime date = (DateTime) c.MIMEDT;
                        DateTime dateFrom = DateTime.Parse(TGViewModel.DateFrom);
                        DateTime dateTo = DateTime.Parse(TGViewModel.DateTo);

                        if ((STATUS.Equals("A") || STATUS.Equals("B")) &&
                            ((date.CompareTo(dateFrom) > 0) && (date.CompareTo(dateTo) <= 0)))
                        {
                            sb.Append("02,");
                            sb.Append(STATUS+",");
                            if (c.CSHMIMP0_NP6 == null)
                                sb.Append(c.MIMMIC + ",");
                            else
                                sb.Append(c.CSHMIMP0_NP6.MIMMIC + ",");
                            sb.Append(c.MIMSTA + ",");
                            sb.Append(c.MIMFGC + ",");
                            if (c.CSHMIMP0_NP6 == null)
                                sb.Append(c.MIMNAM + ",");
                            else
                                sb.Append(c.CSHMIMP0_NP6.MIMNAM + ",");
                            sb.Append(c.MIMSSC + ",");
                            sb.Append(c.MIMDPC + ",");
                            sb.Append(c.MIMCIN + ",");
                            sb.Append(c.MIMDGC + ",");
                            sb.Append(c.MIMASC + ",");
                            sb.Append(c.MIMTXC + ",");
                            sb.Append(c.MIMUTC + ",");
                            sb.Append(c.MIMDWE + ",");
                            sb.Append(c.MIMTCI + ",");
                            sb.Append(c.MIMPRI + ",");
                            sb.Append(c.MIMTCA + ",");
                            sb.Append(c.MIMPRO + ",");
                            sb.Append(c.MIMTCG + ",");
                            sb.Append(c.MIMPRG + ",");
                            sb.Append(c.MIMPND + ",");
                            sb.Append(c.MIMKBP + ",");
                            sb.Append(c.MIMKSC + ",");
                            sb.Append(c.MIMSKT + ",");
                            sb.Append(c.MIMGRP + ",");
                            sb.Append(c.MIMWLV + ",");
                            sb.Append(c.MIMWSD + ",");
                            sb.Append(c.MIMWGR + ",");
                            sb.Append(c.MIMHPT + ",");
                            sb.Append(c.MIMUSR + ",");
                            sb.Append(c.MIMDAT + ",");
                            sb.Append(c.MIMEDT + ",");
                            sb.Append(c.MIMFLG + ",");
                            sb.Append(c.MIMBIN + ",");
                            sb.Append(c.MIMBIT + ",");
                            sb.Append(c.MIMBIR + ",");
                            sb.Append(c.MIMBGR + ",");
                            sb.Append(c.MIMBQU + ",");
                            sb.Append(c.MIMGRA + ",");
                            sb.Append(c.MIMIST + ",");
                            sb.Append(c.MIMCLR + ",");
                            sb.Append(c.MIMNPI + ",");
                            sb.Append(c.MIMNPO + ",");
                            sb.Append(c.MIMNPD + ",");
                            sb.Append(c.MIMSKI + ",");
                            sb.Append(c.MIMBMI + ",");
                            sb.Append(c.MIMNPA + ",");
                            sb.Append(c.MIMNNP + ",");
                            sb.Append(c.MIMNPT + "\n");
                        } // end if
                    } // end for
                }
                if (TGViewModel.IncludeAll || TGViewModel.IncludeRIM)
                {

                }
                if (TGViewModel.IncludeAll || TGViewModel.IncludeREC)
                {

                }
                if (TGViewModel.IncludeAll)
                {

                }
                string filename = "CFM" + StoreNo + "_" + DateTime.Now.ToString("MMddyyyy_HHmm");
                if (TGViewModel.IncludeAll)
                    filename += ".ALL";
                else
                    filename += ".PRO";
                System.IO.File.WriteAllText(@"C:/SMS/SMSWEBCFM/"+filename, sb.ToString());
                return true;
            }
        }

        public List<StoreProfileViewModel> GetStoreList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<StoreProfileViewModel> spList = new List<StoreProfileViewModel>();
                for (int i = 0; i < db.Store_Profile.Count(); i++)
                {
                    StoreProfileViewModel spViewModel = new StoreProfileViewModel();
                    spViewModel.STORE_NO = db.Store_Profile.ElementAt(i).STORE_NO;
                    spViewModel.STORE_NAME = db.Store_Profile.ElementAt(i).STORE_NAME;
                    spViewModel.LOCATION = db.Store_Profile.ElementAt(i).LOCATION;
                    spList.Add(spViewModel);
                }
                return spList;
            }
        }
    }
}