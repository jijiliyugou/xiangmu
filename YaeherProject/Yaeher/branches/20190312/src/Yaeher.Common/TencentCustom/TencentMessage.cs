using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;
using Yaeher.SystemConfig;

namespace Yaeher.Common.TencentCustom
{
    /// <summary>
    /// 
    /// </summary>
    public class TencentMessage
    {
        
    }
    public abstract class MessageBase
    {
        
        protected string requestUri;
        public MessageBase()
        { }
        public async Task<string> Send(string systemToken)
        {
            try
            {
                HttpHelper httpHelper = new HttpHelper();
                TencentToken tencentToken = new TencentToken();
                this.requestUri += systemToken;
                var httpContent = this.CreateHttpClient();
                var SendMessage=  await  httpHelper.PostResponseAsync(this.requestUri, httpContent);
                return SendMessage;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        protected abstract HttpContent CreateHttpClient();
    }
    public abstract class SingleMessageBase : MessageBase
    {
        public SingleMessageBase()
        {
            base.requestUri = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=";
        }
    }
    /// <summary>
    /// 文字发送
    /// </summary>
    public class TextSingleMessage : SingleMessageBase
    {
        public string ToUser { get; set; }
        public string TextContent { get; set; }
        public TextSingleMessage() { }
        protected override HttpContent CreateHttpClient()
        {
            return new StringContent(JsonHelper.ToJson(new
            {
                touser = ToUser,
                msgtype = "text",
                text = new
                {
                    content = TextContent
                }
            }));
        }
    }
    /// <summary>
    /// 图片发送
    /// </summary>
    public class ImageSingleMessage : SingleMessageBase
    {
        public string ToUser { get; set; }
        public string MediaId { get; set; }

        public ImageSingleMessage() { }

        protected override HttpContent CreateHttpClient()
        {
            return new StringContent(JsonHelper.ToJson(new
            {
                touser = ToUser,
                msgtype = "image",
                image = new
                {
                    media_id = MediaId
                }
            }));
        }
    }
    /// <summary>
    /// 语音发送
    /// </summary>
    public class VoiceSingleMessage : SingleMessageBase
    {
        public string ToUser { get; set; }
        public string MediaId { get; set; }

        public VoiceSingleMessage() { }

        protected override HttpContent CreateHttpClient()
        {
            return new StringContent(JsonHelper.ToJson(new
            {
                touser = ToUser,
                msgtype = "voice",
                voice = new
                {
                    media_id = MediaId
                }
            }));
        }
    }
    /// <summary>
    /// 视频发送
    /// </summary>
    public class VideoSingleMessage : SingleMessageBase
    {
        public string ToUser { get; set; }
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public VideoSingleMessage() { }

        protected override HttpContent CreateHttpClient()
        {
            return new StringContent(JsonHelper.ToJson(new
            {
                touser = ToUser,
                msgtype = "video",
                video = new
                {
                    media_id = MediaId,
                    thumb_media_id = ThumbMediaId,
                    title = Title,
                    description = Description
                }
            }));
        }
    }
    /// <summary>
    /// 发送图文消息
    /// </summary>
    public class NewsSingleMessage : SingleMessageBase
    {
        public string ToUser { get; set; }

        public List<SendNewsMessageArticlesModel> Articles { get; set; }

        public NewsSingleMessage() { }

        protected override HttpContent CreateHttpClient()
        {
            return new StringContent(JsonHelper.ToJson(new
            {
                touser = ToUser,
                msgtype = "news",
                news = new
                {
                    articles = Articles
                }
            }));
        }
    }



}
