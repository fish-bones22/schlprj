using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class MenuItemMasterManager
    {
        public MenuItemMasterViewModel SearchMIM(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem)).Any())
                {
                    MIMRow = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem));
                }
                else if (db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Equals(MIMViewModel.SearchItem)).Any())
                {
                    var menuItem = db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Equals(MIMViewModel.SearchItem));
                    MIMRow = menuItem.FirstOrDefault();
                }
                else
                    return new MenuItemMasterViewModel();

                MIMViewModel.MIMMIC = MIMRow.MIMMIC.ToString();
                MIMViewModel.MIMSTA = MIMRow.MIMSTA;
                MIMViewModel.MIMFGC = MIMRow.MIMFGC;
                MIMViewModel.MIMNAM = MIMRow.MIMNAM;
                MIMViewModel.MIMDSC = MIMRow.MIMDSC;
                MIMViewModel.MIMDPC = MIMRow.MIMDPC;
                MIMViewModel.MIMTCI = MIMRow.MIMTCI;
                MIMViewModel.MIMPRI = MIMRow.MIMPRI.ToString();
                MIMViewModel.MIMTCA = MIMRow.MIMTCA;
                MIMViewModel.MIMPRO = MIMRow.MIMPRO.ToString();
                MIMViewModel.MIMTCG = MIMRow.MIMTCG;
                MIMViewModel.MIMPRG = MIMRow.MIMPRG.ToString();
                MIMViewModel.MIMPND = Convert.ToDateTime(MIMRow.MIMPND).ToString("yyyy-MM-dd");
                MIMViewModel.MIMWGR = MIMRow.MIMWGR;
                MIMViewModel.MIMHPT = MIMRow.MIMHPT;
                MIMViewModel.MIMEDT = Convert.ToDateTime(MIMRow.MIMEDT).ToString("yyyy-MM-dd");
                MIMViewModel.MIMNPI = MIMRow.MIMNPI.ToString();
                MIMViewModel.MIMNPO = MIMRow.MIMNPO.ToString();
                MIMViewModel.MIMNPD = MIMRow.MIMNPD.ToString();
                MIMViewModel.MIMNPA = MIMRow.MIMNPA.ToString();
                MIMViewModel.MIMNNP = MIMRow.MIMNNP.ToString();
                MIMViewModel.MIMNPT = MIMRow.MIMNPT;
                MIMViewModel.MIMLON = MIMRow.MIMLON;
                MIMViewModel.MIMUTC = MIMRow.MIMUTC.ToString();

                if (MIMRow.MIMMIC_NP6 != null || MIMRow.MIMMIC_NP6.HasValue)
                {
                    MIMViewModel.MIMMIC_NP6 = MIMRow.CSHMIMP0_NP6.MIMMIC.ToString();
                    MIMViewModel.MIMNAM_NP6 = MIMRow.CSHMIMP0_NP6.MIMNAM;
                    MIMViewModel.MIMLON_NP6 = MIMRow.CSHMIMP0_NP6.MIMLON;
                }

                return MIMViewModel;

            }
        }
        public void SaveMIM(MenuItemMasterViewModel MIMViewModel, string user)
        {
            using ( CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();

                MIMRow.MIMMIC = int.Parse(MIMViewModel.MIMMIC);
                MIMRow.MIMSTA = MIMViewModel.MIMSTA;
                MIMRow.MIMFGC = MIMViewModel.MIMFGC;
                MIMRow.MIMNAM = MIMViewModel.MIMNAM;
                MIMRow.MIMDSC = MIMViewModel.MIMDSC;
                MIMRow.MIMDPC = MIMViewModel.MIMDPC;
                MIMRow.MIMTCI = MIMViewModel.MIMTCI;
                MIMRow.MIMPRI = double.Parse(MIMViewModel.MIMPRI);
                MIMRow.MIMTCA = MIMViewModel.MIMTCA;
                MIMRow.MIMPRO = double.Parse(MIMViewModel.MIMPRO);
                MIMRow.MIMTCG = MIMViewModel.MIMTCG;
                MIMRow.MIMPRG = double.Parse(MIMViewModel.MIMPRG);
                MIMRow.MIMPND = Convert.ToDateTime(MIMViewModel.MIMPND);
                MIMRow.MIMWGR = MIMViewModel.MIMWGR;
                MIMRow.MIMUTC = int.Parse(MIMViewModel.MIMUTC);
                MIMRow.MIMHPT = MIMViewModel.MIMHPT;
                MIMRow.MIMEDT = Convert.ToDateTime(MIMViewModel.MIMEDT);
                MIMRow.MIMNPI = double.Parse(MIMViewModel.MIMNPI);
                MIMRow.MIMNPO = double.Parse(MIMViewModel.MIMNPO);
                MIMRow.MIMNPD = double.Parse(MIMViewModel.MIMNPD);
                MIMRow.MIMNPA = double.Parse(MIMViewModel.MIMNPA);
                MIMRow.MIMNNP = double.Parse(MIMViewModel.MIMNNP);
                MIMRow.MIMNPT = MIMViewModel.MIMNPT;

                MIMRow.MIMLON = MIMViewModel.MIMLON;

                MIMRow.MIMSSC = "08";
                MIMRow.MIMCIN = "";
                MIMRow.MIMDGC = 0;
                MIMRow.MIMASC = "00";
                MIMRow.MIMTXC = "";
                MIMRow.MIMDWE = "0";
                MIMRow.MIMKBP = 0;
                MIMRow.MIMKSC = "";
                MIMRow.MIMSKT = "00";
                MIMRow.MIMGRP = "00";
                MIMRow.MIMWLV = "0";
                MIMRow.MIMWSD = "0";
                MIMRow.MIMUSR = user.Substring(0, 3).ToUpper();
                MIMRow.MIMDAT = DateTime.Now;
                MIMRow.MIMFLG = false;
                MIMRow.MIMBIN = "";
                MIMRow.MIMBIT = 0;
                MIMRow.MIMBIR = "";
                MIMRow.MIMBGR = "";
                MIMRow.MIMBQU = 0;
                MIMRow.MIMGRA = "";
                MIMRow.MIMIST = "";
                MIMRow.MIMCLR = "";
                MIMRow.MIMSKI = 0;
                MIMRow.MIMBMI = 0;


                if (MIMViewModel.MIMMIC_NP6 != null)
                {
                    MIMRow.CSHMIMP0_NP6.MIMMIC = int.Parse(MIMViewModel.MIMMIC_NP6);
                    MIMRow.CSHMIMP0_NP6.MIMNAM = MIMViewModel.MIMNAM_NP6;
                    MIMRow.CSHMIMP0_NP6.MIMLON = MIMViewModel.MIMLON_NP6;
                }
                
                // If MIMMIC is already existing, update it instead of adding a new row
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                {
                    var rowToUpdate = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));
                    db.CSHMIMP0.Remove(rowToUpdate);
                    db.CSHMIMP0.Add(MIMRow);
                }
                else
                    db.CSHMIMP0.Add(MIMRow);
                db.SaveChanges();
            }
        }

        private void s()
        {
            throw new NotImplementedException();
        }

        public void DeleteMIM(MenuItemMasterViewModel MIMViewModel)
        {

        }
    }
}