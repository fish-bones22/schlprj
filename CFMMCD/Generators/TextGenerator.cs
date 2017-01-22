using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFMMCD.Models.DB;
using System.Text;

namespace CFMMCD.Generators
{
    public class TextGenerator
    {
        public StringBuilder GenerateMenuItemMasterText(CSHMIMP0 MenuItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("02,");
            sb.Append(MenuItem.STATUS + ",");
            if (MenuItem.CSHMIMP0_NP6 == null)
                sb.Append(MenuItem.MIMMIC + ",");
            else
                sb.Append(MenuItem.CSHMIMP0_NP6.MIMMIC + ",");
            sb.Append(MenuItem.MIMSTA + ",");
            sb.Append(MenuItem.MIMFGC + ",");
            if (MenuItem.CSHMIMP0_NP6 == null)
                sb.Append(MenuItem.MIMNAM + ",");
            else
                sb.Append(MenuItem.CSHMIMP0_NP6.MIMNAM + ",");
            sb.Append(MenuItem.MIMDSC.Trim() + ",");
            sb.Append(MenuItem.MIMSSC.Trim() + ",");
            sb.Append(MenuItem.MIMDPC.Trim() + ",");
            sb.Append(MenuItem.MIMCIN.Trim() + ",");
            sb.Append(MenuItem.MIMDGC + ",");
            sb.Append(MenuItem.MIMASC.Trim() + ",");
            sb.Append(MenuItem.MIMTXC.Trim() + ",");
            sb.Append(MenuItem.MIMUTC + ",");
            sb.Append(MenuItem.MIMDWE.Trim() + ",");
            sb.Append(MenuItem.MIMTCI.Trim() + ",");
            if (MenuItem.MIMPRI != null)
                sb.Append(((double)MenuItem.MIMPRI).ToString("N2") + ",");
            else sb.Append("0.00,");
            sb.Append(MenuItem.MIMTCA.Trim() + ",");
            if (MenuItem.MIMPRO != null)
                sb.Append(((double)MenuItem.MIMPRO).ToString("N2") + ",");
            else sb.Append("0.00,");
            sb.Append(MenuItem.MIMTCG.Trim() + ",");
            if (MenuItem.MIMPRG != null)
                sb.Append(((double)MenuItem.MIMPRG).ToString("N2") + ",");
            else sb.Append("0.00,");
            if (MenuItem.MIMPND != null)
                sb.Append(((DateTime)MenuItem.MIMPND).ToString("yyyy-MM-dd") + ",");
            else sb.Append(",");
            sb.Append(MenuItem.MIMKBP + ",");
            sb.Append(MenuItem.MIMKSC.Trim() + ",");
            sb.Append(MenuItem.MIMSKT.Trim() + ",");
            sb.Append(MenuItem.MIMGRP.Trim() + ",");
            sb.Append(MenuItem.MIMWLV.Trim() + ",");
            sb.Append(MenuItem.MIMWSD.Trim() + ",");
            sb.Append(MenuItem.MIMWGR.Trim() + ",");
            sb.Append(MenuItem.MIMHPT.Trim() + ",");
            sb.Append(MenuItem.MIMUSR.Trim() + ",");
            if (MenuItem.MIMDAT != null)
                sb.Append(((DateTime)MenuItem.MIMDAT).ToString("yyyy-MM-dd") + ",");
            else sb.Append(",");
            if (MenuItem.MIMEDT != null)
                sb.Append(((DateTime)MenuItem.MIMEDT).ToString("yyyy-MM-dd") + ",");
            else sb.Append(",");
            sb.Append(MenuItem.MIMFLG + ",");
            sb.Append(MenuItem.MIMBIN.Trim() + ",");
            sb.Append(MenuItem.MIMBIT + ",");
            sb.Append(MenuItem.MIMBIR.Trim() + ",");
            sb.Append(MenuItem.MIMBGR.Trim() + ",");
            sb.Append(MenuItem.MIMBQU + ",");
            sb.Append(MenuItem.MIMGRA.Trim() + ",");
            sb.Append(MenuItem.MIMIST.Trim() + ",");
            sb.Append(MenuItem.MIMCLR.Trim() + ",");
            if (MenuItem.MIMNPI != null)
                sb.Append(((double)MenuItem.MIMNPI).ToString("0.00,") + ",");
            else sb.Append("0.00,");
            if (MenuItem.MIMNPO != null)
                sb.Append(((double)MenuItem.MIMNPO).ToString("0.00,") + ",");
            else sb.Append("0.00,");
            if (MenuItem.MIMNPD != null)
                sb.Append(((double)MenuItem.MIMNPD).ToString("0.00,") + ",");
            else sb.Append("0.00,");
            sb.Append(MenuItem.MIMSKI + ",");
            sb.Append(MenuItem.MIMBMI + ",");
            if (MenuItem.MIMNPA != null)
                sb.Append(((double)MenuItem.MIMNPA).ToString("0.00,") + ",");
            else sb.Append("0.00,");
            if (MenuItem.MIMNNP != null)
                sb.Append(((double)MenuItem.MIMNNP).ToString("0.00,") + ",");
            else sb.Append("0.00,");
            sb.Append(MenuItem.MIMNPT.Trim() + System.Environment.NewLine);

            return sb;
        }

