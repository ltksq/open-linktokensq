using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class RandomHelp
    {
        /// <summary>
        /// 获取多少数据项的数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<int> GetRandomArry(int num)
        {
            List<int> r = new List<int>();
            for(int i = 1; i <= num; i++)
            {
                r.Add(i);
            }
            return r;
        }

        /// <summary>
        /// 数据项，返回百分比
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pe"></param>
        /// <returns></returns>
        public static List<int> RandomPe(List<int> items,int pe)
        {
            List<int> list = new List<int>();
            while (list.Count < pe && items.Count>0)
            {
                Random rd = new Random();
                int j=rd.Next(items.Count - 1);
                list.Add(items[j]);
                items.RemoveAt(j);
            }

            return list;
        }

   
    }
}