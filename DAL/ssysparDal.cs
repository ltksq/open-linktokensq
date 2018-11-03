using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ssysparDal
    {
        private static string tableName = "s_syspar";
        private static string keyName = "keyname";

        /// <summary>
        /// Get List By Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<ssysparEntity> GetListByWhere(string where, ref string info)
        {
            try
            {
                where = where.ToLower().Trim();
                if (where.StartsWith("and")) where = where.Substring(3);
                if (!string.IsNullOrEmpty(where) && where.IndexOf("where") < 0) where = " where " + where;

                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, "select * from " + tableName + where).Tables[0];
                return DBAccess.GetEntityList<ssysparEntity>(dt);

            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new List<ssysparEntity>();
            }
        }
    }
}