        public StringBuilder GenerateRawItemMasterText(INVRIMP0 RawItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("03,");
            sb.Append(RawItem.STATUS.Trim() + ",");
            sb.Append(RawItem.RIMRIC + ",");
            sb.Append(RawItem.RIMVPC + ",");
            sb.Append(RawItem.RIMRID.Trim() + ",");
            sb.Append(RawItem.RIMRIG.Trim() + ",");
            sb.Append(RawItem.RIMTEM.Trim() + ",");
            sb.Append(RawItem.RIMPGR.Trim() + ",");
            sb.Append(RawItem.RIMPIS.Trim() + ",");
            sb.Append(RawItem.RIMBVP.Trim() + ",");
            sb.Append(RawItem.RIMBZP.Trim() + ",");
            sb.Append(RawItem.RIMUMC.Trim() + ",");
            if (RawItem.RIMUPC != null)
                sb.Append(((double)RawItem.RIMUPC).ToString("N2") + ",");
            else sb.Append("0.00,");
            if (RawItem.RIMSUQ != null)
                sb.Append(((double)RawItem.RIMSUQ).ToString("N2") + ",");
            else sb.Append("0.00,");
            sb.Append(RawItem.RIMLAY + ",");
            if (RawItem.RIMCPR != null)
                sb.Append(((double)RawItem.RIMCPR).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMCPN != null)
                sb.Append(((double)RawItem.RIMCPN).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMPDT != null)
                sb.Append(((DateTime)RawItem.RIMPDT).ToString("yyyy-MM-dd") + ",");
            else sb.Append("null,");
            sb.Append(RawItem.RIMPVN + ",");
            sb.Append(RawItem.RIMSVN + ",");
            sb.Append(RawItem.RIMCWC.Trim() + ",");
            sb.Append(RawItem.RIMPRO.Trim() + ",");
            sb.Append(RawItem.RIMSE4.Trim() + ",");
            sb.Append(RawItem.RIMERT.Trim() + ",");
            if (RawItem.RIMUSF != null)
                sb.Append(((double)RawItem.RIMUSF).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMSDP != null)
                sb.Append(((double)RawItem.RIMSDP).ToString("N2") + ",");
            else sb.Append("0.00,");
            if (RawItem.RIMUS1 != null)
                sb.Append(((double)RawItem.RIMUS1).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMUS2 != null)
                sb.Append(((double)RawItem.RIMUS2).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMUS3 != null)
                sb.Append(((double)RawItem.RIMUS3).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMUS4 != null)
                sb.Append(((double)RawItem.RIMUS4).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMUS5 != null)
                sb.Append(((double)RawItem.RIMUS5).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMUSX != null)
                sb.Append(((double)RawItem.RIMUSX).ToString("N4") + ",");
            else sb.Append("0.0000,");
            if (RawItem.RIMMSD != null)
                sb.Append(((double)RawItem.RIMMSD).ToString("N2") + ",");
            else sb.Append("0.00,");
            if (RawItem.RIMMSL != null)
                sb.Append(((double)RawItem.RIMMSL).ToString("N2") + ",");
            else sb.Append("0.00,");
            sb.Append(RawItem.RIMLA1.Trim() + ",");
            sb.Append(RawItem.RIMLA2.Trim() + ",");
            sb.Append(RawItem.RIMLP1 + ",");
            sb.Append(RawItem.RIMLP2 + ",");
            sb.Append(RawItem.RIMSTA.Trim() + ",");
            sb.Append(RawItem.RIMUSR.Trim() + ",");
            if (RawItem.RIMDAT != null)
                sb.Append(((DateTime)RawItem.RIMDAT).ToString("yyyy-MM-dd") + ",");
            else sb.Append("null,");
            if (RawItem.RIMEDT != null)
                sb.Append(((DateTime)RawItem.RIMEDT).ToString("yyyy-MM-dd") + ",");
            else sb.Append("null,");
            sb.Append(RawItem.RIMFLG + ",");
            sb.Append(RawItem.RIMORD.Trim() + ",");
            sb.Append(RawItem.RIMLIN + ",");
            sb.Append(RawItem.RIMADE.Trim() + ",");
            sb.Append(RawItem.RIMBAR.Trim() +  System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateRecipeText(INVRIRP0 Recipe)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("04,");
            sb.Append(Recipe.STATUS.Trim() + ",");
            sb.Append(Recipe.RIRRID.Trim() + ",");
            if (Recipe.RIRMIC != null)
                sb.Append(Recipe.RIRMIC.ToString() + ",");
            else sb.Append(",");
            if (Recipe.RIRRIC != null)
                sb.Append(Recipe.RIRRIC.ToString() + ",");
            else sb.Append(",");
            if (Recipe.RIRVPC != null)
                sb.Append(Recipe.RIRVPC.ToString() + ",");
            else sb.Append(",");
            if (Recipe.RIRSFQ != null)
                sb.Append(((double)Recipe.RIRSFQ).ToString("N2") + ",");
            else sb.Append("0.00,");
            if (Recipe.RIRCWC != null)
                sb.Append(Recipe.RIRCWC.Trim() + ",");
            else sb.Append(",");
            if (Recipe.RIRSTA != null)
                sb.Append(Recipe.RIRSTA.Trim() + ",");
            else sb.Append(",");
            if (Recipe.RIRVST != null)
                sb.Append(Recipe.RIRVST.Trim() + ",");
            else sb.Append(",");
            if (Recipe.RIRUSR != null)
                sb.Append(Recipe.RIRUSR.Trim() + ",");
            else sb.Append(",");
            if (Recipe.RIRDAT == null)
                sb.Append("null,");
            else
                sb.Append(Recipe.RIRDAT.ToString() + ",");
            if (Recipe.RIRFLG != null)
                sb.Append(Recipe.RIRFLG.ToString() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateValueMealText(CSHVMLP0 ValueMeal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("05,");
            sb.Append(ValueMeal.STATUS.Trim() + ",");
            sb.Append(ValueMeal.VMLNUM.ToString() + ",");
            if (ValueMeal.VMLNAM != null)
                sb.Append(ValueMeal.VMLMIC.ToString() + ",");
            else sb.Append(",");
            if (ValueMeal.VMLQUA != null)
                sb.Append(ValueMeal.VMLQUA.ToString() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateProductMixText(CSHPMGP0 ProductMixGroup)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("06,");
            sb.Append(ProductMixGroup.STATUS.Trim() + ",");
            if (ProductMixGroup.PMGGRP != null)
                sb.Append(ProductMixGroup.PMGGRP.Trim() + ",");
            else sb.Append(",");
            if (ProductMixGroup.PMGNUM != null)
                sb.Append(ProductMixGroup.PMGNUM.Trim() + ",");
            else sb.Append(",");
            if (ProductMixGroup.PMGTXT != null)
                sb.Append(ProductMixGroup.PMGTXT.Trim() + ",");
            else sb.Append(",");
            if (ProductMixGroup.PMGSTA != null)
                sb.Append(ProductMixGroup.PMGSTA.Trim() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateMaterialGroupText(INVMGRP0 MaterialGroup)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("07,");
            if (MaterialGroup.STATUS != null)
                sb.Append(MaterialGroup.STATUS.Trim() + ",");
            else sb.Append(",");
            if (MaterialGroup.MGRGRP != null)
                sb.Append(MaterialGroup.MGRGRP.Trim() + ",");
            else sb.Append(",");
            if (MaterialGroup.MGRTXT != null)
                sb.Append(MaterialGroup.MGRTXT.Trim() + ",");
            else sb.Append(",");
            if (MaterialGroup.MGRSTA != null)
                sb.Append(MaterialGroup.MGRSTA.Trim() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateInitOfMeasureText(INVUOMP0 UnitOfMeasure)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("08,");
            sb.Append(UnitOfMeasure.STATUS.Trim() + ",");
            if (UnitOfMeasure.UOMDES != null)
                sb.Append(UnitOfMeasure.UOMDES.Trim() + ",");
            else sb.Append(",");
            if (UnitOfMeasure.UOMDEL != null)
                sb.Append(UnitOfMeasure.UOMDEL.Trim() + ",");
            else sb.Append(",");
            if (UnitOfMeasure.UOMDAT == null)
                sb.Append("null,");
            else
                sb.Append(((DateTime)UnitOfMeasure.UOMDAT).ToString("yyyy-MM-dd") + ",");
            if (UnitOfMeasure.UOMUSR != null)
                sb.Append(UnitOfMeasure.UOMUSR.Trim() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }

        public StringBuilder GenerateVendorText(INVVEMP0 Vendor)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("09,");
            sb.Append(Vendor.VEMVEN.ToString() + ",");
            sb.Append(Vendor.VEMWSI.ToString() + ",");
            sb.Append(Vendor.VEMDS1.Trim() + ",");
            if (Vendor.VEMDS2 != null)
                sb.Append(Vendor.VEMDS2.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMCCD != null)
                sb.Append(Vendor.VEMCCD.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMZIP != null)
                sb.Append(Vendor.VEMZIP.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMCTY != null)
                sb.Append(Vendor.VEMCTY.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMSTR != null)
                sb.Append(Vendor.VEMSTR.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMTEL != null)
                sb.Append(Vendor.VEMTEL.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMSTN != null)
                sb.Append(Vendor.VEMSTN.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMLOC != null)
                sb.Append(Vendor.VEMLOC.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMDAY != null)
                sb.Append(Vendor.VEMDAY.ToString() + ",");
            else sb.Append(",");
            if (Vendor.VEMTID != null)
                sb.Append(Vendor.VEMTID.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMSTA != null)
                sb.Append(Vendor.VEMSTA.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMDAT == null)
                sb.Append("null,");
            else
                sb.Append(((DateTime)Vendor.VEMDAT).ToString("yyyy-MM-dd") + ",");
            if (Vendor.VEMUSR != null)
                sb.Append(Vendor.VEMUSR.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMADE != null)
                sb.Append(Vendor.VEMADE.Trim() + ",");
            else sb.Append(",");
            if (Vendor.VEMDEL != null)
                sb.Append(Vendor.VEMDEL.Trim() +  System.Environment.NewLine);
            else sb.Append( System.Environment.NewLine);
            return sb;
        }
    }
}