using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Yaeher.Common;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher
{
    /// <summary>
    /// 咨询管理
    /// </summary>
    public class ConsultationDocOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="YaeherConsultationInfo"></param>
        public ConsultationDocOut(PagedResultDto<YaeherConsultation> YaeherConsultationDto, ConsultationIn YaeherConsultationInfo)
        {
            Items = YaeherConsultationDto.Items;
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / YaeherConsultationInfo.MaxResultCount;
            SkipCount = YaeherConsultationInfo.SkipCount;
            MaxResultCount = YaeherConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherConsultation> Items { get; set; }
    }


    /// <summary>
    /// 咨询管理
    /// </summary>
    public class ConsultationOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="YaeherConsultationInfo"></param>paralist
        ///  <param name="dict"></param>
        ///  <param name="paralist"></param>
        public ConsultationOut(PagedResultDto<YaeherConsultation> YaeherConsultationDto, ConsultationIn YaeherConsultationInfo, Dictionary<int, Tuple<string>> dict, List<SystemParameter> paralist)
        {
            Items = YaeherConsultationDto.Items.Select(t => new YaeherPatientConsultationPageOut(t, dict, paralist)).ToList();
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / YaeherConsultationInfo.MaxResultCount;
            SkipCount = YaeherConsultationInfo.SkipCount;
            MaxResultCount = YaeherConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 质控处理列表输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="YaeherConsultationInfo"></param>
        ///  <param name="paralist"></param>
        public ConsultationOut(PagedResultDto<QualityConsultationPage> YaeherConsultationDto, ConsultationIn YaeherConsultationInfo, List<SystemParameter> paralist)
        {
            Items = YaeherConsultationDto.Items.Select(t => new YaeherPatientConsultationPageOut(t, paralist)).ToList();
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / YaeherConsultationInfo.MaxResultCount;
            SkipCount = YaeherConsultationInfo.SkipCount;
            MaxResultCount = YaeherConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="CollectConsultationInfo"></param>
        /// <param name="paralist"></param>
        public ConsultationOut(PagedResultDto<YaeherConsultation> YaeherConsultationDto, CollectConsultationIn CollectConsultationInfo, List<SystemParameter> paralist)
        {
            Items = YaeherConsultationDto.Items.Select(t => new YaeherPatientConsultationPageOut(t, paralist)).ToList();
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / CollectConsultationInfo.MaxResultCount;
            SkipCount = CollectConsultationInfo.SkipCount;
            MaxResultCount = CollectConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="YaeherConsultationInfo"></param>
        /// <param name="paralist"></param>
        public ConsultationOut(PagedResultDto<YaeherConsultation> YaeherConsultationDto, ConsultationIn YaeherConsultationInfo, List<SystemParameter> paralist)
        {
            Items = YaeherConsultationDto.Items.Select(t => new YaeherPatientConsultationPageOut(t, paralist)).ToList();
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / YaeherConsultationInfo.MaxResultCount;
            SkipCount = YaeherConsultationInfo.SkipCount;
            MaxResultCount = YaeherConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConsultationDto"></param>
        /// <param name="YaeherConsultationInfo"></param>
        /// <param name="paralist"></param>
        /// <param name="dict"></param>
        public ConsultationOut(PagedResultDto<YaeherConsultation> YaeherConsultationDto, ConsultationIn YaeherConsultationInfo, Dictionary<int, Tuple<DateTime>> dict, List<SystemParameter> paralist)
        {
            Items = YaeherConsultationDto.Items.Select(t => new YaeherPatientConsultationPageOut(t, dict, paralist)).ToList();
            TotalCount = YaeherConsultationDto.TotalCount;
            TotalPage = YaeherConsultationDto.TotalCount / YaeherConsultationInfo.MaxResultCount;
            SkipCount = YaeherConsultationInfo.SkipCount;
            MaxResultCount = YaeherConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IList<YaeherPatientConsultationPageOut> Items { get; set; }
    }
    /// <summary>
    /// 患者端我的咨询
    /// </summary>
    public class YaeherPatientConsultationPageOut : YaeherConsultation
    {
        /// <summary>
        /// 医生头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// utc创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 咨询单状态
        /// </summary>
        public string ConsultStateCode { get; set; }
        /// <summary>
        /// 有无过敏
        /// </summary>
        public bool HasAllergic { get; set; }
        /// <summary>
        /// 过敏史
        /// </summary>
        public string AllergicHistory { get; set; }

        /// <summary>
        ///剩余追问次数
        /// </summary>
        public int ReplysCount { get; set; }
        /// <summary>
        /// 1 男，2 女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 是否质控评分
        /// </summary>
        public bool IsQuality { get; set; }
        /// <summary>
        /// 指控评论理由
        /// </summary>
        public string QualityReason { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 患者评分
        /// </summary>
        public int EvaluateLevel { get; set; }
        /// <summary>
        /// 患者评论理由
        /// </summary>
        public string EvaluateReason { get; set; }
        /// <summary>
        ///患者评论内容
        /// </summary>
        public string EvaluateContent { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string ReplyState { get; set; }
        /// <summary>
        /// 质控退单状态
        /// </summary>
        public string RefundCheckState { get; set; }
        /// <summary>
        /// 1.待处理的订单增加用户已经等待的时间xx小时xx分（以患者用户提交新咨询或者追问的最近一次时间开始计算）：已等待xx小时xx分
        ///2.已处理的订单显示订单结束时间剩余xx小时xx分（已结束的不用显示）：距离结束剩余xx小时xx分
        /// </summary>
        public string CreatedOnFormatter { get; set; }
        /// <summary>
        /// 咨询者图像
        /// </summary>
        public string ConsultantUserImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        ///  <param name="dict"></param>
        ///  <param name="paralist"></param>
        public YaeherPatientConsultationPageOut(YaeherConsultation ConsultationInfo, Dictionary<int, Tuple<string>> dict, List<SystemParameter> paralist)
        {
            Id = ConsultationInfo.Id;
            IsReturnVisit = ConsultationInfo.IsReturnVisit;
            IsEvaluate = ConsultationInfo.IsEvaluate;
            UserImage = dict[ConsultationInfo.Id].Item1;
            ConsultNumber = ConsultationInfo.ConsultNumber;
            ConsultantID = ConsultationInfo.ConsultantID;
            ConsultantName = ConsultationInfo.ConsultantName;
            ConsultantJSON = ConsultationInfo.ConsultantJSON;
            DoctorName = ConsultationInfo.DoctorName;
            DoctorID = ConsultationInfo.DoctorID;
            DoctorJSON = ConsultationInfo.DoctorJSON;
            PatientID = ConsultationInfo.PatientID;
            PatientName = ConsultationInfo.PatientName;
            PatientJSON = ConsultationInfo.PatientJSON;
            ConsultType = ConsultationInfo.ConsultType;
            IIInessType = ConsultationInfo.IIInessType;
            IIInessJSON = ConsultationInfo.IIInessJSON;
            PhoneNumber = ConsultationInfo.PhoneNumber;
            PatientCity = ConsultationInfo.PatientCity;
            IIInessDescription = ConsultationInfo.IIInessDescription;
            InquiryTimes = ConsultationInfo.InquiryTimes;
            ConsultState = ConsultationInfo.ConsultState;
            ConsultState = paralist.Find(t => t.Code == ConsultationInfo.ConsultState).Name;
            ConsultStateCode = ConsultationInfo.ConsultState;
            OvertimeRemindTimes = ConsultationInfo.OvertimeRemindTimes;
            Overtime = ConsultationInfo.Overtime;
            RefundBy = ConsultationInfo.RefundBy;
            RefundTime = ConsultationInfo.RefundTime;
            RefundType = ConsultationInfo.RefundType;
            RefundNumber = ConsultationInfo.RefundNumber;
            RefundState = ConsultationInfo.RefundState;
            RefundReason = ConsultationInfo.RefundReason;
            RefundRemarks = ConsultationInfo.RefundRemarks;
            RecommendDoctorID = ConsultationInfo.RecommendDoctorID;
            RecommendDoctorName = ConsultationInfo.RecommendDoctorName;
            CreatedBy = ConsultationInfo.CreatedBy;
            CreatedOn = ConsultationInfo.CreatedOn;
            ModifyBy = ConsultationInfo.ModifyBy;
            ModifyOn = ConsultationInfo.ModifyOn;
            Id = ConsultationInfo.Id;
            Completetime = ConsultationInfo.Completetime;
            ReplysCount = ConsultationInfo.HasInquiryTimes;//已放弃
            HasInquiryTimes = ConsultationInfo.HasInquiryTimes;//剩余追问次数移动端用，pc用
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).Sex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        ///  <param name="paralist"></param>
        public YaeherPatientConsultationPageOut(QualityConsultationPage ConsultationInfo, List<SystemParameter> paralist)
        {
            IsReturnVisit = ConsultationInfo.IsReturnVisit;
            IsEvaluate = ConsultationInfo.IsEvaluate;
            UserImage = ConsultationInfo.UserImage;
            ConsultNumber = ConsultationInfo.ConsultNumber;
            ConsultantID = ConsultationInfo.ConsultantID;
            ConsultantName = ConsultationInfo.ConsultantName;
            ConsultantJSON = "";
            DoctorName = ConsultationInfo.DoctorName;
            DoctorID = ConsultationInfo.DoctorID;
            DoctorJSON = "";
            PatientID = ConsultationInfo.PatientID;
            PatientName = ConsultationInfo.PatientName;
            PatientJSON = "";
            ConsultType = ConsultationInfo.ConsultType;
            IIInessType = ConsultationInfo.IIInessType;
            IIInessJSON = "";
            PhoneNumber = ConsultationInfo.PhoneNumber;
            PatientCity = ConsultationInfo.PatientCity;
            IIInessDescription = ConsultationInfo.IIInessDescription;
            InquiryTimes = ConsultationInfo.InquiryTimes;
            ConsultState = ConsultationInfo.ConsultState;
            ConsultState = paralist.Find(t => t.Code == ConsultationInfo.ConsultState).Name;
            ConsultStateCode = ConsultationInfo.ConsultState;
            OvertimeRemindTimes = ConsultationInfo.OvertimeRemindTimes;
            Overtime = ConsultationInfo.Overtime;
            RefundBy = ConsultationInfo.RefundBy;
            RefundTime = ConsultationInfo.RefundTime;
            RefundType = ConsultationInfo.RefundType;
            RefundNumber = ConsultationInfo.RefundNumber;
            RefundCheckState = ConsultationInfo.RefundCheckState;
            RefundState = ConsultationInfo.RefundState;
            RefundReason = ConsultationInfo.RefundReason;
            RefundRemarks = ConsultationInfo.RefundRemarks;
            RecommendDoctorID = ConsultationInfo.RecommendDoctorID;
            RecommendDoctorName = ConsultationInfo.RecommendDoctorName;
            CreatedBy = ConsultationInfo.CreatedBy;
            CreatedOn = ConsultationInfo.CreatedOn;
            ModifyBy = ConsultationInfo.ModifyBy;
            ModifyOn = ConsultationInfo.ModifyOn;
            Id = ConsultationInfo.Id;
            Completetime = ConsultationInfo.Completetime;
            ReplysCount = ConsultationInfo.HasInquiryTimes;//已放弃
            HasInquiryTimes = ConsultationInfo.HasInquiryTimes;//剩余追问次数移动端用，pc用
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).Sex;
            QualityReason = ConsultationInfo.QualityReason;
            QualityLevel = ConsultationInfo.QualityLevel;
            EvaluateLevel = ConsultationInfo.EvaluateLevel;
            EvaluateReason = ConsultationInfo.EvaluateReason;
            EvaluateContent = ConsultationInfo.EvaluateContent;
            IsQuality = ConsultationInfo.IsQuality;
            ReplyState = ConsultationInfo.ReplyState;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        ///  <param name="paralist"></param>
        public YaeherPatientConsultationPageOut(YaeherConsultation ConsultationInfo, List<SystemParameter> paralist)
        {
            Id = ConsultationInfo.Id;
            IsReturnVisit = ConsultationInfo.IsReturnVisit;
            IsEvaluate = ConsultationInfo.IsEvaluate;
            ConsultNumber = ConsultationInfo.ConsultNumber;
            ConsultantID = ConsultationInfo.ConsultantID;
            ConsultantName = ConsultationInfo.ConsultantName;
            ConsultantJSON = ConsultationInfo.ConsultantJSON;
            DoctorName = ConsultationInfo.DoctorName;
            DoctorID = ConsultationInfo.DoctorID;
            DoctorJSON = ConsultationInfo.DoctorJSON;
            PatientID = ConsultationInfo.PatientID;
            PatientName = ConsultationInfo.PatientName;
            PatientJSON = ConsultationInfo.PatientJSON;
            ConsultType = ConsultationInfo.ConsultType;
            IIInessType = ConsultationInfo.IIInessType;
            IIInessJSON = ConsultationInfo.IIInessJSON;
            PhoneNumber = ConsultationInfo.PhoneNumber;
            PatientCity = ConsultationInfo.PatientCity;
            IIInessDescription = ConsultationInfo.IIInessDescription;
            InquiryTimes = ConsultationInfo.InquiryTimes;
            //ConsultState = ConsultationInfo.ConsultState;
            ConsultState = paralist.Find(t => t.Code == ConsultationInfo.ConsultState).Name;
            ConsultStateCode = ConsultationInfo.ConsultState;
            OvertimeRemindTimes = ConsultationInfo.OvertimeRemindTimes;
            Overtime = ConsultationInfo.Overtime;
            RefundBy = ConsultationInfo.RefundBy;
            RefundTime = ConsultationInfo.RefundTime;
            RefundType = ConsultationInfo.RefundType;
            RefundNumber = ConsultationInfo.RefundNumber;
            RefundState = ConsultationInfo.RefundState;
            RefundReason = ConsultationInfo.RefundReason;
            RefundRemarks = ConsultationInfo.RefundRemarks;
            RecommendDoctorID = ConsultationInfo.RecommendDoctorID;
            RecommendDoctorName = ConsultationInfo.RecommendDoctorName;
            CreatedBy = ConsultationInfo.CreatedBy;
            CreatedOn = ConsultationInfo.CreatedOn;
            ModifyBy = ConsultationInfo.ModifyBy;
            ModifyOn = ConsultationInfo.ModifyOn;
            Id = ConsultationInfo.Id;
            Completetime = ConsultationInfo.Completetime;
            ReplysCount = ConsultationInfo.HasInquiryTimes;//已放弃
            HasInquiryTimes = ConsultationInfo.HasInquiryTimes;//剩余追问次数移动端用，pc用
            if (ConsultationInfo.ConsultState == "return" || ConsultationInfo.ConsultState == "success")
            {
                HasInquiryTimes = 0;
            }

            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).Sex;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).AllergicHistory;
            CreatedOn = ConsultationInfo.CreatedOn;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="ConsultationInfo"></param>
        ///  <param name="paralist"></param>
        public YaeherPatientConsultationPageOut(YaeherConsultation ConsultationInfo, Dictionary<int, Tuple<DateTime>> dict, List<SystemParameter> paralist)
        {
            Id = ConsultationInfo.Id;
            IsReturnVisit = ConsultationInfo.IsReturnVisit;
            IsEvaluate = ConsultationInfo.IsEvaluate;
            ConsultNumber = ConsultationInfo.ConsultNumber;
            ConsultantID = ConsultationInfo.ConsultantID;
            ConsultantName = ConsultationInfo.ConsultantName;
            ConsultantJSON = ConsultationInfo.ConsultantJSON;
            DoctorName = ConsultationInfo.DoctorName;
            DoctorID = ConsultationInfo.DoctorID;
            DoctorJSON = ConsultationInfo.DoctorJSON;
            UserImage = JsonHelper.FromJson<YaeherDoctor>(DoctorJSON).UserImageFile; // 增加医生图像
            ConsultantUserImage=JsonHelper.FromJson<YaeherUser>(ConsultantJSON).UserImage; // 咨询者头像
            PatientID = ConsultationInfo.PatientID;
            PatientName = ConsultationInfo.PatientName;
            PatientJSON = ConsultationInfo.PatientJSON;
            ConsultType = ConsultationInfo.ConsultType;
            IIInessType = ConsultationInfo.IIInessType;
            IIInessJSON = ConsultationInfo.IIInessJSON;
            PhoneNumber = ConsultationInfo.PhoneNumber;
            PatientCity = ConsultationInfo.PatientCity;
            IIInessDescription = ConsultationInfo.IIInessDescription;
            InquiryTimes = ConsultationInfo.InquiryTimes;
            //ConsultState = ConsultationInfo.ConsultState;
            ConsultState = paralist.Find(t => t.Code == ConsultationInfo.ConsultState).Name;
            ConsultStateCode = ConsultationInfo.ConsultState;
            OvertimeRemindTimes = ConsultationInfo.OvertimeRemindTimes;
            Overtime = ConsultationInfo.Overtime;
            RefundBy = ConsultationInfo.RefundBy;
            RefundTime = ConsultationInfo.RefundTime;
            RefundType = ConsultationInfo.RefundType;
            RefundNumber = ConsultationInfo.RefundNumber;
            RefundState = ConsultationInfo.RefundState;
            RefundReason = ConsultationInfo.RefundReason;
            RefundRemarks = ConsultationInfo.RefundRemarks;
            RecommendDoctorID = ConsultationInfo.RecommendDoctorID;
            RecommendDoctorName = ConsultationInfo.RecommendDoctorName;
            CreatedBy = ConsultationInfo.CreatedBy;
            CreatedOn = ConsultationInfo.CreatedOn;
            ModifyBy = ConsultationInfo.ModifyBy;
            ModifyOn = ConsultationInfo.ModifyOn;
            Id = ConsultationInfo.Id;
            ReplysCount = ConsultationInfo.HasInquiryTimes;//已放弃
            HasInquiryTimes = ConsultationInfo.HasInquiryTimes;//剩余追问次数移动端用，pc用
            Completetime = ConsultationInfo.Completetime;

            if (ConsultationInfo.ConsultState == "created")
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(dict[ConsultationInfo.Id].Item1);
                if (timeSpan.TotalMinutes < 0)
                { CreatedOnFormatter = ""; }
                else
                { CreatedOnFormatter = timeSpan.Days > 0 ? (timeSpan.Days * 24 + timeSpan.Hours + "小时" + timeSpan.Minutes + "分") : timeSpan.Hours + "小时" + timeSpan.Minutes + "分"; }
            }
            else if (ConsultationInfo.ConsultState == "consulting")
            {
                if (ConsultationInfo.HasReply == false)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(dict[ConsultationInfo.Id].Item1);
                    if (timeSpan.TotalMinutes < 0)
                    { CreatedOnFormatter = ""; }
                    else { CreatedOnFormatter = timeSpan.Days > 0 ? (timeSpan.Days * 24 + timeSpan.Hours + "小时" + timeSpan.Minutes + "分") : timeSpan.Hours + "小时" + timeSpan.Minutes + "分"; }

                }
                else
                {
                    TimeSpan timeSpan = dict[ConsultationInfo.Id].Item1.Subtract(DateTime.Now);
                    if (timeSpan.TotalMinutes < 0)
                    { CreatedOnFormatter = ""; }
                    else { CreatedOnFormatter = timeSpan.Days > 0 ? (timeSpan.Days * 24 + timeSpan.Hours + "小时" + timeSpan.Minutes + "分") : timeSpan.Hours + "小时" + timeSpan.Minutes + "分"; }

                }
                //  TimeSpan timeSpan = dict[ConsultationInfo.Id].Item1.Subtract(DateTime.Now);
                //  CreatedOnFormatter = timeSpan.Days > 0 ? (timeSpan.Days * 24 + timeSpan.Hours + "小时" + timeSpan.Minutes + "分") : timeSpan.Hours + "小时" + timeSpan.Minutes + "分";

            }
            else if (ConsultationInfo.ConsultState == "success")
            {
                if (ConsultationInfo.HasReply == false)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(dict[ConsultationInfo.Id].Item1);
                    if (timeSpan.TotalMinutes < 0)
                    { CreatedOnFormatter = ""; }
                    else { CreatedOnFormatter = timeSpan.Days > 0 ? (timeSpan.Days * 24 + timeSpan.Hours + "小时" + timeSpan.Minutes + "分") : timeSpan.Hours + "小时" + timeSpan.Minutes + "分"; }

                }
                else
                {
                    HasInquiryTimes = 0;
                    CreatedOnFormatter = "";
                }
            }
            else if (ConsultationInfo.ConsultState == "return" )
            {
                HasInquiryTimes = 0;
                CreatedOnFormatter = "";
            }

            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).Sex;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(ConsultationInfo.PatientJSON).AllergicHistory;
            CreatedOn = ConsultationInfo.CreatedOn;

        }
    }

    /// <summary>
    /// 患者端咨询明细
    /// </summary>
    public class ConsultationDetailOut
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="orderTradeRecord"></param>
        /// <param name="RecommendDoctor"></param>
        /// <param name="consultationfile"></param>
        /// <param name="reply"></param>
        /// <param name="doctor"></param>
        /// <param name="eva"></param>
        /// <param name="serverid"></param>
        /// <param name="IIIness"></param>
        /// <param name="paramlist"></param>
        /// <param name="answerparamresult"></param>
        /// <param name="ReplyMinutesparamResult"></param>
        public ConsultationDetailOut(YaeherConsultation item, OrderTradeRecord orderTradeRecord, YaeherDoctorUser RecommendDoctor, IList<ReplyDetail> consultationfile, IList<ReplyDetail> reply, YaeherUser doctor, IList<ConsultationEvaluation> eva, int serverid, int IIIness, List<SystemParameter> paramlist, List<SystemParameter> answerparamresult, SystemParameter ReplyMinutesparamResult)
        {
            Id = item.Id;
            CreatedBy = item.CreatedBy;
            CreatedOn = item.CreatedOn;
            UserImage = doctor.UserImage;
            ConsultNumber = item.ConsultNumber;
            ConsultantID = item.ConsultantID;
            ConsultantName = item.ConsultantName;
            ConsultantJSON = item.ConsultantJSON;
            DoctorName = item.DoctorName;
            DoctorPhoneNumber = JsonHelper.FromJson<YaeherDoctor>(item.DoctorJSON).PhoneNumber;
            DoctorID = item.DoctorID;
            DoctorJSON = item.DoctorJSON;
            PatientID = item.PatientID;
            PatientName = item.PatientName;
            PatientJSON = item.PatientJSON;
            ConsultType = item.ConsultType;
            IsReturnVisit = item.IsReturnVisit;
            IIInessType = item.IIInessType;
            IIInessId = IIIness;
            IIInessJSON = item.IIInessJSON;
            PhoneNumber = item.PhoneNumber;
            PatientCity = item.PatientCity;
            IIInessDescription = item.IIInessDescription;
            InquiryTimes = item.InquiryTimes;
            ConsultState = item.ConsultState;
            OvertimeRemindTimes = item.OvertimeRemindTimes;
            Overtime = item.Overtime;
            RefundBy = item.RefundBy;
            RefundTime = item.RefundTime;
            RefundType = item.RefundType;
            RefundNumber = item.RefundNumber;
            RefundState = item.RefundState;
            RefundReason = item.RefundReason;
            RefundRemarks = item.RefundRemarks;
            RecommendDoctorName = item.RecommendDoctorName;
            RecommendDoctorID = item.RecommendDoctorID;
            RecommendDoctorImage = RecommendDoctor.UserImage;
            Age = item.Age;
            Replys = reply.OrderBy(t => t.CreatedOn).ToList();
            Consultationfile = consultationfile.ToList();
            TimeSpan ts = item.Completetime.Subtract(DateTime.Now);
            if (ts.TotalDays > 0)
            {
                InquiryTimesMsg = ts.Days + "天";
            }
            InquiryTimesMsg += ts.TotalHours > 0 ? ts.Hours + "时" + ts.Minutes + "分" : ts.Minutes + "分";
            Canhargeback = false;
            if (orderTradeRecord != null)
            {
                DateTime CanhargebackTime = orderTradeRecord.CreatedOn.AddDays(double.Parse(ReplyMinutesparamResult.ItemValue.ToString() == "" ? "0" : ReplyMinutesparamResult.ItemValue.ToString()));

                if ((DateTime.Now >= CanhargebackTime) && Replys.Count <= 0 && item.ConsultState == "created")//能退单时间，从数据库拿
                {
                    Canhargeback = true;
                }
            }
            var replycount = reply.Where(t => t.ReplyType == "inquiries").ToList();
            // ReplysCount = int.Parse(answerparamresult[0].ItemValue) - replycount.Count > 0 ? int.Parse(answerparamresult[0].ItemValue) - replycount.Count : 0;
            ReplysCount = item.HasInquiryTimes;//已放弃
            HasInquiryTimes = item.HasInquiryTimes;//剩余追问次数移动端用，pc用
            ConsulationStatusCode = item.ConsultState;
            ConsultState = paramlist.Find(t => t.Code == item.ConsultState).Name;
            if (item.ConsultState == "consulting") { CanReplys = true; } else { InquiryTimesMsg = ""; CanReplys = false; }
            if (item.ConsultState == "unpaid" || item.ConsultState == "success" || item.ConsultState == "return") { HasInquiryTimes = 0; CanDelete = true; } else { CanDelete = false; }
            if (item.ConsultState == "success" && !item.IsEvaluate)
            { CanEvaluation = true; }
            else
            { CanEvaluation = false; }
            if (HasInquiryTimes < 1||(HasInquiryTimes!=item.InquiryTimes&&item.HasReply==false)) { CanReplys = false; }
            HasEvaluation = item.IsEvaluate;
            ReturnVisit = item.ReturnVisit;
            EvaluationId = eva.Count > 0 ? eva[0].Id : 0;
            EvaluateLevel = eva.Count > 0 ? eva[0].EvaluateLevel : 0;
            IsEvaluate = item.IsEvaluate;
            ServiceMoneyListID = serverid;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).AllergicHistory;
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).Sex;
        }

        /// <summary>
        /// 医生端咨询详情输出模型
        /// </summary>
        /// <param name="item"></param>
        /// <param name="RecommendDoctoruser"></param>
        /// <param name="qualityControlManages"></param>
        ///  <param name="refund"></param>
        /// <param name="hasCollect"></param>
        /// <param name="consultationfile"></param>
        /// <param name="reply"></param>
        /// <param name="doctor"></param>
        /// <param name="eva"></param>
        /// <param name="serverid"></param>
        /// <param name="IIIness"></param>
        /// <param name="paramlist"></param>
        ///<param name="answerparamresult"></param>
        public ConsultationDetailOut(YaeherConsultation item, YaeherDoctorUser RecommendDoctoruser, IList<QualityControlManage> qualityControlManages, RefundManage refund, bool hasCollect, IList<ReplyDetail> consultationfile, IList<ReplyDetail> reply, YaeherUser doctor, IList<ConsultationEvaluation> eva, int serverid, int IIIness, List<SystemParameter> paramlist, List<SystemParameter> answerparamresult)
        {
            qualityControlManage = qualityControlManages;
            IsReturnVisit = item.IsReturnVisit;
            HasCollect = hasCollect;
            Id = item.Id;
            CreatedBy = item.CreatedBy;
            CreatedOn = item.CreatedOn;
            UserImage = doctor.UserImage;
            ConsultNumber = item.ConsultNumber;
            ConsultantID = item.ConsultantID;
            ConsultantName = item.ConsultantName;
            ConsultantJSON = item.ConsultantJSON;
            DoctorName = item.DoctorName;
            DoctorID = item.DoctorID;
            DoctorJSON = item.DoctorJSON;
            PatientID = item.PatientID;
            PatientName = item.PatientName;
            PatientJSON = item.PatientJSON;
            ConsultType = item.ConsultType;
            IIInessType = item.IIInessType;
            IIInessId = IIIness;
            IIInessJSON = item.IIInessJSON;
            DoctorPhoneNumber = JsonHelper.FromJson<YaeherDoctor>(item.DoctorJSON).PhoneNumber;
            PhoneNumber = item.PhoneNumber;
            PatientCity = item.PatientCity;
            IIInessDescription = item.IIInessDescription;
            InquiryTimes = item.InquiryTimes;
            ConsultState = item.ConsultState;
            OvertimeRemindTimes = item.OvertimeRemindTimes;
            Overtime = item.Overtime;
            RefundBy = item.RefundBy;
            RefundTime = item.RefundTime;
            RefundType = item.RefundType;
            RefundNumber = item.RefundNumber;
            RefundCheckState = refund == null ? "" : refund.CheckState;
            RefundState = item.RefundState;
            RefundReason = refund == null ? "" : refund.RefundReason;
            RefundRemarks = refund == null ? "" : refund.RefundRemarks;
            RecommendDoctorName = item.RecommendDoctorName;
            RecommendDoctorID = item.RecommendDoctorID;
            RecommendDoctorImage = RecommendDoctoruser.UserImage;
            Age = item.Age;
            Replys = reply.OrderBy(t => t.CreatedOn).ToList();
            Consultationfile = consultationfile.ToList();
            TimeSpan ts = DateTime.UtcNow.Subtract(item.CreatedOn);
            if (ts.TotalMinutes >= 3 || Replys.Count > 0)
            {
                Canhargeback = false;
            }
            else
            {
                Canhargeback = true;
            }
            var replycount = reply.Where(t => t.ReplyType == "inquiries").ToList();
            ReplysCount = item.HasInquiryTimes;//已放弃
            HasInquiryTimes = item.HasInquiryTimes;//剩余追问次数移动端用，pc用
            ConsulationStatusCode = item.ConsultState;
            ConsultState = paramlist.Find(t => t.Code == item.ConsultState).Name;
            if (item.ConsultState == "consulting") { CanReplys = true; } else { CanReplys = false; }
            if (item.ConsultState == "success" && !HasEvaluation)
            { CanEvaluation = true; }
            else
            { CanEvaluation = false; }
            if (item.ConsultState == "success" || item.ConsultState == "return") { CanDelete = true; } else { CanDelete = false; }
            if (HasInquiryTimes < 1) { CanReplys = false; }
            HasEvaluation = item.IsEvaluate;
            ReturnVisit = item.ReturnVisit;
            EvaluationId = eva.Count > 0 ? eva[0].Id : 0;//评分id
            QualityLevel = eva.Count > 0 ? eva[0].QualityLevel : 0;
            IsQuality = eva.Count > 0 ? eva[0].IsQuality : false;
            QualityReason = eva.Count > 0 ? eva[0].QualityReason : "";
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).Sex;
            ServiceMoneyListID = serverid;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).AllergicHistory;
        }
        /// <summary>
        /// 医生端咨询详情输出模型
        /// </summary>
        /// <param name="item"></param>
        /// <param name="RecommendDoctoruser"></param>
        /// <param name="refund"></param>
        /// <param name="hasCollect"></param>
        /// <param name="consultationfile"></param>
        /// <param name="reply"></param>
        /// <param name="doctor"></param>
        /// <param name="eva"></param>
        /// <param name="serverid"></param>
        /// <param name="IIIness"></param>
        /// <param name="canhargeback"></param>
        /// <param name="paramlist"></param>
        /// <param name="answerparamresult"></param>
        public ConsultationDetailOut(YaeherConsultation item, YaeherDoctorUser RecommendDoctoruser, RefundManage refund, bool hasCollect, IList<ReplyDetail> consultationfile, IList<ReplyDetail> reply, YaeherUser doctor, IList<ConsultationEvaluation> eva, int serverid, int IIIness, bool canhargeback, List<SystemParameter> paramlist, List<SystemParameter> answerparamresult)
        {
            Id = item.Id;
            IsReturnVisit = item.IsReturnVisit;
            RefundCheckState = refund == null ? "" : refund.CheckState;
            RefundState = item.RefundState;
            RefundReason = refund == null ? "" : item.RefundReason;
            RefundRemarks = refund == null ? "" : item.RefundRemarks;
            HasCollect = hasCollect;
            CreatedBy = item.CreatedBy;
            CreatedOn = item.CreatedOn;
            UserImage = doctor.UserImage;
            ConsultNumber = item.ConsultNumber;
            ConsultantID = item.ConsultantID;
            ConsultantName = item.ConsultantName;
            ConsultantJSON = item.ConsultantJSON;
            DoctorName = item.DoctorName;
            DoctorPhoneNumber = JsonHelper.FromJson<YaeherDoctor>(item.DoctorJSON).PhoneNumber;
            DoctorID = item.DoctorID;
            DoctorJSON = item.DoctorJSON;
            PatientID = item.PatientID;
            PatientName = item.PatientName;
            PatientJSON = item.PatientJSON;
            ConsultType = item.ConsultType;
            IIInessType = item.IIInessType;
            IIInessId = IIIness;
            IIInessJSON = item.IIInessJSON;
            PhoneNumber = item.PhoneNumber;
            PatientCity = item.PatientCity;
            IIInessDescription = item.IIInessDescription;
            InquiryTimes = item.InquiryTimes;
            ConsultState = item.ConsultState;
            OvertimeRemindTimes = item.OvertimeRemindTimes;
            Overtime = item.Overtime;
            RefundBy = item.RefundBy;
            RefundTime = item.RefundTime;
            RefundType = item.RefundType;
            RefundNumber = item.RefundNumber;
            RecommendDoctorName = item.RecommendDoctorName;
            RecommendDoctorID = item.RecommendDoctorID;
            RecommendDoctorImage = RecommendDoctoruser.UserImage;
            Age = item.Age;
            Replys = reply.OrderBy(t => t.CreatedOn).ToList();
            Consultationfile = consultationfile.ToList();
            //TimeSpan ts = DateTime.UtcNow.Subtract(item.CreatedOn);
            //if (ts.TotalMinutes >= 3 || Replys.Count > 0)
            //{
            //医生端是否可以退单从前面判断中来
            Canhargeback = canhargeback;
            //}
            //else
            //{
            //    Canhargeback = true;
            //}
            var replycount = reply.Where(t => t.ReplyType == "inquiries").ToList();
            ReplysCount = item.HasInquiryTimes;//已放弃
            HasInquiryTimes = item.HasInquiryTimes;//剩余追问次数移动端用，pc用
            ConsulationStatusCode = item.ConsultState;
            ConsultState = paramlist.Find(t => t.Code == item.ConsultState).Name;
            if (item.ConsultState == "consulting") { CanReplys = true; } else { CanReplys = false; }
            HasEvaluation = item.IsEvaluate;
            if (item.ConsultState == "success" && !HasEvaluation)
            { CanEvaluation = true; }
            else
            { CanEvaluation = false; }
            if (item.ConsultState == "success" || item.ConsultState == "return") { HasInquiryTimes = 0; CanDelete = true; } else { CanDelete = false; }
            if (HasInquiryTimes < 1) { CanReplys = false; }
            ReturnVisit = item.ReturnVisit;
            EvaluationId = eva.Count > 0 ? eva[0].Id : 0;//评分id
            QualityLevel = eva.Count > 0 ? eva[0].QualityLevel : 0;
            IsQuality = eva.Count > 0 ? eva[0].IsQuality : false;
            QualityReason = eva.Count > 0 ? eva[0].QualityReason : "";
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).Sex;
            ServiceMoneyListID = serverid;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).AllergicHistory;
        }
        /// <summary>
        /// 指控委员回复
        /// </summary>
        public IList<QualityControlManage> qualityControlManage { get; set; }
        /// <summary>
        /// 质控查看咨询单的退单表状态
        /// </summary>
        public string RefundCheckState { get; set; }
        /// <summary>
        /// 1男2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 患者评分
        /// </summary>
        public int EvaluateLevel { get; set; }
        /// <summary>
        /// 是否质控评分
        /// </summary>
        public bool IsQuality { get; set; }
        /// <summary>
        /// 质控原因
        /// </summary>
        public string QualityReason { get; set; }
        /// <summary>
        /// 能否删除
        /// </summary>
        public bool CanDelete { get; set; }
        /// <summary>
        /// 是否可以评分
        /// </summary>
        public bool CanEvaluation { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string ReturnVisit { get; set; }
        /// <summary>
        /// 是否已回访
        /// </summary>
        public bool IsReturnVisit { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool HasCollect { get; set; }
        /// <summary>
        /// 有无过敏
        /// </summary>
        public bool HasAllergic { get; set; }
        /// <summary>
        /// 过敏史
        /// </summary>
        public string AllergicHistory { get; set; }
        /// <summary>
        /// 是否已评分
        /// </summary>
        public bool IsEvaluate { get; set; }
        /// <summary>
        /// 评分Id
        /// </summary>
        public int EvaluationId { get; set; }
        /// <summary>
        /// 医生服务Id
        /// </summary>
        public int ServiceMoneyListID { get; set; }
        /// <summary>
        /// 咨询状态  已创建(支付完成之后)created,正在咨询consulting, 已退单 return，已完成 success,已删除 deleted
        /// </summary>
        public string ConsulationStatusCode { get; set; }
        /// <summary>
        /// 是否已评分
        /// </summary>
        public bool HasEvaluation { get; set; }
        /// <summary>
        /// 可追问次数(已放弃使用)
        /// </summary>
        public int ReplysCount { get; set; }
        /// <summary>
        /// 可追问次数
        /// </summary>
        public int HasInquiryTimes { get; set; }
        /// <summary>
        /// 追问消息 如有需要请在XX时XX分内追问
        /// </summary>
        public string InquiryTimesMsg { get; set; }
        /// <summary>
        /// 是否可追问
        /// </summary>
        public bool CanReplys { get; set; }
        /// <summary>
        /// 是可以退单
        /// </summary>
        public bool Canhargeback { get; set; }
        /// <summary>
        /// 医生头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(20)]
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询姓名
        /// YaeherUser表fullname
        /// </summary>
        [MaxLength(20)]
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 咨询医生JSON
        /// </summary>
        public string DoctorJSON { get; set; }
        /// <summary>
        /// 患者ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [MaxLength(20)]
        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }
        /// <summary>
        /// 咨询类型
        /// 图文 电话 或者其他
        /// </summary>
        [MaxLength(20)]
        public string ConsultType { get; set; }
        /// <summary>
        /// 疾病类型
        /// </summary>
        [MaxLength(20)]
        public string IIInessType { get; set; }
        /// <summary>
        /// 疾病Id （医生标签Id）
        /// </summary>
        public int IIInessId { get; set; }
        /// <summary>
        /// 疾病类型JSON
        /// </summary>
        public string IIInessJSON { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 医生电话
        /// </summary>
        public string DoctorPhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [MaxLength(200)]
        public string PatientCity { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string IIInessDescription { get; set; }
        /// <summary>
        /// 最大追问次数
        /// </summary>
        public int InquiryTimes { get; set; }
        /// <summary>
        /// 咨询问诊状态
        /// </summary>
        [MaxLength(20)]
        public string ConsultState { get; set; }
        /// <summary>
        /// 咨询超时提醒次数
        /// </summary>
        public int OvertimeRemindTimes { get; set; }
        /// <summary>
        /// 咨询退单超时时间
        /// </summary>
        public DateTime Overtime { get; set; }
        /// <summary>
        /// 退单人
        /// </summary>
        public int RefundBy { get; set; }
        /// <summary>
        /// 退单时间
        /// </summary>
        public DateTime RefundTime { get; set; }
        /// <summary>
        /// 退单类型
        /// 来源 患者 医生 系统
        /// </summary>
        [MaxLength(10)]
        public string RefundType { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        [MaxLength(20)]
        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>
        [MaxLength(20)]
        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>
        [MaxLength(1000)]
        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因
        /// </summary>
        [MaxLength(1000)]
        public string RefundRemarks { get; set; }
        /// <summary>
        /// 推荐医生
        /// </summary>
        public int RecommendDoctorID { get; set; }
        /// <summary>
        /// 推荐医生姓名
        /// YaeherUser表name  医生name
        /// </summary>
        public string RecommendDoctorName { get; set; }
        /// <summary>
        /// 推荐医生头像
        /// </summary>
        public string RecommendDoctorImage { get; set; }
        /// <summary>
        /// 回答追问详情
        /// </summary>
        public List<ReplyDetail> Replys { get; set; }
        /// <summary>
        /// 咨询附件
        /// </summary>
        public List<ReplyDetail> Consultationfile { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 创建人userid
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 咨询单ID
        /// </summary>
        public int Id { get; set; }

    }
    /// <summary>
    /// ReplyDetail
    /// </summary>
    public class ReplyDetail
    {
        /// <summary>
        /// MP3文件地址
        /// </summary>
        public string FileContentAddress { get; set; }
        /// <summary>
        /// MP3文件地址
        /// </summary>
        public double FileTotalTime { get; set; }
        /// <summary>
        /// 咨询回答Id
        /// </summary>
        public int ReplyId { get; set; }
        /// <summary>
        /// 咨询回答num
        /// </summary>
        public string ReplyNumber { get; set; }
        /// <summary>
        /// 咨询单num
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询Id
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 媒体类型 图片，视频
        /// </summary>
        public string Mediatype { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 文字内容或者图片地址或者音频地址
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 附件list结果集
        /// </summary>
        public List<ConsultationFile> ConsultationFile { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 回答类型  回答 追问
        /// </summary>
        public string ReplyType { get; set; }
        /// <summary>
        /// 咨询类型  图文 电话 或者其他
        /// </summary>
        public string ConsultType { get; set; }
        /// <summary>
        /// 回复类型 Message  Phone
        /// </summary>
        public string AnswerType { get; set; }
        /// <summary>
        /// 配合附件结果集
        /// </summary>
        public ReplyDetail(ReplyDetail a, List<ReplyDetail> b)
        {
            ReplyId = a.ReplyId;
            Message = a.Message;
            CreatedOn = a.CreatedOn;
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 回复信息
        /// </summary>
        /// <param name="a"></param>
        public ReplyDetail()
        {
        }
    }
    /// <summary>
    /// 文件
    /// </summary>
    public class ConsultationFile
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 缩略地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string FileContentUrl { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 音频文件时长
        /// </summary>
        public double FileTotalTime { get; set; }
    }
    /// <summary>
    /// 指控处理咨询单列表
    /// </summary>
    public class QualityConsultationPage : YaeherConsultation
    {
        /// <summary>
        /// 质控退单状态
        /// </summary>
        public string RefundCheckState { get; set; }
        /// <summary>
        /// 是否质控评分
        /// </summary>
        public bool IsQuality { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string ReplyState { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 指控评论理由
        /// </summary>
        public string QualityReason { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 患者评分
        /// </summary>
        public int EvaluateLevel { get; set; }
        /// <summary>
        /// 患者评论理由
        /// </summary>
        public string EvaluateReason { get; set; }
        /// <summary>
        ///患者评论内容
        /// </summary>
        public string EvaluateContent { get; set; }
    }
    /// <summary>
    /// 新医生列表
    /// </summary>
    public class DoctorNew
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        public int DoctorId { get; set; }
        /// <summary>
        /// 完成单数
        /// </summary>
        public int ConsultationCount { get; set; }
    }
    /// <summary>
    /// 字段
    /// </summary>
    public class DoctorNewIn : ListParameters<DoctorNew>, IPagedResultRequest
    {

    }
}
