using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.Constants;
using Yaeher.Common.HttpHelpers;
using Yaeher.Common.SendMsm;
using Yaeher.Common.SystemHelper;
using Yaeher.SystemConfig;

namespace Yaeher.Common.CloudCallCenter
{
    /// <summary>
    /// Ali呼叫中心
    /// </summary>
    public  class AliCallCenter
    {
        /// <summary>
        /// 发起双呼
        /// </summary>
        public async Task<String> StartBack2BackCall(YaeherPhone yaeherPhone)
        {
            AliCCC aliCCC = new AliCCC();
            AliAccessToken aliAccessToken = new AliAccessToken();
            AliAccessTokenEntity aliAccessTokenEntity = new AliAccessTokenEntity();
            AccessTokenInfo accessTokenInfo = new AccessTokenInfo();
            HttpHelper httpHelper = new HttpHelper();
            #region 公共参数
            aliCCC.Format = "JSON";
            aliCCC.Version = "2017-07-05";
            aliCCC.Timestamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            aliCCC.SignatureType = "BEARERTOKEN";
            aliCCC.RegionId = "cn-shanghai";
            aliCCC.SignatureNonce =new RandomCode().GenerateCheckCode(45);
            ///人工维护的token  10个月维护一次
            accessTokenInfo.refresh_token ="s6P4FNojcR0t9Yk6";  
            var aliAccessTokens =await aliAccessToken.RefreshAccessToken(accessTokenInfo);
            aliCCC.BearerToken = aliAccessTokens.access_token;
            #endregion

            #region 双呼参数
            aliCCC.Action = "StartBack2BackCall";
            aliCCC.InstanceId = "33295c19-1afd-4926-ae46-cd5a28ade3e8";
            aliCCC.CallCenterNumber = yaeherPhone.CallCenterNumber;
            aliCCC.Caller = yaeherPhone.Caller;
            aliCCC.Callee = yaeherPhone.Callee;
            #endregion

            string url = "https://ccc.cn-shanghai.aliyuncs.com/?";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(url);
            stringBuilder.Append("&Format="+aliCCC.Format);
            stringBuilder.Append("&Version=" + aliCCC.Version);
            stringBuilder.Append("&Timestamp=" + aliCCC.Timestamp);
            stringBuilder.Append("&SignatureType=" + aliCCC.SignatureType);
            stringBuilder.Append("&RegionId=" + aliCCC.RegionId);
            stringBuilder.Append("&SignatureNonce=" + aliCCC.SignatureNonce);
            stringBuilder.Append("&BearerToken=" + aliCCC.BearerToken);
            stringBuilder.Append("&Action=" + aliCCC.Action);
            stringBuilder.Append("&InstanceId=" + aliCCC.InstanceId);
            //stringBuilder.Append("&WorkflowId=" + aliCCC.WorkflowId);
            stringBuilder.Append("&CallCenterNumber=" + aliCCC.CallCenterNumber);
            stringBuilder.Append("&Caller=" + aliCCC.Caller);
            stringBuilder.Append("&Callee=" + aliCCC.Callee);
            var AuthorizationCode =await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
            return AuthorizationCode.ToString();
        }
        
