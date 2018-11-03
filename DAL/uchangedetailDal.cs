using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class uchangedetailDal
    {
        private static string tableName = "u_changedetail";
        private static string keyName = "detid";

        public static List<uchangedetailEntity> PageData(string where,int currpage,int pagesize=15)
        {
            DataTable dt = DBAccess.DataAccess.InfoPagerData(DBAccess.LogUName, "*", keyName, tableName, where, " detid desc", pagesize, currpage).Tables[0];
            return DBAccess.GetEntityList<uchangedetailEntity>(dt);
        }

        public static int DataCount(string where)
        {
            return DBAccess.DataAccess.InfoPagerCount(DBAccess.LogUName, keyName, tableName, where);
        }

        public static bool Update(uchangedetailEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.detid)).Tables[0];
            if (dt.Rows.Count == 1)
            {
             
                dt.Rows[0]["ftype"] = item.ftype;
                dt.Rows[0]["remark"] = item.remark;
                dt.Rows[0]["datachange_lasttime"] = DateTime.Now;

                return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
            }
            else
                return true;
        }

        public static bool Inert(uchangedetailEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.detid)).Tables[0];

            DataRow dr = dt.NewRow();
            dr["ftype"] = item.ftype;
            dr["fmoney"] = item.fmoney;
            dr["uid"] = item.uid;
            dr["remark"] = item.remark;
            dr["datachange_lasttime"] = DateTime.Now;
            dt.Rows.Add(dr);
            return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
        }

       
    }
}
