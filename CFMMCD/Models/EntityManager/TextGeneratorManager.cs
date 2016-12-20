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
        public bool GeneratePackets(TextGeneratorViewModel TGViewModel)
        {
            StringBuilder sb = new StringBuilder();
            if (TGViewModel.IncludeAll || TGViewModel.IncludeMIM)
            {
                using (CFMMCDEntities db = new CFMMCDEntities())
                {
                    for (int i = 0; i < db.CSHMIMP0.Count(); i++)
                    {
                        string STATUS = db.CSHMIMP0.ElementAt(i).STATUS;
                        if (STATUS.Equals("A") || STATUS.Equals("B"))
                        {
                            sb.Append("02,");
                            sb.Append(STATUS+",");
                            sb.Append(db.CSHMIMP0.ElementAt(i).MIMMIC + ",");
                            sb.Append(db.CSHMIMP0.ElementAt(i).MIMSTA + ",");
                            sb.Append(db.CSHMIMP0.ElementAt(i).MIMFGC + ",");
                            sb.Append(db.CSHMIMP0.ElementAt(i).MIMMIC + ",");
                        }
                    }
                }
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
            return true;
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