        /// <summary>
        /// 获取呼叫录音
        /// </summary>https://help.aliyun.com/document_detail/65296.html?spm=a2c4g.11186623.2.17.77faaaedlaOoCW
        /// <returns></returns>
        public async Task<String> ListCallDetailRecords()
        {
            AliCCC aliCCC = new AliCCC();
            HttpHelper httpHelper = new HttpHelper();
            #region 公共参数
            aliCCC.Format = "JSON";
            aliCCC.Version = "2017-07-05";
            aliCCC.Timestamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            aliCCC.SignatureType = "BEARERTOKEN";
            aliCCC.RegionId = "cn-shanghai";
            aliCCC.SignatureNonce = new RandomCode().GenerateCheckCode(45);
            ///人工维护的token
            aliCCC.BearerToken = "eyJhbGciOiJSUzI1NiIsImsyaWQiOiJlNE92NnVOUDhsMEY2RmVUMVhvek5wb1NBcVZLblNGRyIsImtpZCI6IkpDOXd4enJocUowZ3RhQ0V0MlFMVWZldkVVSXdsdEZodWk0TzFiaDY3dFUifQ.bkJudEI3OUpCcmF2aWhuNmNmbE5lbVdzNW45cFBOSWdNellrQStPYmZ5cjZVOG9rMUJqUTZqLzl3RDdkbzhHQkorMzdwYmYwR1BCTDZHUGRjaVhMVUFkaFAzeVNYanBuK2N6UkN1SnRRc3FRMGJIVTF4cVVjUDVRNUJpK2JsSWxZdlowZ2VWSzYvS2pzcVNjWHJLSlVvWkNnWE0wWGJZZ0NCVm1BaEtaTHBPVnRjdnAvUmwwV01tSFFkWnVQa0lQWEp0UFlQV3Q3QThSd0RHVTdUY1BnYlhIVy9uTmc3RVJYNk9VOENrPQ.F6WZmD5Jp3rNB7ps-FlbwaE6uqZDuFQCZv9lRwzMs8YQVln_NqiMFdhX3Wxn8TvYmFaoh2oqKkme7VkCR6e4n6cIXCa-CfC1DW5H7EaDhsJx0lU_HR01ufRI2ZBQyAKzZ72nnZPqE-7pxi4PwmArLniFJIwvv_MDgA71gY3C5KWgnDGV14qG0BWG-K78UJIItJ3xnZYUvbl3zdcgdIZOj9zI-fU4Ibj4Ods3lXF9CVG42J6E8cK1QAs0efJ4i5HvnvEMna3wdczbPpdBUQ6_0-fEpVzic6TxnDO4KO_qMrZ3ch-GBd0CQMsU3SNvWIqZ4orhz37mN9GXD3IiqEyMig";
            #endregion

            #region 获取录音地址
            aliCCC.Action = "ListCallDetailRecords";
            aliCCC.InstanceId = "33295c19-1afd-4926-ae46-cd5a28ade3e8";
            int PageNumber = 1;
            int PageSize = 100;
            string CallDetailurl = "https://ccc.cn-shanghai.aliyuncs.com/?";
            #region 公共参数
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(CallDetailurl);
            stringBuilder.Append("&Format=" + aliCCC.Format);
            stringBuilder.Append("&Version=" + aliCCC.Version);
            stringBuilder.Append("&Timestamp=" + aliCCC.Timestamp);
            stringBuilder.Append("&SignatureType=" + aliCCC.SignatureType);
            stringBuilder.Append("&RegionId=" + aliCCC.RegionId);
            stringBuilder.Append("&SignatureNonce=" + aliCCC.SignatureNonce);
            stringBuilder.Append("&BearerToken=" + aliCCC.BearerToken);
            stringBuilder.Append("&Action=" + aliCCC.Action);
            stringBuilder.Append("&InstanceId=" + aliCCC.InstanceId);
            #endregion
            stringBuilder.Append("&StartTime=" + TimeHelp.GetTimeStamp(DateTime.Parse("2018-10-10")));
            stringBuilder.Append("&StopTime=" + TimeHelp.GetTimeStamp(DateTime.Parse("2018-10-12")));
            stringBuilder.Append("&PageNumber=" + PageNumber);
            stringBuilder.Append("&PageSize=" + PageSize);
            var AuthorizationCode =await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
            var ListCallDetail = JsonHelper.FromJson<CallDetailRecordInfo>(AuthorizationCode);
            #endregion

            #region 获取下载地址
            List<ALICCListCallDetail> aLICCListCallDetailList = new List<ALICCListCallDetail>();
            if (ListCallDetail!=null)
            {
                foreach (var CallDetail in ListCallDetail.CallDetailRecords.List.CallDetailRecord)
                {
                    ALICCListCallDetail aLICCListCallDetail = new ALICCListCallDetail();
                    aLICCListCallDetail.ContactId = CallDetail.ContactId;
                    aLICCListCallDetail.StartTime = CallDetail.StartTime;
                    aLICCListCallDetail.Duration = CallDetail.Duration;
                    aLICCListCallDetail.ContactType = CallDetail.ContactType;
                    aLICCListCallDetail.ContactDisposition = CallDetail.ContactDisposition;
                    aLICCListCallDetail.CallingNumber = CallDetail.CallingNumber;
                    aLICCListCallDetail.CalledNumber = CallDetail.CalledNumber;
                    aLICCListCallDetail.InstanceId = CallDetail.InstanceId;
                    if (CallDetail.Recordings.Recording.Count > 0)
                    {
                        aLICCListCallDetail.FileName = CallDetail.Recordings.Recording[0].FileName;
                    }
                    aLICCListCallDetailList.Add(aLICCListCallDetail);
                }
            }
            StringBuilder FilestringBuilder = new StringBuilder();
            //重新获取随机值
            aliCCC.SignatureNonce = new RandomCode().GenerateCheckCode(45);
            #region
            FilestringBuilder.Append(CallDetailurl);
            FilestringBuilder.Append("&Format=" + aliCCC.Format);
            FilestringBuilder.Append("&Version=" + aliCCC.Version);
            FilestringBuilder.Append("&Timestamp=" + aliCCC.Timestamp);
            FilestringBuilder.Append("&SignatureType=" + aliCCC.SignatureType);
            FilestringBuilder.Append("&RegionId=" + aliCCC.RegionId);
            FilestringBuilder.Append("&SignatureNonce=" + aliCCC.SignatureNonce);
            FilestringBuilder.Append("&BearerToken=" + aliCCC.BearerToken);
            string DownloadAction = "DownloadRecording";
            FilestringBuilder.Append("&Action=" + DownloadAction);
            FilestringBuilder.Append("&InstanceId=" + aliCCC.InstanceId);
            #endregion
            if (aLICCListCallDetailList.Count > 0)
            {
                foreach (var aLICCListCallDetail in aLICCListCallDetailList)
                {
                    FilestringBuilder.Append("&FileName=" + aLICCListCallDetail.FileName);
                    var MediaDownloadParam = await httpHelper.PostResponseAsync(FilestringBuilder.ToString(), "");
                    var MediaDownload = JsonHelper.FromJson<MediaDownload>(MediaDownloadParam);
                    if (MediaDownload != null)
                    {
                        aLICCListCallDetail.SignatureUrl = MediaDownload.MediaDownloadParam.SignatureUrl;
                    }
                }
            }
            return AuthorizationCode.ToString();
            #endregion
        }
    }
}
