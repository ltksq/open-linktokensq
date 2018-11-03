using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PostResponse
    {
        public bool IsSuccess { get; set; }

        public int Result { get; set; }
        /// <summary>
        /// 消息1
        /// </summary>
        public string Message { get; set; }

   
        public object Data { get; set; }
    }
}
