using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yaeher.Common.Constants
{
    /// <summary>
    /// 生成随机
    /// </summary>
    public class RandomCode
    {

        private int rep = 0;
        /// <summary>
        /// 随机生成不重复数字字符串  
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <returns></returns>
        public string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
        /// <summary>
        /// 随机生成字符串（数字和字母混和） 
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <returns></returns>
        public string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string RamdomRecode(int num)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXZ";
            Random randrom = new Random();
            string str = "";
            for (int i = 0; i < num; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }
    }
}
