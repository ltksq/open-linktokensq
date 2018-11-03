using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConstPar
{
    public class ConstPar
    {
        
        /// <summary>
        /// 税率
        /// </summary>
        public static double TaxRate = string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["TaxRate"]) ? 2.0 : double.Parse(System.Configuration.ConfigurationSettings.AppSettings["ChangCommission"]);

        /// <summary>
        /// 官方wkc钱包地址
        /// </summary>
        public static string CZ_WKCAddress = System.Configuration.ConfigurationSettings.AppSettings["CZ_WKCAddress"];

       
    }
}
