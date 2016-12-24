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
                for (int i = 0; i < 5-StoreNo.Length+2; i++)
                    StoreNo = "0" + StoreNo;
                sb.Append("01," + StoreNo + "," + StoreName + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "," + TGViewModel.PromoTitle + "\n");
                if (TGViewModel.IncludeAll || TGViewModel.IncludeMIM)
                {
                    foreach (CSHMIMP0 c in db.CSHMIMP0)
                    {
                        string STATUS = c.STATUS;
                        DateTime date = (DateTime) c.MIMEDT;
                        DateTime dateFrom = DateTime.Parse(TGViewModel.DateFrom);
                        DateTime dateTo = DateTime.Parse(TGViewModel.DateTo);

                        if ((STATUS.Equals("A") || STATUS.Equals("E")) &&
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
                            sb.Append(c.MIMDSC.Trim() + ",");
                            sb.Append(c.MIMSSC.Trim() + ",");
                            sb.Append(c.MIMDPC.Trim() + ",");
                            sb.Append(c.MIMCIN.Trim() + ",");
                            sb.Append(c.MIMDGC + ",");
                            sb.Append(c.MIMASC.Trim() + ",");
                            sb.Append(c.MIMTXC.Trim() + ",");
                            sb.Append(c.MIMUTC + ",");
                            sb.Append(c.MIMDWE.Trim() + ",");
                            sb.Append(c.MIMTCI.Trim() + ",");
                            sb.Append(c.MIMPRI + ",");
                            sb.Append(c.MIMTCA.Trim() + ",");
                            sb.Append(c.MIMPRO + ",");
                            sb.Append(c.MIMTCG.Trim() + ",");
                            sb.Append(c.MIMPRG + ",");
                            sb.Append(((DateTime)c.MIMPND).ToString("yyyy-MM-dd") + ",");
                            sb.Append(c.MIMKBP + ",");
                            sb.Append(c.MIMKSC.Trim() + ",");
                            sb.Append(c.MIMSKT.Trim() + ",");
                            sb.Append(c.MIMGRP.Trim() + ",");
                            sb.Append(c.MIMWLV.Trim() + ",");
                            sb.Append(c.MIMWSD.Trim() + ",");
                            sb.Append(c.MIMWGR.Trim() + ",");
                            sb.Append(c.MIMHPT.Trim() + ",");
                            sb.Append(c.MIMUSR.Trim() + ",");
                            sb.Append(((DateTime)c.MIMDAT).ToString("yyyy-MM-dd") + ",");
                            sb.Append(((DateTime)c.MIMEDT).ToString("yyyy-MM-dd") + ",");
                            sb.Append(c.MIMFLG + ",");
                            sb.Append(c.MIMBIN.Trim() + ",");
                            sb.Append(c.MIMBIT + ",");
                            sb.Append(c.MIMBIR.Trim() + ",");
                            sb.Append(c.MIMBGR.Trim() + ",");
                            sb.Append(c.MIMBQU + ",");
                            sb.Append(c.MIMGRA.Trim() + ",");
                            sb.Append(c.MIMIST.Trim() + ",");
                            sb.Append(c.MIMCLR.Trim() + ",");
                            sb.Append(c.MIMNPI + ",");
                            sb.Append(c.MIMNPO + ",");
                            sb.Append(c.MIMNPD + ",");
                            sb.Append(c.MIMSKI + ",");
                            sb.Append(c.MIMBMI + ",");
                            sb.Append(c.MIMNPA + ",");
                            sb.Append(c.MIMNNP + ",");
                            sb.Append(c.MIMNPT.Trim() + "\n");
                        } // end if
                    } // end for
                }
                if (TGViewModel.IncludeAll || TGViewModel.IncludeRIM)
                {
                    foreach (INVRIMP0 c in db.INVRIMP0)
                    {
                        string STATUS = c.STATUS;
                        DateTime date = (DateTime)c.RIMEDT;
                        DateTime dateFrom = DateTime.Parse(TGViewModel.DateFrom);
                        DateTime dateTo = DateTime.Parse(TGViewModel.DateTo);

                        if ((STATUS.Equals("A") || STATUS.Equals("E")) &&
                            ((date.CompareTo(dateFrom) > 0) && (date.CompareTo(dateTo) <= 0)))
                        {
                             sb.Append("03,");
                             sb.Append(STATUS.Trim() + ",");
                             sb.Append(c.RIMRIC + ",");
                             sb.Append(c.RIMVPC + ",");
                             sb.Append(c.RIMRID.Trim() + ",");
                             sb.Append(c.RIMRIG.Trim() + ",");
                             sb.Append(c.RIMTEM.Trim() + ",");
                             sb.Append(c.RIMPGR.Trim() + ",");
                             sb.Append(c.RIMPIS.Trim() + ",");
                             sb.Append(c.RIMBVP.Trim() + ",");
                             sb.Append(c.RIMBZP.Trim() + ",");
                             sb.Append(c.RIMUMC.Trim() + ",");
                             sb.Append(c.RIMUPC + ",");
                             sb.Append(c.RIMSUQ + ",");
                             sb.Append(c.RIMLAY + ",");
                             sb.Append(c.RIMCPR + ",");
                             sb.Append(c.RIMCPN + ",");
                            sb.Append(((DateTime)c.RIMPDT).ToString("yyyy-MM-dd") + ",");
                            sb.Append(c.RIMPVN + ",");
                             sb.Append(c.RIMSVN + ",");
                             sb.Append(c.RIMCWC.Trim() + ",");
                             sb.Append(c.RIMPRO.Trim() + ",");
                             sb.Append(c.RIMSE4.Trim() + ",");
                             sb.Append(c.RIMERT.Trim() + ",");
                             sb.Append(c.RIMUSF + ",");
                             sb.Append(c.RIMSDP + ",");
                             sb.Append(c.RIMUS1 + ",");
                             sb.Append(c.RIMUS2 + ",");
                             sb.Append(c.RIMUS3 + ",");
                             sb.Append(c.RIMUS4 + ",");
                             sb.Append(c.RIMUS5 + ",");
                             sb.Append(c.RIMUSX + ",");
                             sb.Append(c.RIMMSD + ",");
                             sb.Append(c.RIMMSL + ",");
                             sb.Append(c.RIMLA1.Trim() + ",");
                             sb.Append(c.RIMLA2.Trim() + ",");
                             sb.Append(c.RIMLP1 + ",");
                             sb.Append(c.RIMLP2 + ",");
                             sb.Append(c.RIMSTA.Trim() + ",");
                             sb.Append(c.RIMUSR.Trim() + ",");
                             sb.Append(((DateTime)c.RIMDAT).ToString("yyyy-MM-dd") + ",");
                             sb.Append(((DateTime)c.RIMEDT).ToString("yyyy-MM-dd") + ",");
                             sb.Append(c.RIMFLG + ",");
                             sb.Append(c.RIMORD.Trim() + ",");
                             sb.Append(c.RIMLIN + ",");
                             sb.Append(c.RIMADE.Trim() + ",");
                             sb.Append(c.RIMBAR.Trim() + "\n");
                        } // end if
                    } // end for
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
                    spViewModel.STORE_NO = db.Store_Profile.ElementAt(i).STORE_NO.ToString();
                    spViewModel.STORE_NAME = db.Store_Profile.ElementAt(i).STORE_NAME;
                    spViewModel.LOCATION = db.Store_Profile.ElementAt(i).LOCATION;
                    spList.Add(spViewModel);
                }
                return spList;
            }
        }
    }
}