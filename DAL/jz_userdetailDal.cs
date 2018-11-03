
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class jz_userdetailDal
    {
        private static string tableName = "jz_userdetail";
        private static string keyName = "zjuid";

        public static bool Insert(jz_userdetailEntity item)
        {
            bool rv = true;
            try
            {
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, 0)).Tables[0];
                DataRow dr = dt.NewRow();

                dr["zjuid"] = item.zjuid;
                dr["uid"] = item.uid;
                dr["fmoney"] = item.fmoney;
                dr["fmoney"] = item.fmoney;
                dr["remark"] = item.remark;
                dr["createtime"] = DateTime.Now;
 
                dt.Rows.Add(dr);
                rv = DBAccess.DataAccess.Miou_UpdateDataSet(DBAccess.LogUName, tableName, "*", "1<>1", "", dt).StartsWith("000");

            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                rv = false;
            }
            return rv;
        }

        /// <summary>
        /// Get List By Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<jz_userdetailEntity> GetListAll()
        {
            try
            {
               
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, "SELECT 0 AS zjuid ,u_info.uid,u_info.nikename,SUM(fmoney) AS fmoney,MAX(createtime) AS createtime FROM jz_userdetail,u_info WHERE u_info.uid=jz_userdetail.uid GROUP BY u_info.uid,u_info.nikename ORDER BY SUM(fmoney) DESC").Tables[0];
                return DBAccess.GetEntityList<jz_userdetailEntity>(dt);

            }
            catch (Exception ex)
            {
              
                return new List<jz_userdetailEntity>();
            }
        }
    }
}
