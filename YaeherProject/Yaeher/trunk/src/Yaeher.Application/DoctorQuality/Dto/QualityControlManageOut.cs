using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Quality.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.DoctorQuality.Dto
{
    /// <summary>
    /// 处理质控
    /// </summary>
    public class QualityControlManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QualityControlManageDto"></param>
        /// <param name="QualityControlManageInfo"></param>
        public QualityControlManageOut(PagedResultDto<QualityControlManage> QualityControlManageDto, QualityControlManageIn QualityControlManageInfo)
        {
            Items = QualityControlManageDto.Items;
            TotalCount = QualityControlManageDto.TotalCount;
            TotalPage = QualityControlManageDto.TotalCount / QualityControlManageInfo.MaxResultCount;
            SkipCount = QualityControlManageInfo.SkipCount;
            MaxResultCount = QualityControlManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QualityControlManage> Items { get; set; }
    }
    /// <summary>
    /// 处理质控
    /// </summary>
    public class QualityControlManagePageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QualityControlManageDto"></param>
        /// <param name="QualityControlManageInfo"></param>
        public QualityControlManagePageOut(PagedResultDto<QualityControlManagePage> QualityControlManageDto, QualityControlManageIn QualityControlManageInfo)
        {
            Items = QualityControlManageDto.Items.Select(t=>new QualityControlManagePageOutDetail(t)).ToList();
            TotalCount = QualityControlManageDto.TotalCount;
            TotalPage = QualityControlManageDto.TotalCount / QualityControlManageInfo.MaxResultCount;
            SkipCount = QualityControlManageInfo.SkipCount;
            MaxResultCount = QualityControlManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QualityControlManagePageOutDetail> Items { get; set; }
    }
    public class QualityControlManagePageOutDetail : QualityControlManage
    {
        public QualityControlManagePageOutDetail(QualityControlManagePage a)
        {
            Id = a.Id;
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(a.PatientJSON).Sex;
            ConsultantJSON = a.ConsultantJSON;
            PatientID = a.PatientID;
            PatientName = a.PatientName;
            IIInessType = a.IIInessType;
            PatientCity = a.PatientCity;
            IIInessDescription = a.IIInessDescription;
            InquiryTimes = a.InquiryTimes;
            ConsultState = a.ConsultState;
            OvertimeRemindTimes = a.OvertimeRemindTimes;
            Overtime = a.Overtime;
            RefundBy = a.RefundBy;
            RecommendDoctorID = a.RecommendDoctorID;
            RecommendDoctorName = a.RecommendDoctorName;
            HasReply = a.HasReply;
            Age = a.Age;
            ServiceMoneyListId = a.ServiceMoneyListId;
            HasInquiryTimes = a.HasInquiryTimes;
            IsReturnVisit = a.IsReturnVisit;
            IsEvaluate = a.IsEvaluate;
            ReturnVisit = a.ReturnVisit;
            ReturnVisitTime = a.ReturnVisitTime;
            Completetime = a.Completetime;
            DoctorID = a.DoctorID;
            DoctorName = a.DoctorName;
            CreatedOn = a.CreatedOn;
            ConsultNumber = a.ConsultNumber;
            ConsultID = a.ConsultID;
            ConsultantID = a.ConsultantID;
            ConsultantName = a.ConsultantName;
            ConsultType = a.ConsultType;
            QualityLevel = a.QualityLevel;
            RepayIllnessDescription = a.RepayIllnessDescription;
            ReplyState = a.ReplyState;
            QualityControlManageState = a.QualityControlManageState;
            QualityControlManageStateCode = a.QualityControlManageState;
            QualityDoctor = a.QualityDoctor;
        }
        /// <summary>
        /// 状态code
        /// </summary>
        public string QualityControlManageStateCode { get; set; }

        /// <summary>
        /// 1男2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }

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

        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }

        /// <summary>
        /// 疾病类型
        /// </summary>

        public string IIInessType { get; set; }
        /// <summary>
        /// 疾病类型JSON
        /// </summary>
        public string IIInessJSON { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>

        public string PhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>

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
        /// 咨询问诊状态,已创建，咨询中,已退单,已完成
        /// </summary>

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

        public string RefundType { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>

        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>

        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>

        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因 手填
        /// </summary>

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
        /// 是否已回复
        /// </summary>
        public bool HasReply { get; set; }
        /// <summary>
        /// 患者年龄
        /// </summary>

        public string Age { get; set; }
        /// <summary>
        /// 医生提供服务费用表Id
        /// </summary>
        public int ServiceMoneyListId { get; set; }
        /// <summary>
        /// 剩余追问次数
        /// </summary>
        public int HasInquiryTimes { get; set; }

        /// <summary>
        /// 是否已回访
        /// </summary>
        public bool IsReturnVisit { get; set; }

        /// <summary>
        /// 是否评价
        /// </summary>
        public bool IsEvaluate { get; set; }

        /// <summary>
        /// 回访内容
        /// </summary>

        public string ReturnVisit { get; set; }
        /// <summary>
        /// 回访时间
        /// </summary>
        public DateTime ReturnVisitTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime Completetime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QualityDoctor { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>

        public string QualityControlManageState { get; set; }
    }

    /// <summary>
    /// 质控委员查看代办page
    /// </summary>
    public class QualityControlManagePage : QualityControlManage
    {
        public QualityControlManagePage()
        {

        }
        public string QualityControlManageStateCode { get; set; }
        /// <summary>
        /// 1男2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }

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

        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }

        /// <summary>
        /// 疾病类型
        /// </summary>

        public string IIInessType { get; set; }
        /// <summary>
        /// 疾病类型JSON
        /// </summary>
        public string IIInessJSON { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>

        public string PhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>

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
        /// 咨询问诊状态,已创建，咨询中,已退单,已完成
        /// </summary>

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

        public string RefundType { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>

        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>

        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>

        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因 手填
        /// </summary>

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
        /// 是否已回复
        /// </summary>
        public bool HasReply { get; set; }
        /// <summary>
        /// 患者年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 医生提供服务费用表Id
        /// </summary>
        public int ServiceMoneyListId { get; set; }
        /// <summary>
        /// 剩余追问次数
        /// </summary>
        public int HasInquiryTimes { get; set; }
        /// <summary>
        /// 是否已回访
        /// </summary>
        public bool IsReturnVisit { get; set; }
        /// <summary>
        /// 是否评价
        /// </summary>
        public bool IsEvaluate { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string ReturnVisit { get; set; }
        /// <summary>
        /// 回访时间
        /// </summary>
        public DateTime ReturnVisitTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime Completetime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>

        public string QualityControlManageState { get; set; }
        /// <summary>
        /// 质控医生
        /// </summary>
        public string QualityDoctor { get; set; }
    }
    /// <summary>
    /// 患者端咨询明细
    /// </summary>
    public class QualityControlManageOutDetail
    {

        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="quality"></param>
        /// <param name="item"></param>
        /// <param name="hasCollect"></param>
        /// <param name="consultationfile"></param>
        /// <param name="reply"></param>
        /// <param name="doctor"></param>
        /// <param name="eva"></param>
        /// <param name="serverid"></param>
        /// <param name="IIIness"></param>
        ///  <param name="paramlist"></param>
        public QualityControlManageOutDetail(QualityControlManage quality, YaeherConsultation item, bool hasCollect, IList<ReplyDetail> consultationfile, IList<ReplyDetail> reply, YaeherUser doctor, IList<ConsultationEvaluation> eva, int serverid, int IIIness, List<SystemParameter> paramlist )
        {
            Id = quality.Id;
            Sex = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).Sex;
            QualityLevel = quality.QualityLevel;
            RepayIllnessDescription = quality.RepayIllnessDescription;
            ReplyState = quality.ReplyState;
            IsReturnVisit = item.IsReturnVisit;
            HasCollect = hasCollect;
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
            QualityDoctor = quality.DoctorName;
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
            var replycount = reply.Where(t => t.Message != "" && t.Message != null && t.ReplyType == "inquiries").ToList();
            ReplysCount = 2 - replycount.Count > 0 ? 2 - replycount.Count : 0;
            ConsulationStatusCode = item.ConsultState;
            ConsultState = paramlist.Find(t => t.Code == item.ConsultState).Name;
            if (item.ConsultState == "consulting") { CanReplys = true; } else { CanReplys = false; }
            if (item.ConsultState == "success" && !HasEvaluation)
            { CanEvaluation = true; }
            else
            { CanEvaluation = false; }
            if (item.ConsultState == "success" || item.ConsultState == "return") { CanDelete = true; } else { CanDelete = false; }
            if (ReplysCount < 1) { CanReplys = false; }
            HasEvaluation = item.IsEvaluate;
            ReturnVisit = item.ReturnVisit;
            EvaluationId = eva.Count > 0 ? eva[0].Id : 0;
            ServiceMoneyListID = serverid;
            HasAllergic = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).HasAllergic;
            AllergicHistory = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(item.PatientJSON).AllergicHistory;
        }
        /// <summary>
        /// 质控评分Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///指控委员 评分
        /// </summary>
        public virtual int QualityLevel { get; set; }
        /// <summary>
        /// 指控委员处理描述
        /// </summary>
        public virtual string RepayIllnessDescription { get; set; }
        /// <summary>
        /// 指控委员处理状态  未处理  处理  退回
        /// </summary>
        public virtual string ReplyState { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
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
        /// 可追问次数
        /// </summary>
        public int ReplysCount { get; set; }
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
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
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
        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }
        /// <summary>
        /// 咨询类型
        /// 图文 电话 或者其他
        /// </summary>
        public string ConsultType { get; set; }
        /// <summary>
        /// 疾病类型
        /// </summary>
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
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
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
        public string RefundType { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>
        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>
        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因
        /// </summary>
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
        /// 
        /// </summary>
        public string QualityDoctor { get; set; }

    }
    /// <summary>
    /// 质控委员
    /// </summary>
    public class QualityDoctorInfoOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public QualityDoctorInfoOut(PagedResultDto<QualityManager> ClinicInfomationDto,QualityCommitteeRegisterIn ClinicInfomationInfo)
        {
            Items = ClinicInfomationDto.Items;
            TotalCount = ClinicInfomationDto.TotalCount;
            TotalPage = ClinicInfomationDto.TotalCount / ClinicInfomationInfo.MaxResultCount;
            SkipCount = ClinicInfomationInfo.SkipCount;
            MaxResultCount = ClinicInfomationInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public QualityDoctorInfoOut(PagedResultDto<QualityManager> ClinicInfomationDto,QualityCommitteeIn ClinicInfomationInfo)
        {
            Items = ClinicInfomationDto.Items;
            TotalCount = ClinicInfomationDto.TotalCount;
            TotalPage = ClinicInfomationDto.TotalCount / ClinicInfomationInfo.MaxResultCount;
            SkipCount = ClinicInfomationInfo.SkipCount;
            MaxResultCount = ClinicInfomationInfo.MaxResultCount;
        }
        /// <summary>
        /// 科室集合
        /// </summary>
        public IReadOnlyList<QualityManager> Items { get; set; }
    }
    /// <summary>
    /// 质控委员
    /// </summary>
    public class QualityManager : ClinicDoctorsView
    {
        /// <summary>
        /// 质控申请状态
        /// </summary>
        public string CheckState { get; set; }
        /// <summary>
        /// 申请Id
        /// </summary>
        public int QualityCommitteeRegisterId { get; set; }
        /// <summary>
        /// 申请原因
        /// </summary>
        public string QualityApplyRemark { get; set; }
        /// <summary>
        /// 申请类型    申请质控 申请停用质控
        /// </summary>
        public virtual string ApplyState { get; set; }
    }
    /// <summary>
    /// 质控退单率查看
    /// </summary>
    public class RefundReport
    {
        /// <summary>
        /// 退单率
        /// </summary>
        public string RefundRate { get; set; }
        /// <summary>
        /// 退单数
        /// </summary>
        public int RefundCount { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public int ConsultationCount { get; set; }
    }
}
