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
        public void SearchMIM(MenuItemMasterViewModel MIMViewModel)
        {

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
                MIMRow.MIMHPT = MIMViewModel.MIMHPT;
                MIMRow.MIMEDT = Convert.ToDateTime(MIMViewModel.MIMEDT);
                MIMRow.MIMNPI = double.Parse(MIMViewModel.MIMNPI);
                MIMRow.MIMNPO = double.Parse(MIMViewModel.MIMNPO);
                MIMRow.MIMNPD = double.Parse(MIMViewModel.MIMNPD);
                MIMRow.MIMNPA = double.Parse(MIMViewModel.MIMNPA);
                MIMRow.MIMNNP = double.Parse(MIMViewModel.MIMNNP);
                MIMRow.MIMNPT = MIMViewModel.MIMNPT;

                MIMRow.MIMSSC = "08";
                MIMRow.MIMCIN = "";
                MIMRow.MIMDGC = 0;
                MIMRow.MIMASC = "00";
                MIMRow.MIMTXC = "";
                MIMRow.MIMUTC = 0;
                MIMRow.MIMDWE = "0";
                MIMRow.MIMKBP = 0;
                MIMRow.MIMKSC = "";
                MIMRow.MIMSKT = "00";
                MIMRow.MIMGRP = "00";
                MIMRow.MIMWLV = "0";
                MIMRow.MIMWSD = "0";
                MIMRow.MIMUSR = user.Substring(0, 3).ToUpper();
                MIMRow.MIMDAT = Convert.ToDateTime(DateTime.Now.ToString("YYYY/MM/DD"));
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