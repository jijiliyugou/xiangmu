using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;

namespace Yaeher.Common.EventHelper
{
    public class EventPrincipalHost
    {
        public enum ClientType
        {
            /// <summary>
            /// 订阅消息事件
            /// </summary>
            Subscribe = 0,
            /// <summary>
            ///   发布消息事件
            /// </summary>
            Publishs = 1,
            /// <summary>
            /// 取消订阅
            /// </summary>
            UnSubscribe = 2

        }
        HttpHelper httpHelper = new HttpHelper();

        /// <summary>
        /// 链接Event的一些事件方法
        /// </summary>
        /// <param name="clientType">枚举类型订阅还是发布</param>
        /// <param name="uri">EventSite</param>
        /// <param name="objectInfo">需要传递的Json对象</param>
        /// <returns></returns>
        public async Task<String> ClientPost(ClientType clientType,string uri, object objectInfo)
        {
            if (objectInfo != null)
            {
                string paramStr = string.Empty;
                string Result = string.Empty;
                switch (clientType)
                {
                    case ClientType.Subscribe:
                        paramStr = "SubscribeInfo=" + JsonHelper.ToJson(objectInfo);
                        Result =await httpHelper.PostResponseAsync(uri+"/Event/ServerSubscribe", paramStr);  // 将订阅注册到服务端
                        return Result;
                    case ClientType.Publishs:
                        paramStr = "PublishsInfo=" + JsonHelper.ToJson(objectInfo);
                        Result = await httpHelper.PostResponseAsync(uri + "/Event/ServerPublishs", paramStr);  // 将发布注册到服务端
                        return Result;
                    case ClientType.UnSubscribe://取消订阅
                        paramStr = "SubscribeID=" + objectInfo;
                        Result = await httpHelper.PostResponseAsync(uri + "/Event/ServerUnSubscribe", paramStr);  // 将发布注册到服务端
                        return Result;
                    default:
                        return "fail";
                }
            }
            else
            {
                return "fail";
            }
        }
    }
}
