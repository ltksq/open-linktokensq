using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class uimportmoneyDal
    {
        private static string tableName = "u_importmoney";
        private static string keyName = "inmid";

        public static bool Insert(uimportmoneyEntity item)
        {
            bool rv = true;
            try
            {
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select  * from {0} where {1} = {2}  ", tableName, keyName, 0)).Tables[0];
                DataRow dr = dt.NewRow();
                dr["uid"] = item.uid;
                dr["wkcaddress"] = item.wkcaddress;
                dr["thash"] = item.thash;
                dr["indatetime"] = item.indatetime;
                dr["fmoney"] = item.fmoney;
                dt.Rows.Add(dr);
                rv = DBAccess.DataAccess.Miou_UpdateDataSet("", tableName, "*", "1<>1", "", dt).StartsWith("000");
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                rv = false;
            }
            return rv;
        }

        public static bool AutoInMoney(string inaddress,string hash,double fmoney,DateTime dt,ref string info)
        {
            bool rv = false;

            try
            {
                if (((TimeSpan)(DateTime.Now - dt)).TotalHours >= 3)
                {
                    info = "过期不处理";
                    return rv;
                }

                string sv = DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName
                    , string.Format("select  count(1) from {0} where wkcaddress='{1}' and thash = '{2}'  ", tableName,inaddress, hash)).ToString();
                if (sv == "1")
                {
                    info = "已处理";
                    return rv;
                }
                string msg = "";
                var uinfo=uinfoDal.GetListByWhere(string.Format("wkcaddress='{0}'",inaddress), ref msg);
                if(uinfo!=null && uinfo.Count > 0)
                {
                    var uacc = uaccountDal.GetByKeyId(uinfo[0].uid, ref msg);
                    if (uacc != null)
                    {
                        uimportmoneyEntity ui = new uimportmoneyEntity();
                        ui.uid = uinfo[0].uid;
                        ui.indatetime = dt;
                        ui.fmoney = fmoney;
                        ui.thash = hash;
                        ui.wkcaddress = inaddress;
                        rv = uimportmoneyDal.Insert(ui);
                        if (rv)
                        {
                            DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, string.Format(" CALL accountInMoney({0},{1})", ui.uid, ui.fmoney));
                        }
                    }
                    else
                    {
                        info = "未找到用户资金信息";
                    }
                }
                else
                {
                    info = "未找到用户";
                }
            }
            catch { }
            return rv;
        }


    }
}
