using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class uaccountDal
    {
        private static string tableName = "u_account";
        private static string keyName = "uid";

        /// <summary>
        ///  
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static uaccountEntity GetByKeyId(int id, ref string info)
        {
            try
            {
                uaccountEntity entity = new uaccountEntity();
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, id)).Tables[0];
                var list = DBAccess.GetEntityList<uaccountEntity>(dt);
                if (list != null && list.Count > 0)
                    entity = list[0];

                return entity;
            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new uaccountEntity();
            }
        }

        public static bool Update(uaccountEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.uid)).Tables[0];
            if (dt.Rows.Count == 1)
            {

                dt.Rows[0]["accountmony"] = item.accountmony;
                dt.Rows[0]["stopmoney"] = item.stopmoney;
                return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
            }
            else
                return true;
        }

       
          
        public static double GetUserBlance(int uid)
        {
            
            double count = 0;
            double.TryParse(DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, string.Format("select accountmony from u_account where uid={0} ", uid)), out count);

            return count;
        }

       
    }
}
