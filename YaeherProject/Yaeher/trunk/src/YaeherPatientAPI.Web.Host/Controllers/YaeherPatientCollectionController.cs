using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.LableManages.Dto;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherPatientAPI.Web.Host.Controllers
{
    /// <summary>
    /// 患者端
    /// </summary>
    public class YaeherPatientCollectionController : YaeherAppServiceBase
    {
        private readonly IPatientCollectionService _PatientCollectionService;
        private readonly IPatientDoctorService _PatientDoctorService;
        private readonly IShareDoctorService _ShareDoctorService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="patientCollectionService"></param>
        /// <param name="patientDoctorService"></param>
        /// <param name="shareDoctorService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        public YaeherPatientCollectionController(IPatientCollectionService patientCollectionService, 
                                                IPatientDoctorService patientDoctorService, 
                                                IShareDoctorService shareDoctorService, 
                                                IAbpSession session, 
                                                IYaeherOperListService yaeherOperListService)
        {
            _PatientCollectionService = patientCollectionService;
            _PatientDoctorService = patientDoctorService;
            _ShareDoctorService = shareDoctorService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
        }
        /// <summary>
        /// 新增我的收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherPatientCollection")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherPatientCollection([FromBody] YaeherPatientCollection input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new YaeherPatientCollection()
            {
                UserID = input.UserID,
                Opentype = input.Opentype,
                CollectionType = input.CollectionType,
                CollectionUrl = input.CollectionUrl,
                CollectionJSON = input.CollectionJSON,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _PatientCollectionService.CreateYaeherPatientCollection(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherPatientCollection",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateYaeherPatientCollection",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收藏Page
        /// </summary>
        /// <param name="PagePatientCollection"> PagePatientCollection 数据</param>
        /// <returns></returns>
        [Route("api/YaeherPatientCollectionPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherPatientCollectionPage([FromBody]YaeherPatientCollectionIn PagePatientCollection)
        {
            if (!Commons.CheckSecret(PagePatientCollection.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PagePatientCollection.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(PagePatientCollection.Opentype))
            {
                PagePatientCollection.AndAlso(t => t.Opentype == PagePatientCollection.Opentype);
            }
            var values = await _PatientCollectionService.YaeherPatientCollectionPage(PagePatientCollection);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherPatientCollectionOut(values, PagePatientCollection);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientCollectionPage",
                OperContent = JsonHelper.ToJson(PagePatientCollection),
                OperType = "YaeherPatientCollectionPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收藏List 
        /// </summary>
        /// <param name="PatientCollectionList"> PagePatientCollection 数据</param>
        /// <returns></returns>
        [Route("api/YaeherPatientCollectionList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherPatientCollectionList([FromBody]YaeherPatientCollectionIn PatientCollectionList)
        {
            if (!Commons.CheckSecret(PatientCollectionList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PatientCollectionList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(PatientCollectionList.Opentype))
            {
                PatientCollectionList.AndAlso(t => t.Opentype.Contains(PatientCollectionList.Opentype));
            }
            var value = await _PatientCollectionService.YaeherPatientCollectionList(PatientCollectionList);
            if (value.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientCollectionList",
                OperContent = JsonHelper.ToJson(PatientCollectionList),
                OperType = "YaeherPatientCollectionList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新我的收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherPatientCollection")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherPatientCollection([FromBody] YaeherPatientCollection input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _PatientCollectionService.YaeherPatientCollectionByID(input.Id);

            if (query != null)
            {
                query.UserID = input.UserID;
                query.Opentype = input.Opentype;
                query.CollectionType = input.CollectionType;
                query.CollectionUrl = input.CollectionUrl;
                query.CollectionJSON = input.CollectionJSON;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _PatientCollectionService.UpdateYaeherPatientCollection(query);
                this.ObjectResultModule.Message = "success";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = res;

            }
            else
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherPatientCollection",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateYaeherPatientCollection",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherPatientCollection")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherPatientCollection([FromBody] YaeherPatientCollection input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _PatientCollectionService.YaeherPatientCollectionByID(input.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _PatientCollectionService.DeleteYaeherPatientCollection(query);
                this.ObjectResultModule.Message = "success";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteYaeherPatientCollection",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteYaeherPatientCollection",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }


        /// <summary>
        /// 新增我的医生
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherPatientDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherPatientDoctor([FromBody] YaeherPatientDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new YaeherPatientDoctor()
            {
                UserID = input.UserID,
                DoctorName = input.DoctorName,
                DoctorID = input.DoctorID,
                DoctorJSON = input.DoctorJSON,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _PatientDoctorService.CreateYaeherPatientDoctor(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherPatientDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateYaeherPatientDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion


            return this.ObjectResultModule;
        }
        /// <summary>
        /// 收藏/取关我的医生
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/YaeherPatientDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherPatientDoctor([FromBody] YaeherPatientDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var collectdoctor = await this._PatientDoctorService.YaeherPatientDoctorByExpression(t => t.DoctorID == input.DoctorID && t.CreatedBy == userid);
            if (collectdoctor != null)
            {
                if (!collectdoctor.IsDelete)
                {
                    collectdoctor.IsDelete = true;
                    collectdoctor.DeleteBy = userid;
                    collectdoctor.DeleteTime = DateTime.Now;
                    var res = await _PatientDoctorService.UpdateYaeherPatientDoctor(collectdoctor);
                    this.ObjectResultModule.Message = "uncollect success";
                }
                else
                {
                    collectdoctor.IsDelete = false;
                    collectdoctor.DeleteBy = 0;
                    var res = await _PatientDoctorService.UpdateYaeherPatientDoctor(collectdoctor);

                    this.ObjectResultModule.Message = "collect success";
                }

                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var secret = await CreateSecret();
                var DocContent = "{\"Id\":" + input.DoctorID.ToString() + ",\"secret\":\""+secret+"\"}";
                var Doctor = await this.PostResponseAsync(Commons.DoctorIp + "api/YaeherDoctorById/", DocContent);
                var DocResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorUser>>>(Doctor);
                if (DocResult == null) { return new ObjectResultModule("", 204, "NoContent"); }

                var create = new YaeherPatientDoctor()
                {
                    UserID = DocResult.result.item.UserID,
                    DoctorName = DocResult.result.item.DoctorName,
                    DoctorID = DocResult.result.item.Id,
                    DoctorJSON = JsonHelper.ToJson(DocResult.result.item),
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var res = await _PatientDoctorService.CreateYaeherPatientDoctor(create);
                if (res.Id > 0)
                {
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "collect sucess";
                    this.ObjectResultModule.Object = res;
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "error!";
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "YaeherPatientDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取我的医生Page
        /// </summary>
        /// <param name="PagePatientDoctor"> PagePatientDoctor 数据</param>
        /// <returns></returns>
        [Route("api/YaeherPatientDoctorPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherPatientDoctorPage([FromBody]YaeherDoctorSearch PagePatientDoctor)
        {
            if (!Commons.CheckSecret(PagePatientDoctor.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
           var secret = await CreateSecret();
            var doc = new YaeherPatientDoctorIn();
            doc.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var doctor = await _PatientDoctorService.YaeherPatientDoctorList(doc);
            var docarray = "";
            for (var i = 0; i < doctor.Count; i++)
            {
                docarray += "" + doctor[i].DoctorID + ",";
            }
            docarray = docarray.TrimEnd(',');
            var nulllist = new List<ClinicDoctorsView>();
            if (string.IsNullOrEmpty(docarray))
            {
                { return new ObjectResultModule(nulllist, 200, "sucess"); }
            }
            var Content = "{\"IDArray\":\"" + docarray.ToString() + "\",\"KeyWord\":\""+ PagePatientDoctor .KeyWord+ "\",\"secret\":\""+secret+"\"}";
            var clinic = await this.PostResponseAsync(Commons.DoctorIp + "api/YaeherPatientDoctorByIDArray/", Content);
            var ClinicInfo = JsonHelper.FromJson<APIResult<List<ClinicDoctorsView>>>(clinic);
            if (ClinicInfo == null || ClinicInfo.result.Count < 1) { { return new ObjectResultModule(nulllist, 200, "sucess"); } }
            //查询标签
            //Content = "{\"IDArray\":\"" + docarray.ToString() + "\"}";
            //var labellist = await this.PostResponseAsync(Commons.DoctorIp + "api/DoctorLableManageList/", Content);
            //var label = JsonHelper.FromJson<APIResult<List<LabelDoctorManage>>>(labellist);
            //if (label == null || label.result.Count < 1) { { return new ObjectResultModule(nulllist, 200, "sucess"); } }

            // 查询科室信息
            //if (ClinicInfo != null && ClinicInfo.result.Count > 0)
            //{
            //    for (var i = 0; i < ClinicInfo.result.Count; i++)
            //    {
            //        ClinicInfo.result[i].Doctorslable = label.result.Where(t => t.DoctorID == ClinicInfo.result[i].Id).ToList();
            //        ClinicInfo.result[i].KeyWord += JsonHelper.ToJson(label.result.Where(t => t.DoctorID == ClinicInfo.result[i].Id).ToList());
            //    }
            //}
            var tasksCount = ClinicInfo.result.Count;
            //var query = from a in ClinicInfo.result
            //            select new ClinicDoctorsView
            //            {
            //                UserImage = a.UserImage,
            //                DoctorName = a.DoctorName,
            //                DoctorLevel = a.DoctorLevel,
            //                UserID = a.UserID,
            //                HospitalName = a.HospitalName,
            //                Title = a.Title,
            //                Status = a.Status,
            //                Id = a.Id,
            //                CreatedOn = a.CreatedOn,
            //                CreatedBy = a.CreatedBy,
            //                ModifyOn = a.ModifyOn,
            //                ModifyBy = a.ModifyBy,
            //                DeleteBy = a.DeleteBy,
            //                DeleteTime = a.DeleteTime,
            //                IsDelete = a.IsDelete,
            //                Doctorslable = a.Doctorslable,
            //                //KeyWord = a.KeyWord,
            //            };
            //if (!string.IsNullOrEmpty(PagePatientDoctor.KeyWord))
            //{
            //    query = query.Where(t => t.KeyWord.Contains(PagePatientDoctor.KeyWord));
            //}
          //  ClinicInfo.result = ClinicInfo.result;
            //获取总数
            var ClinicViewLists = new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfo.result);

            this.ObjectResultModule.Object = new ClinicDoctorInfoOut(ClinicViewLists, PagePatientDoctor);
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientDoctorPage",
                OperContent = JsonHelper.ToJson(PagePatientDoctor),
                OperType = "YaeherPatientDoctorPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;

        }

        /// <summary>
        /// 获取我的医生List 
        /// </summary>
        /// <param name="PatientDoctorList"> PatientDoctorList 数据</param>
        /// <returns></returns>
        [Route("api/YaeherPatientDoctorList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> YaeherPatientDoctorList([FromBody]YaeherPatientDoctorIn PatientDoctorList)
        {
            if (!Commons.CheckSecret(PatientDoctorList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //Logger.Info(JsonHelper.ToJson(PatientDoctorList));
            PatientDoctorList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(PatientDoctorList.DoctorName))
            {
                PatientDoctorList.AndAlso(t => t.DoctorName.Contains(PatientDoctorList.DoctorName));
            }
            if (PatientDoctorList.doctorid > 0)
            {
                PatientDoctorList.AndAlso(t => t.DoctorID == PatientDoctorList.doctorid);
            }
            if (PatientDoctorList.createdby > 0)
            {
                PatientDoctorList.AndAlso(t => t.CreatedBy == PatientDoctorList.createdby);
            }
            if (!string.IsNullOrEmpty(PatientDoctorList.DoctorName))
            {
                PatientDoctorList.AndAlso(t => t.DoctorName.Contains(PatientDoctorList.DoctorName));
            }
            var value = await _PatientDoctorService.YaeherPatientDoctorList(PatientDoctorList);
            if (value.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientDoctorList",
                OperContent = JsonHelper.ToJson(PatientDoctorList),
                OperType = "YaeherPatientDoctorList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        
        /// <summary>
        /// 新增我的分享
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateShareDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateShareDoctor([FromBody] ShareDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new ShareDoctor()
            {
                PatientID = input.PatientID,
                PatientName = input.PatientName,
                PatientJSON = input.PatientJSON,
                DoctorID = input.DoctorID,
                DoctorJSON = input.DoctorJSON,
                DoctorName = input.DoctorName,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _ShareDoctorService.CreateShareDoctor(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateShareDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateShareDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的分享Page
        /// </summary>
        /// <param name="PageShareDoctor"> PageShareDoctor 数据</param>
        /// <returns></returns>
        [Route("api/ShareDoctorPage")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> ShareDoctorPage([FromBody]ShareDoctorIn PageShareDoctor)
        {
            if (!Commons.CheckSecret(PageShareDoctor.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PageShareDoctor.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(PageShareDoctor.DoctorName))
            {
                PageShareDoctor.AndAlso(t => t.DoctorName.Contains(PageShareDoctor.DoctorName));
            }
            var values = await _ShareDoctorService.ShareDoctorPage(PageShareDoctor);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ShareDoctorOut(values, PageShareDoctor);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ShareDoctorPage",
                OperContent = JsonHelper.ToJson(PageShareDoctor),
                OperType = "ShareDoctorPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        
        /// <summary>
        /// 获取我的分享List 
        /// </summary>
        /// <param name="ShareDoctorList"> ShareDoctorList 数据</param>
        /// <returns></returns>
        [Route("api/ShareDoctorList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> ShareDoctorList([FromBody]ShareDoctorIn ShareDoctorList)
        {
            if (!Commons.CheckSecret(ShareDoctorList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ShareDoctorList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ShareDoctorList.DoctorName))
            {
                ShareDoctorList.AndAlso(t => t.DoctorName.Contains(ShareDoctorList.DoctorName));
            }

            var value = await _ShareDoctorService.ShareDoctorList(ShareDoctorList);
            if (value.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ShareDoctorList",
                OperContent = JsonHelper.ToJson(ShareDoctorList),
                OperType = "ShareDoctorList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新我的分享
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateShareDoctor")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> UpdateShareDoctor([FromBody] ShareDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _ShareDoctorService.ShareDoctorByID(input.Id);

            if (query != null)
            {
                query.PatientID = input.PatientID;
                query.PatientName = input.PatientName;
                query.PatientJSON = input.PatientJSON;
                query.DoctorID = input.DoctorID;
                query.DoctorJSON = input.DoctorJSON;
                query.DoctorName = input.DoctorName;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _ShareDoctorService.UpdateShareDoctor(query);
                this.ObjectResultModule.Message = "success";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = res;

            }
            else
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateShareDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateShareDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除我的分享
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteShareDoctor")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> DeleteShareDoctor([FromBody] ShareDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _ShareDoctorService.ShareDoctorByID(input.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _ShareDoctorService.DeleteShareDoctor(query);
                this.ObjectResultModule.Message = "success";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteShareDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteShareDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

    }
}
