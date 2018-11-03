using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Concurrent;
namespace Common.Cache
{
    public class SCode
    {
        public string code { get; set; }
        public byte[] value { get; set; }
    }
    public class SecurityCode
    {

        private static readonly SecurityCode instance = new SecurityCode();


        public ConcurrentQueue<SCode> CodeList = new ConcurrentQueue<SCode>();
 

        private SecurityCode()
        {

        }

        public static SecurityCode GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 装载用户数据进缓存
        /// </summary>
        public void LoadDataToCache()
        {
 
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        int count = CodeList.Count;
                        for (int i = count; i < 500; i++)
                        {
                            SCode sc = new SCode();
                            sc.code= Common.Cache.SecurityCode.CreateRandomCode(rand.Next(4,6)); //验证码的字符
                            sc.value=Common.Cache.SecurityCode.CreateValidateGraphic(sc.code);
                            sc.code = sc.code.Replace(" ", "");
                            CodeList.Enqueue(sc);
                        }
                    }
                    catch
                    {

                    }
            
                    System.Threading.Thread.Sleep(200);
                }
            });

            task.Start();
        }

        static Random rand = new Random();

        /// <summary>
        /// 生成随机的字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,u,s,t,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";

           
            for (int i = 0; i < codeCount; i++)
            {
                int t = rand.Next(0,allCharArray.Length);
                randomCode += allCharArray[t];
            }

            while (randomCode.Length < 7)
            {
                int i = rand.Next(0, randomCode.Length);
                randomCode = randomCode.Insert(i, " ");
            }

            return randomCode;
        }

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * double.Parse(rand.Next(13,18).ToString())), 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 30; i++)
                {
                    int x1 = rand.Next(image.Width);
                    int x2 = rand.Next(image.Width);
                    int y1 = rand.Next(image.Height);
                    int y2 = rand.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", rand.Next(13,18), (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰线
                for (int i = 0; i < 150; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(rand.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
