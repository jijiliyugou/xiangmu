using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Common.SystemHelper
{
    public class StringHepler
    {
        /// <summary>
        ///  过滤特殊符
        /// </summary>
        /// <param name="SpecialString"></param>
        /// <returns></returns>
        public string FilterString(String SpecialString)
        {
            foreach (var a in SpecialString)
            {
                byte[] bts = Encoding.UTF32.GetBytes(a.ToString());

                if ((bts[0].ToString() == "253" && bts[1].ToString() == "255")||bts[1].ToString() == "39")
                {
                    SpecialString = SpecialString.Replace(a.ToString(), "");
                }
            }
            return SpecialString;
        }
    }
}
