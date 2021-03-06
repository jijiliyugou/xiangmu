﻿using Newtonsoft.Json;
using System.Xml.Linq;

namespace Yaeher.Common
{

    /// <summary>
    /// 提供了一个关于json的辅助类
    /// </summary>
    public static class JsonHelper
   {

       #region Method
       /// <summary>
        /// 类对像转换成json格式
        /// </summary> 
        /// <returns></returns>
        public static string ToJson(object t)
        {
            return  JsonConvert.SerializeObject(t, Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling =  NullValueHandling.Include });
        }
       /// <summary>
        /// 类对像转换成json格式
       /// </summary>
       /// <param name="t"></param>
       /// <param name="HasNullIgnore">是否忽略NULL值</param>
       /// <returns></returns>
        public static string ToJson(object t, bool HasNullIgnore)
        {
            if (HasNullIgnore)
                return JsonConvert.SerializeObject(t, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            else
               return ToJson(t);
        }
        /// <summary>
        /// json格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            if(!string.IsNullOrEmpty(strJson))
                return JsonConvert.DeserializeObject<T>(strJson);
            return null;
        }

        /// <summary>
        /// 获取Xml结果中对应节点的值
        /// </summary>
        /// <param name="_resultXml"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetXmlValue(XDocument _resultXml, string nodeName)
        {
            if (_resultXml == null || _resultXml.Element("xml") == null
                || _resultXml.Element("xml").Element(nodeName) == null)
            {
                return "";
            }
            return _resultXml.Element("xml").Element(nodeName).Value;
        }
        #endregion

    }
}
