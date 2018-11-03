using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class uinfoDal
    {
        private static string tableName = "u_info";
        private static string keyName = "uid";


        /// <summary>
        /// Get List By Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<uinfoEntity> GetListByWhere(string where, ref string info)
        {
            try
            {
                where = where.ToLower().Trim();
                if (where.StartsWith("and")) where = where.Substring(3);
                if (!string.IsNullOrEmpty(where) && where.IndexOf("where") < 0) where = " where " + where;

                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, "select * from " + tableName + where).Tables[0];
                return DBAccess.GetEntityList<uinfoEntity>(dt);

            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new List<uinfoEntity>();
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static uinfoEntity GetByKeyId(int id, ref string info)
        {
            try
            {
                uinfoEntity entity = new uinfoEntity();
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, id)).Tables[0];
                var list = DBAccess.GetEntityList<uinfoEntity>(dt);
                if (list != null && list.Count > 0)
                    entity = list[0];

                return entity;
            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new uinfoEntity();
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool CheckUidAndPwd(string userid,string pwd, ref string info)
        {
            bool r = false;
            try
            {
               
                int count = int.Parse(DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, string.Format("select count(1) from {0} where userid='{1}' and userpwd='{2}';", tableName, userid, pwd)));
                r = count > 0;
                return r;
            }
            catch (Exception ex)
            {
                info = ex.Message;
            }
            return r;
        }

        public static bool Update(uinfoEntity item)
        {
            DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.uid)).Tables[0];
            if (dt.Rows.Count == 1)
            {
                dt.Rows[0]["changepwd"] = item.changepwd;
                dt.Rows[0]["countrycode"] = item.countrycode;
                dt.Rows[0]["nikename"] = item.nikename;
                dt.Rows[0]["regdatetime"] = item.regdatetime;
                dt.Rows[0]["userid"] = item.userid;
                dt.Rows[0]["puid"] = item.puid;
                dt.Rows[0]["usertype"] = item.usertype;
                dt.Rows[0]["wkcaddress"] = item.wkcaddress;
                dt.Rows[0]["userpwd"] = item.userpwd;
                dt.Rows[0]["wkccheckmoney"] = item.wkccheckmoney;
                dt.Rows[0]["wkccheckpass"] = item.wkccheckpass;
                dt.Rows[0]["datachange_lasttime"] = DateTime.Now;
                dt.Rows[0]["isvip"] = item.isvip;
                dt.Rows[0]["vipdate"] = item.vipdate;
                dt.Rows[0]["lastchecktime"] = item.lastchecktime;
                return DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
            }
            else
                return true;
        }

        public static bool Insert(uinfoEntity item,ref string fuid)
        {
            bool rv = true;
            try
            {
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.uid)).Tables[0];
                DataRow dr = dt.NewRow();
                dr["changepwd"] = item.changepwd;
                dr["countrycode"] = item.countrycode;
                dr["nikename"] = item.nikename;
                dr["regdatetime"] = item.regdatetime;
                dr["userid"] = item.userid;
                dr["puid"] = item.puid;
                dr["usertype"] = item.usertype;
                dr["wkcaddress"] = item.wkcaddress;
                dr["userpwd"] = item.userpwd;
                dr["wkccheckmoney"] = item.wkccheckmoney;
                dr["wkccheckpass"] = item.wkccheckpass;
                dr["datachange_lasttime"] = DateTime.Now;
                dr["isvip"] = 0;
                dr["vipdate"] = DateTime.Now.Date.AddDays(-1);
                dt.Rows.Add(dr);
                rv = DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
                if (rv)
                {
                    string info = string.Empty;
                    var ulist = uinfoDal.GetListByWhere(string.Format("userid='{0}' and countrycode='{1}'", item.userid, item.countrycode), ref info);
                    if (ulist != null && ulist.Count > 0)
                    {
                        fuid = ulist[0].uid.ToString();
                        DBAccess.DataAccess.Miou_GetDataScalarBySql("", string.Format(@"
INSERT INTO  u_account 
	(uid, 
	accountmony, 
	stopmoney
	)
	VALUES
	({0}, 0, 0);
 
select 1;
", ulist[0].uid));

                        rv = true;

                    }

                }
            }
            catch(Exception ex) {
                string ss = ex.ToString();  
                rv = false;
            }
            return rv;
        }

     
    }
}
