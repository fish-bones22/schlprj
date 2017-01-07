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
            sb.Append(MenuItem.MIMPRI + ",");
            sb.Append(MenuItem.MIMTCA.Trim() + ",");
            sb.Append(MenuItem.MIMPRO + ",");
            sb.Append(MenuItem.MIMTCG.Trim() + ",");
            sb.Append(MenuItem.MIMPRG + ",");
            sb.Append(((DateTime)MenuItem.MIMPND).ToString("yyyy-MM-dd") + ",");
            sb.Append(MenuItem.MIMKBP + ",");
            sb.Append(MenuItem.MIMKSC.Trim() + ",");
            sb.Append(MenuItem.MIMSKT.Trim() + ",");
            sb.Append(MenuItem.MIMGRP.Trim() + ",");
            sb.Append(MenuItem.MIMWLV.Trim() + ",");
            sb.Append(MenuItem.MIMWSD.Trim() + ",");
            sb.Append(MenuItem.MIMWGR.Trim() + ",");
            sb.Append(MenuItem.MIMHPT.Trim() + ",");
            sb.Append(MenuItem.MIMUSR.Trim() + ",");
            sb.Append(((DateTime)MenuItem.MIMDAT).ToString("yyyy-MM-dd") + ",");
            sb.Append(((DateTime)MenuItem.MIMEDT).ToString("yyyy-MM-dd") + ",");
            sb.Append(MenuItem.MIMFLG + ",");
            sb.Append(MenuItem.MIMBIN.Trim() + ",");
            sb.Append(MenuItem.MIMBIT + ",");
            sb.Append(MenuItem.MIMBIR.Trim() + ",");
            sb.Append(MenuItem.MIMBGR.Trim() + ",");
            sb.Append(MenuItem.MIMBQU + ",");
            sb.Append(MenuItem.MIMGRA.Trim() + ",");
            sb.Append(MenuItem.MIMIST.Trim() + ",");
            sb.Append(MenuItem.MIMCLR.Trim() + ",");
            sb.Append(MenuItem.MIMNPI + ",");
            sb.Append(MenuItem.MIMNPO + ",");
            sb.Append(MenuItem.MIMNPD + ",");
            sb.Append(MenuItem.MIMSKI + ",");
            sb.Append(MenuItem.MIMBMI + ",");
            sb.Append(MenuItem.MIMNPA + ",");
            sb.Append(MenuItem.MIMNNP + ",");
            sb.Append(MenuItem.MIMNPT.Trim() + "\n");

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
            sb.Append(RawItem.RIMUPC + ",");
            sb.Append(RawItem.RIMSUQ + ",");
            sb.Append(RawItem.RIMLAY + ",");
            sb.Append(RawItem.RIMCPR + ",");
            sb.Append(RawItem.RIMCPN + ",");
            if (RawItem.RIMPDT != null)
                sb.Append(((DateTime)RawItem.RIMPDT).ToString("yyyy-MM-dd") + ",");
            sb.Append(RawItem.RIMPVN + ",");
            sb.Append(RawItem.RIMSVN + ",");
            sb.Append(RawItem.RIMCWC.Trim() + ",");
            sb.Append(RawItem.RIMPRO.Trim() + ",");
            sb.Append(RawItem.RIMSE4.Trim() + ",");
            sb.Append(RawItem.RIMERT.Trim() + ",");
            sb.Append(RawItem.RIMUSF + ",");
            sb.Append(RawItem.RIMSDP + ",");
            sb.Append(RawItem.RIMUS1 + ",");
            sb.Append(RawItem.RIMUS2 + ",");
            sb.Append(RawItem.RIMUS3 + ",");
            sb.Append(RawItem.RIMUS4 + ",");
            sb.Append(RawItem.RIMUS5 + ",");
            sb.Append(RawItem.RIMUSX + ",");
            sb.Append(RawItem.RIMMSD + ",");
            sb.Append(RawItem.RIMMSL + ",");
            sb.Append(RawItem.RIMLA1.Trim() + ",");
            sb.Append(RawItem.RIMLA2.Trim() + ",");
            sb.Append(RawItem.RIMLP1 + ",");
            sb.Append(RawItem.RIMLP2 + ",");
            sb.Append(RawItem.RIMSTA.Trim() + ",");
            sb.Append(RawItem.RIMUSR.Trim() + ",");
            sb.Append(((DateTime)RawItem.RIMDAT).ToString("yyyy-MM-dd") + ",");
            sb.Append(((DateTime)RawItem.RIMEDT).ToString("yyyy-MM-dd") + ",");
            sb.Append(RawItem.RIMFLG + ",");
            sb.Append(RawItem.RIMORD.Trim() + ",");
            sb.Append(RawItem.RIMLIN + ",");
            sb.Append(RawItem.RIMADE.Trim() + ",");
            sb.Append(RawItem.RIMBAR.Trim() + "\n");
            return sb;
        }
    }
}