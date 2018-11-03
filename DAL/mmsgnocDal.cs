using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class mmsgnocDal
    {
        private static string tableName = "m_msgnoc";
        private static string keyName = "msgid";

        /// <summary>
        /// Get List By Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<mmsgnocEntity> GetListByWhere(string where, ref string info)
        {
            try
            {
                where = where.ToLower().Trim();
                if (where.StartsWith("and")) where = where.Substring(3);
                if (!string.IsNullOrEmpty(where) && where.IndexOf("where") < 0) where = " where " + where;

                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, "select * from " + tableName + where).Tables[0];
                return DBAccess.GetEntityList<mmsgnocEntity>(dt);

            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new List<mmsgnocEntity>();
            }
        }

        public static bool Insert(mmsgnocEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.msgid)).Tables[0];
            DataRow dr = dt.NewRow();
            dr["msgcode"] = item.msgcode;
            dr["stopdatetime"] = item.stopdatetime;
            dr["mobileno"] = item.mobileno;

            dt.Rows.Add(dr);
            return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");

        }
 

        public static bool Update(mmsgnocEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.msgid)).Tables[0];
            if (dt.Rows.Count == 1)
            {
                dt.Rows[0]["msgcode"] = item.msgcode;
                dt.Rows[0]["stopdatetime"] = item.stopdatetime;
                dt.Rows[0]["mobileno"] = item.mobileno;

                return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
            }
            else
                return true;
        }
    }
}
