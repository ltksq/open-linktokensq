using Miou.Common.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBAccess
    {
        static string dbConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static string dbType = System.Configuration.ConfigurationManager.ConnectionStrings["DbType"].ToString();

        static DbBaseAccess dataAccess = null;
        public static string LogUName = "sys";
        static DBAccess()
        {
            try
            {
                dataAccess = new Miou.Common.DB.DbBaseAccess(dbConnString, LogUName, dbType);
            }
            catch { }
        }

        public static Miou.Common.DB.DbBaseAccess DataAccess
        {
            get { return dataAccess; }
        }


        /// <summary>
        /// 根据IDataReader中的数据生成实体类集合
        /// </summary>
        /// <typeparam name="T">实体层的类</typeparam>
        /// <param name="dt">表</param>
        /// <returns>集合</returns>
        public static List<T> GetEntityList<T>(IDataReader dr) where T : new()
        {
            List<T> entityList = new List<T>();

            int fieldCount = -1;

            while (dr.Read())
            {
                if (-1 == fieldCount)
                    fieldCount = dr.FieldCount;

                T t = (T)Activator.CreateInstance(typeof(T));

                for (int i = 0; i < fieldCount; i++)
                {
                    PropertyInfo field = t.GetType().GetProperty(dr.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (field == null)
                        continue;

                    if (null == dr[i] || Convert.IsDBNull(dr[i]))
                        field.SetValue(t, null, null);
                    else if (dr[i] is decimal || dr[i] is float)
                        field.SetValue(t, Convert.ToDouble(dr[i]), null);
                    else
                        field.SetValue(t, dr[i], null);
                }
                entityList.Add(t);
            }

            dr.Close();

            if (entityList.Count == 0)
                return null;

            return entityList;
        }


        /// <summary>
        /// 根据IDataReader中的数据生成实体类集合
        /// </summary>
        /// <typeparam name="T">实体层的类</typeparam>
        /// <param name="dt">表</param>
        /// <returns>集合</returns>
        public static List<T> GetEntityList<T>(DataTable dr) where T : new()
        {
            List<T> entityList = new List<T>();

            int fieldCount = -1;

            for (int j = 0; j < dr.Rows.Count; j++)
            {
                if (-1 == fieldCount)
                    fieldCount = dr.Columns.Count;

                T t = (T)Activator.CreateInstance(typeof(T));

                for (int i = 0; i < fieldCount; i++)
                {
                    PropertyInfo field = t.GetType().GetProperty(dr.Columns[i].ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (field == null)
                        continue;

                    if (null == dr.Rows[j][i] || Convert.IsDBNull(dr.Rows[j][i]))
                        field.SetValue(t, null, null);
                    else if (string.Compare(field.PropertyType.FullName, "system.int32", true) == 0
   && string.Compare(dr.Columns[i].DataType.FullName, "system.int64", true) == 0)
                    {
                        field.SetValue(t, int.Parse(dr.Rows[j][i].ToString()), null);
                    }
                    else if (string.Compare(field.PropertyType.FullName, "system.int32", true) == 0
                       && string.Compare(dr.Columns[i].DataType.FullName, "System.Decimal", true) == 0)
                    {
                        field.SetValue(t, int.Parse(dr.Rows[j][i].ToString()), null);
                    }
                    else if (string.Compare(field.PropertyType.FullName, "system.int32", true) == 0
                  && string.Compare(dr.Columns[i].DataType.FullName, "System.double", true) == 0)
                    {
                        field.SetValue(t, int.Parse(dr.Rows[j][i].ToString()), null);
                    }
                    else if (dr.Rows[j][i] is decimal || dr.Rows[j][i] is float)
                        field.SetValue(t, Convert.ToDouble(dr.Rows[j][i]), null);
                    else if (dr.Columns[i].DataType.FullName == "System.UInt64")
                        field.SetValue(t, Convert.ToBoolean(dr.Rows[j][i]), null);
                    else
                        field.SetValue(t, dr.Rows[j][i], null);
                }
                entityList.Add(t);
            }

            return entityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTabelBySql(string sql)
        {
            return DBAccess.DataAccess.Miou_GetDataSetBySqlNoKey(DBAccess.LogUName, sql).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ExecSql(string sql)
        {
            return DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, sql).ToString();
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static void FillDataRow(ref DataRow dr, object model)
        {

            foreach (PropertyInfo propertyInfo in model.GetType().GetProperties())
            {
                bool havle = false;
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    if (string.Compare(dr.Table.Columns[i].ColumnName, propertyInfo.Name, true) == 0)
                    {
                        havle = true;
                        break;
                    }
                }
                if (havle)
                    dr[propertyInfo.Name] = propertyInfo.GetValue(model, null);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fmoney"></param>
        /// <param name="type">30活动支出</param>
        /// <returns></returns>
        public static int UserPay(int uid, double fmoney,int type=30)
        {
            int count = 0;
            int.TryParse(DBAccess.DataAccess.Miou_GetDataScalarBySql(DBAccess.LogUName, string.Format("CALL  accountPay ({0},{1},{2}) ", uid, fmoney,type)), out count);

            return count;
        }
    }
}
