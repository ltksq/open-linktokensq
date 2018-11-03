using Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
 
    public class c_voteDal
    {
        private static string tableName = "c_vote";
        private static string keyName = "voteid";

        public static c_voteEntity GetByKeyId(int id, ref string info)
        {
            try
            {
                c_voteEntity entity = new c_voteEntity();
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, id)).Tables[0];
                var list = DBAccess.GetEntityList<c_voteEntity>(dt);
                if (list != null && list.Count > 0)
                    entity = list[0];

                return entity;
            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new c_voteEntity();
            }
        }
        /// <summary>
        /// Get List By Where
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<c_voteEntity> GetListByWhere(string where, ref string info)
        {
            try
            {
                where = where.ToLower().Trim();
                if (where.StartsWith("and")) where = where.Substring(3);
                if (!string.IsNullOrEmpty(where) && where.IndexOf("where") < 0) where = " where " + where;

                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName
                    , @"select *
,(SELECT COUNT(1) FROM c_voteuser u WHERE u.voteid=c_vote.voteid) AS joinusercount 
,IFNULL((SELECT SUM(wkcamount) FROM c_voteuser u WHERE u.voteid=c_vote.voteid),0) AS ticketcount 
,paywkcamount+IFNULL((SELECT SUM(paywkcamount) FROM c_voteuser u WHERE u.voteid=c_vote.voteid),0) AS useamount  
from " + tableName + " " + where).Tables[0];
                return DBAccess.GetEntityList<c_voteEntity>(dt);

            }
            catch (Exception ex)
            {
                info = ex.Message;
                return new List<c_voteEntity>();
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="wherestr"></param>
        /// <param name="order"></param>
        /// <param name="pagesize">一页记录数</param>
        /// <param name="currpage">当前页</param>
        /// <returns></returns>
        public static List<c_voteEntity> InfoPagerData(string wherestr, int pagesize, int currpage)
        {
            try
            {
                if (currpage <= 0) currpage = 1;
                int intStartRecords = (currpage - 1) * pagesize;//计算从第几条开始获取

                string strSql = String.Format(@"
select 
c_vote.*
,(SELECT COUNT(1) FROM c_voteuser u WHERE u.voteid=c_vote.voteid) AS joinusercount 
,IFNULL((SELECT SUM(wkcamount) FROM c_voteuser u WHERE u.voteid=c_vote.voteid),0) AS ticketcount 
,paywkcamount+IFNULL((SELECT SUM(paywkcamount) FROM c_voteuser u WHERE u.voteid=c_vote.voteid),0) AS useamount 
from {0} 
WHERE c_vote.votestatus<5 {1} 
LIMIT {2},{3} ", tableName, wherestr, intStartRecords, pagesize);

                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, strSql).Tables[0];
                return DBAccess.GetEntityList<c_voteEntity>(dt);

            }
            catch (Exception ex)
            {
                return new List<c_voteEntity>();
            }
        }


        public static int GetCountByWhere(string where)
        {
            try
            {
                where = where.ToLower().Trim();
                if (where.StartsWith("and")) where = where.Substring(3);
                if (!string.IsNullOrEmpty(where) && where.IndexOf("where") < 0) where = " where " + where;

                return int.Parse(DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, "select count(1) from " + tableName + " " + where));

            }
            catch (Exception ex)
            {

                return 0;
            }
        }
 
        public static bool Update(c_voteEntity item, ref string info)
        {
            bool rv = true;
            try
            {
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, item.voteid)).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    item.lasttime = DateTime.Now;
                    DataRow dr = dt.Rows[0];
                    DBAccess.FillDataRow(ref dr, item);

                    string vs = DBAccess.DataAccess.Miou_UpdateDataSet(DBAccess.LogUName, tableName, "*", "1<>1", "", dt);
                    if (!vs.StartsWith("000000"))
                    {
                        rv = false;
                        info = vs.Substring(6);
                    }
                }
                else
                {
                    rv = false;
                    info = "找不到记录!";
                }
            }
            catch (Exception ex)
            {
                info = ex.Message;
                rv = false;
            }

            return rv;


        }

        public static long Insert(c_voteEntity item)
        {
            long id = 0;
            try
            {
                DataTable dt = DBAccess.DataAccess.Miou_GetDataSetBySql(DBAccess.LogUName, string.Format("select * from {0} where {1} = {2} ;", tableName, keyName, 0)).Tables[0];
                dt.TableName = tableName;
                DataRow dr = dt.NewRow();
                item.createtime = DateTime.Now;
                item.lasttime = item.createtime;
                DBAccess.FillDataRow(ref dr, item);
                List<long> ids = new List<long>();
              
                dt.Rows.Add(dr);
               
                if (DBAccess.DataAccess.Miou_UpdateDataTablesNoTranWithIndentity(DBAccess.LogUName
                    , new List<DataTable> { dt }, out ids).StartsWith("000"))
                {
                    if (ids.Count > 0) id = ids[0];
                }
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                 
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="merchantId">商户id</param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool Del(string id, ref string info)
        {
            bool rv = true;

            try
            {
                string vs = DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, string.Format("delete from {0} where {1}={2};delete from mo_onecloud_dayreport where onecloundid={2} ; select '000000';", tableName, keyName, id));
                if (!vs.StartsWith("000000"))
                {
                    rv = false;
                    info = vs;
                }
            }
            catch (Exception ex)
            {
                info = ex.Message;
            }
            return rv;
        }
    }
}
