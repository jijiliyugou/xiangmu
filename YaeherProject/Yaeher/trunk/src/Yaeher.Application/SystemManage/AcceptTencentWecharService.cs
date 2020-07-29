using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common.Constants;
using Yaeher.Release;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    ///  微信发送消息
    /// </summary>
    public class AcceptTencentWecharService : IAcceptTencentWecharService
    {
        private readonly IRepository<AcceptTencentWechar> _repository;

        private readonly IRepository<RelationLabelGroup> _RelationLabelGrouprepository;
        private readonly IRepository<RelationLabelList> _RelationLabelListrepository;
        private readonly IRepository<ClinicInfomation> _ClinicInfomationrepository;
        private readonly IRepository<YaeherDoctor> _YaeherDoctorrepository;
        private readonly IRepository<ReleaseManage> _ReleaseManagerepository;
        private readonly IRepository<QuestionRelease> _QuestionReleaserepository;
        private readonly IRepository<ServiceMoneyList> _ServiceMoneyListrepository;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="RelationLabelGrouprepository"></param>
        /// <param name="RelationLabelListrepository"></param>
        /// <param name="ClinicInfomationrepository"></param>
        /// <param name="YaeherDoctorrepository"></param>
        /// <param name="ReleaseManagerepository"></param>
        /// <param name="QuestionReleaserepository"></param>
        /// <param name="ServiceMoneyListrepository"></param>
        public AcceptTencentWecharService(IRepository<AcceptTencentWechar> repository,
                                          IRepository<RelationLabelGroup> RelationLabelGrouprepository,
                                          IRepository<RelationLabelList> RelationLabelListrepository,
                                          IRepository<ClinicInfomation> ClinicInfomationrepository,
                                          IRepository<YaeherDoctor> YaeherDoctorrepository,
                                          IRepository<ReleaseManage> ReleaseManagerepository,
                                          IRepository<QuestionRelease> QuestionReleaserepository,
                                          IRepository<ServiceMoneyList> ServiceMoneyListrepository)
        {
            _repository = repository;
            _RelationLabelGrouprepository = RelationLabelGrouprepository;
            _RelationLabelListrepository= RelationLabelListrepository;
            _ClinicInfomationrepository= ClinicInfomationrepository;
            _YaeherDoctorrepository= YaeherDoctorrepository;
            _ReleaseManagerepository= ReleaseManagerepository;
            _QuestionReleaserepository= QuestionReleaserepository;
            _ServiceMoneyListrepository = ServiceMoneyListrepository;
        }

        /// <summary>
        /// 发送消息模板 List
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<AcceptTencentWechar>> AcceptTencentList(AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            //初步过滤
            var AcceptTencentWechars = _repository.GetAll().OrderBy(a => a.CreatedOn).Where(AcceptTencentWecharInfo.Expression);
            return await AcceptTencentWechars.ToListAsync();
        }

        /// <summary>
        /// 发送消息模板 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptTencentWechar> AcceptTencentByID(int Id)
        {
            var AcceptTencentWechars = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return AcceptTencentWechars;
        }
        /// <summary>
        /// 发送消息模板 page
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<AcceptTencentWechar>> AcceptTencentPage(AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(AcceptTencentWecharInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / AcceptTencentWecharInfo.MaxResultCount;
            var AcceptTencentWecharList = await query.PageBy(AcceptTencentWecharInfo.SkipTotal, AcceptTencentWecharInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<AcceptTencentWechar>(tasksCount, AcceptTencentWecharList.MapTo<List<AcceptTencentWechar>>());
        }
        /// <summary>
        /// 新建 发送消息模板
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptTencentWechar> CreateAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo)
        {
            //AcceptTencentWecharInfo.Id=  _repository.InsertAndGetId(AcceptTencentWecharInfo);
            //return AcceptTencentWecharInfo;
            // 不做数据返回操作
            return await _repository.InsertAsync(AcceptTencentWecharInfo);
        }

        /// <summary>
        /// 修改 发送消息模板
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptTencentWechar> UpdateAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo)
        {
            return await _repository.UpdateAsync(AcceptTencentWecharInfo);
        }

        /// <summary>
        /// 删除 发送消息模板
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptTencentWechar> DeleteAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo)
        {
            return await _repository.UpdateAsync(AcceptTencentWecharInfo);
        }

         /// <summary>
        /// 推荐发送发送消息
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> SendWecharMesaage(string KeyWord)
        {
            // 查询标签关联的组数据
            RelationLabelGroupIn relationLabelGroupIn = new RelationLabelGroupIn();
            relationLabelGroupIn.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(KeyWord))
            {
                relationLabelGroupIn.AndAlso(a => a.LableName.Contains(KeyWord));
            }
            else
            { 
                // 当输入为空时 不查询
                return null;
            }
            var LabelGroupList =await _RelationLabelGrouprepository.GetAll().Where(relationLabelGroupIn.Expression).ToListAsync();
            List<LabelGroup> labelGroups = new List<LabelGroup>();
            if (LabelGroupList != null)
            {
                foreach (var Item in LabelGroupList)
                {
                    string[] LableID = Item.LableID.Split(',');
                    string[] LableName = Item.LableName.Split(',');
                    if (LableID.Length > 0)
                    {
                        for (int a = 0; a < LableID.Length; a++)
                        {
                            LabelGroup labelGroup = new LabelGroup();
                            labelGroup.GroupName = Item.GroupName;
                            labelGroup.LableID = int.Parse(LableID[a]);
                            labelGroup.LableName = LableName[a];
                            labelGroups.Add(labelGroup);
                        }
                    }
                }
            }
            // 查询业务与标签之间的关系
            RelationLabelListIn relationLabelListIn = new RelationLabelListIn();
            relationLabelListIn.AndAlso(a => a.IsDelete == false);
            var relationLabelList = _RelationLabelListrepository.GetAll().Where(relationLabelListIn.Expression);
            List<RelationLabelList> relationLabelLists = new List<RelationLabelList>(); 
            if (LabelGroupList != null)
            {
                var RelationLabelLists = from a in labelGroups
                                         join b in relationLabelList on a.LableID equals b.LableID
                                         select b;
                if (RelationLabelLists != null)
                {
                    relationLabelLists = RelationLabelLists.Distinct().ToList();
                }
            }
            StringBuilder sbClinic = new StringBuilder();
            StringBuilder sbDoctor = new StringBuilder();
            StringBuilder sbPaper = new StringBuilder();
            StringBuilder sbQuestion = new StringBuilder();
            if (relationLabelLists != null)
            {
                
                // 关键字查询科室信息
                var ClinicList = relationLabelLists.Where(a => a.RelationCode == "Clinic").ToList();
                if (ClinicList != null&&ClinicList.Count>0)
                {
                    ClinicInfomationIn clinicInfomationIn = new ClinicInfomationIn();
                    clinicInfomationIn.AndAlso(a => a.IsDelete == false);
                    // 增加查询状态
                    var YaeherCinlinList = _ClinicInfomationrepository.GetAll().Where(clinicInfomationIn.Expression);
                    var RecomCinlinList = from a in ClinicList
                                          join b in YaeherCinlinList on a.BusinessID equals b.Id
                                          select b;
                    if (RecomCinlinList != null)
                    {
                        var CinlinList = RecomCinlinList.Distinct().ToList().OrderBy(a=>a.OrderSort).ThenByDescending(a=>a.CreatedOn).Take(4);
                        //int CinlinCount = 0;
                        foreach (var Item in CinlinList)
                        {
                            var ClinicUrl= Commons.WecharWeb + "doctor-list-patient?id="+Item.Id+"&clinicNmae="+Item.ClinicName+"&rShow=no";
                            sbClinic.Append("<a href=\""+ClinicUrl+"\">"+Item.ClinicName+"</a>   ");
                            sbClinic.Append("\n");
                            // 去掉科室换行
                            //CinlinCount += 1;
                            //if (CinlinCount >= 2)
                            //{
                            //    sbClinic.Append("\n");
                            //    sbClinic.Append("\n");
                            //    CinlinCount = 0;
                            //}
                        }
                    }
                }
               
                #region 关键字查询医生信息 暂时去掉
                // 关键字查询医生信息
                //var DoctorList = relationLabelLists.Where(a => a.RelationCode == "Doctor").ToList();
                //if (DoctorList != null&&DoctorList.Count>0)
                //{
                //    YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
                //    yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
                //    yaeherDoctorIn.AndAlso(a => a.AuthCheckRes =="success");
                //    // 增加查询状态
                //    var YaeherDoctotList = _YaeherDoctorrepository.GetAll().Where(yaeherDoctorIn.Expression);
                //    //查询医生开启状态
                //    ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
                //    serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
                //    serviceMoneyListIn.AndAlso(a => a.ServiceState == true);
                //    var DocotorService= _ServiceMoneyListrepository.GetAll().Where(serviceMoneyListIn.Expression);

                //    var RecomDoctotList = from a in DoctorList
                //                          join b in YaeherDoctotList on a.BusinessID equals b.Id
                //                          join c in DocotorService on b.Id equals c.DoctorID
                //                          select b;
                //    if (RecomDoctotList != null)
                //    {
                //        var DoctotList = RecomDoctotList.Distinct().ToList().OrderByDescending(a=>a.CreatedOn).Take(12);
                //        int DoctotCount = 0;
                //        foreach (var Item in DoctotList)
                //        {
                //            var DoctotUrl= Commons.WecharWeb + "doctor-detail-patient?id="+Item.Id+"&rShow=no";
                //            sbDoctor.Append("<a href=\""+DoctotUrl+"\">"+Item.DoctorName+"</a>   ");
                //            DoctotCount += 1;
                //            if (DoctotCount >=3)
                //            {
                //                sbDoctor.Append("\n");
                //                sbDoctor.Append("\n");
                //                DoctotCount = 0;
                //            }
                //        }
                //    }
                //}
                //// 关键字查询文章信息
                //var PaperList = relationLabelLists.Where(a => a.RelationCode == "Paper").ToList();
                //if (PaperList != null&&PaperList.Count>0)
                //{
                //    ReleaseManageIn releaseManageIn = new ReleaseManageIn();
                //    releaseManageIn.AndAlso(a => a.IsDelete == false);
                //    // 增加查询状态
                //    var ReleasList = _ReleaseManagerepository.GetAll().Where(releaseManageIn.Expression);
                //    var RecomReleasList = from a in PaperList
                //                            join b in ReleasList on a.BusinessID equals b.Id
                //                            select b;
                //    if (RecomReleasList != null)
                //    {
                //        var ReleasLists = RecomReleasList.Distinct().ToList().OrderByDescending(a=>a.CreatedOn).Take(5);
                //        foreach (var Item in ReleasLists)
                //        {
                //            var ReleasUrl= Commons.WecharWeb + "article-detail?id="+Item.Id+"&rShow=no&look=no";
                //            sbPaper.Append("<a href=\""+ReleasUrl+"\">"+Item.PaperTiltle+"</a>\n");
                //        }
                //    }
                //}
                //// 关键字查询问答信息
                //var QuestionList = relationLabelLists.Where(a => a.RelationCode == "Question").ToList();
                //if (QuestionList != null&&QuestionList.Count>0)
                //{
                //    QuestionReleaseIn questionReleaseIn = new QuestionReleaseIn();
                //    questionReleaseIn.AndAlso(a => a.IsDelete == false);
                //    // 增加查询状态
                //    var QuestinList = _QuestionReleaserepository.GetAll().Where(questionReleaseIn.Expression);
                //    var RecomQuestionList = from a in QuestionList
                //                            join b in QuestinList on a.BusinessID equals b.Id
                //                            select b;
                //    if (RecomQuestionList != null)
                //    {
                //        var QuestionLists = RecomQuestionList.Distinct().ToList().OrderByDescending(a=>a.CreatedOn).Take(5);
                //        foreach (var Item in QuestionLists)
                //        {
                //            var QuestionUrl= Commons.WecharWeb + "question-detail?id="+Item.Id+"&rShow=no";
                //            sbQuestion.Append("<a href=\""+QuestionUrl+"\">"+Item.Title+"</a>\n");
                //        }
                //    }
                //}
                #endregion
            }
            StringBuilder sbWecharMessage = new StringBuilder();
            if (sbClinic.ToString() != null&&sbClinic.ToString()!="")
            {
                sbWecharMessage.Append("推荐科室： \n");
                sbWecharMessage.Append(sbClinic.ToString() +"\n");
            }
            #region  暂时去掉
            //if (sbDoctor.ToString() != null&&sbDoctor.ToString()!="")
            //{
            //    sbWecharMessage.Append("推荐医生： \n");
            //    sbWecharMessage.Append(sbDoctor.ToString() +"\n");
            //}
            //if (sbPaper.ToString() != null&&sbPaper.ToString()!="")
            //{
            //    sbWecharMessage.Append("推荐文章： \n");
            //    sbWecharMessage.Append(sbPaper.ToString() +"\n");
            //}
            //if (sbQuestion.ToString() != null&&sbQuestion.ToString()!="")
            //{
            //    sbWecharMessage.Append("推荐问答： \n");
            //    sbWecharMessage.Append(sbQuestion.ToString() +"\n");
            //}
            #endregion
            return sbWecharMessage.ToString();
        }
    }
}
