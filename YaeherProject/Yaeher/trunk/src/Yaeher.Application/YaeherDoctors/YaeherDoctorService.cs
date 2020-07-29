using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.YaeherDoctors.Dto;
using Yaeher.ClinicManage.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctorService : IYaeherDoctorService
    {
        private readonly IRepository<YaeherDoctor> _repository;
        private readonly IRepository<ClinicDoctorReltion> _relrepository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<DoctorOnlineRecord> _doctoronlinerepository;
        private readonly IRepository<ServiceMoneyList> _serviceMoneyListrepository;

        /// <summary>
        /// 医生基本信息 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="relrepository"></param>
        /// <param name="userrepository"></param>
        /// <param name="doctoronlinerepository"></param>
        /// <param name="serviceMoneyListrepository"></param>
        public YaeherDoctorService(IRepository<YaeherDoctor> repository,
            IRepository<ClinicDoctorReltion> relrepository,
            IRepository<YaeherUser> userrepository,
            IRepository<DoctorOnlineRecord> doctoronlinerepository,
            IRepository<ServiceMoneyList> serviceMoneyListrepository)
        {
            _repository = repository;
            _relrepository = relrepository;
            _userrepository = userrepository;
            _doctoronlinerepository = doctoronlinerepository;
            _serviceMoneyListrepository = serviceMoneyListrepository;
        }
        /// <summary>
        /// 查询医生基本信息 List
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherDoctor>> YaeherDoctorList(YaeherDoctorIn YaeherDoctorInfo)
        {
            var YaeherDoctors = await _repository.GetAllListAsync(YaeherDoctorInfo.Expression);
            return YaeherDoctors.ToList();
        }
        /// <summary>
        /// 查询医生基本信息 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherClinicDoctor>> YaeherClinicDoctorList()
        {
            var relation = _relrepository.GetAll();
            var label = _repository.GetAll();
            var user = _userrepository.GetAll();
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete);
            var querylist = from a in relation
                            join b in label on a.DoctorID equals b.Id
                            join c in user on b.UserID equals c.Id
                            join d in doctorline on a.DoctorID equals d.DoctorID
                            where !a.IsDelete && !b.IsDelete && !c.IsDelete && b.CheckRes == "success" && d.OnlineState == "Online" && b.AuthCheckRes == "success"
                            select new YaeherClinicDoctor
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy = b.CreatedBy,
                                ModifyBy = b.ModifyBy,
                                ModifyOn = b.ModifyOn,
                                UserImage = c.UserImage,
                                DoctorName = b.DoctorName,
                                ClinicID = a.ClinicID,
                                Id = a.DoctorID,
                            };
            return await querylist.ToListAsync();
        }
        /// <summary>
        /// 查询老医生基本信息 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherClinicDoctor>> YaeherClinicOldDoctorList(IList<DoctorNew> doctorolds)
        {
            var relation = _relrepository.GetAll();
            var label = _repository.GetAll();
            var user = _userrepository.GetAll();
            var querylist = from a in relation
                            join b in label on a.DoctorID equals b.Id
                            join c in user on b.UserID equals c.Id
                            where !a.IsDelete && !b.IsDelete && !c.IsDelete && b.CheckRes == "success" && b.AuthCheckRes == "success"
                            select new YaeherClinicDoctor
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy = b.CreatedBy,
                                ModifyBy = b.ModifyBy,
                                ModifyOn = b.ModifyOn,
                                UserImage = c.UserImage,
                                DoctorName = b.DoctorName,
                                ClinicID = a.ClinicID,
                                Id = a.DoctorID,
                            };
            if (doctorolds.Count > 0)
            {
                var newdoctorrel = new int[doctorolds.Count];
                for (var i = 0; i < doctorolds.Count(); i++)
                {
                    newdoctorrel[i] = doctorolds[i].DoctorId;
                }
                if (newdoctorrel.Count() > 0)
                {
                    querylist = querylist.Where(t => newdoctorrel.Contains(t.Id));
                }
            }
            return await querylist.ToListAsync();
        }


        /// <summary>
        /// 查询某科室在职医生基本信息 page
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherClinicDoctor>> YaeherClinicDoctorPage(ClinicInfomationIn input)
        {
            var relation = _relrepository.GetAll();
            var label = _repository.GetAll();
            var user = _userrepository.GetAll();
            var query = from a in relation
                        join b in label on a.DoctorID equals b.Id
                        join c in user on b.UserID equals c.Id
                        where !a.IsDelete && !b.IsDelete && b.CheckRes == "success" && b.AuthCheckRes == "success"
                        where !a.IsDelete && !b.IsDelete
                        select new YaeherClinicDoctor
                        {
                            CreatedOn = b.CreatedOn,
                            CreatedBy = b.CreatedBy,
                            ModifyBy = b.ModifyBy,
                            ModifyOn = b.ModifyOn,
                            UserImage = c.UserImage,
                            DoctorName = b.DoctorName,
                            ClinicID = a.ClinicID,
                        };

            //获取总数
            var tasksCount = query.Count();
            //获取总页码
            var totalpage = tasksCount / input.MaxResultCount;

            var YaeherDoctorList = await query.PageBy(input.SkipTotal, input.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherClinicDoctor>(tasksCount, YaeherDoctorList.MapTo<List<YaeherClinicDoctor>>());
        }


        /// <summary>
        /// 查询医生基本信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctor> YaeherDoctorByID(int Id)
        {
            var YaeherDoctors = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherDoctors;
        }
        /// <summary>
        /// 查询医生基本信息byId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctor> YaeherDoctorByUserID(int UserId)
        {
            var YaeherDoctors = await _repository.FirstOrDefaultAsync(t => t.UserID == UserId && !t.IsDelete);
            return YaeherDoctors;
        }

        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherDoctor>> YaeherDoctorPage(YaeherDoctorIn YaeherDoctorInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(YaeherDoctorInfo.Expression).OrderByDescending(t => t.CreatedOn);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherDoctorInfo.MaxResultCount;
            var YaeherDoctorList = await query.PageBy(YaeherDoctorInfo.SkipTotal, YaeherDoctorInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherDoctor>(tasksCount, YaeherDoctorList.MapTo<List<YaeherDoctor>>());
        }
        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherDoctorUser>> YaeherDoctorUserPage(YaeherDoctorIn YaeherDoctorInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().Where(YaeherDoctorInfo.Expression).OrderByDescending(t => t.CreatedOn);
            var user = _userrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in query1
                        join b in user on a.UserID equals b.Id
                        select new YaeherDoctorUser
                        {
                            DoctorName = a.DoctorName,
                            UserID = a.UserID,
                            Address = a.Address,
                            HospitalName = a.HospitalName,
                            Department = a.Department,
                            WorkYear = a.WorkYear,
                            Title = a.Title,
                            GraduateSchool = a.GraduateSchool,
                            IsBelieveTCM = a.IsBelieveTCM,
                            IsServiceConscious = a.IsServiceConscious,
                            WechatNum = a.WechatNum,
                            PhoneNumber = a.PhoneNumber,
                            Recommender = a.Recommender,
                            RecommenderName = a.RecommenderName,
                            CheckRes = a.CheckRes,
                            Checker = a.Checker,
                            CheckRemark = a.CheckRemark,
                            CheckTime = a.CheckTime,
                            TsetTime = a.TsetTime,
                            TestID = a.TestID,
                            BaseTestRes = a.BaseTestRes,
                            SimTestRes = a.SimTestRes,
                            AuthCheckRes = a.AuthCheckRes,
                            AuthChecker = a.AuthChecker,
                            AuthType = a.AuthType,
                            AuthCheckRemark = a.AuthCheckRemark,
                            AuthCheckTime = a.AuthCheckTime,
                            UserImageFile = a.UserImageFile,
                            Resume = a.Resume,
                            UserImage = b.UserImage,
                            CreatedOn = a.CreatedOn,
                            Id = a.Id,
                            IsAbroad = a.IsAbroad,
                        };
            //获取总数
            var tasksCount = query.Count();
            if (YaeherDoctorInfo.CheckRes == "checking")//未处理顺序
            {
                query = query.OrderBy(t => t.CreatedOn);
            }
            else//已处理倒叙
            {
                query = query.OrderByDescending(t => t.CreatedOn);
            }
            //获取总数
            var totalpage = tasksCount / YaeherDoctorInfo.MaxResultCount;
            var YaeherDoctorList = await query.PageBy(YaeherDoctorInfo.SkipTotal, YaeherDoctorInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherDoctorUser>(tasksCount, YaeherDoctorList.MapTo<List<YaeherDoctorUser>>());
        }
        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctorUser> YaeherDoctorUser(YaeherDoctorIn YaeherDoctorInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().Where(YaeherDoctorInfo.Expression);
            var user = _userrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in query1
                        join b in user on a.UserID equals b.Id
                        select new YaeherDoctorUser
                        {
                            DoctorName = a.DoctorName,
                            UserID = a.UserID,
                            Address = a.Address,
                            HospitalName = a.HospitalName,
                            Department = a.Department,
                            WorkYear = a.WorkYear,
                            Title = a.Title,
                            GraduateSchool = a.GraduateSchool,
                            IsBelieveTCM = a.IsBelieveTCM,
                            IsServiceConscious = a.IsServiceConscious,
                            WechatNum = a.WechatNum,
                            PhoneNumber = a.PhoneNumber,
                            Recommender = a.Recommender,
                            RecommenderName = a.RecommenderName,
                            CheckRes = a.CheckRes,
                            Checker = a.Checker,
                            CheckRemark = a.CheckRemark,
                            CheckTime = a.CheckTime,
                            TsetTime = a.TsetTime,
                            TestID = a.TestID,
                            BaseTestRes = a.BaseTestRes,
                            SimTestRes = a.SimTestRes,
                            AuthCheckRes = a.AuthCheckRes,
                            AuthChecker = a.AuthChecker,
                            AuthType = a.AuthType,
                            AuthCheckRemark = a.AuthCheckRemark,
                            AuthCheckTime = a.AuthCheckTime,
                            UserImage = b.UserImage,
                            UserImageContent = b.UserImage,
                            UserImageFile = a.UserImageFile,
                            UserImageFileContent = a.UserImageFile,
                            Resume = a.Resume,
                            CreatedOn = a.CreatedOn,
                            Id = a.Id,
                            IsAbroad = a.IsAbroad,
                        };
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherDoctor>> QualityYaeherDoctorPage(YaeherDoctorIn YaeherDoctorInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(YaeherDoctorInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherDoctorInfo.MaxResultCount;
            var YaeherDoctorList = await query.PageBy(YaeherDoctorInfo.SkipTotal, YaeherDoctorInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherDoctor>(tasksCount, YaeherDoctorList.MapTo<List<YaeherDoctor>>());
        }

        /// <summary>
        /// 新建医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctor> CreateYaeherDoctor(YaeherDoctor YaeherDoctorInfo)
        {
            YaeherDoctorInfo.Id = await _repository.InsertAndGetIdAsync(YaeherDoctorInfo);
            return YaeherDoctorInfo;
        }

        /// <summary>
        /// 修改医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctor> UpdateYaeherDoctor(YaeherDoctor YaeherDoctorInfo)
        {
            return await _repository.UpdateAsync(YaeherDoctorInfo);
        }

        /// <summary>
        /// 删除医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherDoctor> DeleteYaeherDoctor(YaeherDoctor YaeherDoctorInfo)
        {
            return await _repository.UpdateAsync(YaeherDoctorInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<bool> UpdateDoctorService(DoctorInfo doctorInfo)
        {
            if (doctorInfo.DoctorId != 0)
            {
                var DoctorImageText = await _serviceMoneyListrepository.FirstOrDefaultAsync(a => a.DoctorID == doctorInfo.DoctorId && a.ServiceType=="ImageText" && a.IsDelete == false);
                var DoctorPhone = await _serviceMoneyListrepository.FirstOrDefaultAsync(a => a.DoctorID == doctorInfo.DoctorId && a.ServiceType=="Phone" && a.IsDelete == false);
                var DoctorInfo = await _repository.FirstOrDefaultAsync(a => a.Id == doctorInfo.DoctorId);
                // 实际接单数
                int ChangeNumer = 0;
                bool ImageFull = false;  // 图文是否满单
                bool PhoneFull = false;  // 电话是否满单
                if (doctorInfo.OrderType == "order")
                {
                    ChangeNumer = 1;
                }
                else
                {
                    ChangeNumer = -1;
                }
                if (doctorInfo.ServiceType == "ImageText")
                {
                    // 图文
                    DoctorImageText.ActualNumber = DoctorImageText.ActualNumber + ChangeNumer;
                    if (DoctorImageText.ActualNumber > DoctorImageText.ServiceFrequency)
                    {
                        // 超过了接单数
                        return false;
                    }
                    else
                    {
                        if (DoctorImageText.ServiceFrequency == DoctorImageText.ActualNumber)
                        {
                            ImageFull = true;
                        }
                        await _serviceMoneyListrepository.UpdateAsync(DoctorImageText);
                        return true;
                    }
                }
                else if (doctorInfo.ServiceType == "Phone")
                {
                    //电话
                    DoctorPhone.ActualNumber = DoctorPhone.ActualNumber + ChangeNumer;
                    if (DoctorPhone.ActualNumber > DoctorPhone.ServiceFrequency)
                    {
                        // 超过了接单数
                        return false;
                    }
                    else
                    {
                        if (DoctorPhone.ServiceFrequency == DoctorPhone.ActualNumber)
                        {
                            PhoneFull = true;
                        }
                        await _serviceMoneyListrepository.UpdateAsync(DoctorPhone);
                        return true;
                    }
                }
                // 当图文、电话服务关闭 所有服务是关闭
                if (!DoctorImageText.ServiceState && !DoctorPhone.ServiceState)
                {
                    DoctorInfo.ServiceState = false;
                }
                else
                {
                    DoctorInfo.ServiceState = true;
                }
                // 判断是否满单
                if(DoctorImageText.ServiceState)
                {
                    if (ImageFull && (!DoctorPhone.ServiceState || PhoneFull))
                    {
                        DoctorInfo.ReceiptState = false;
                    }
                    else
                    {
                        DoctorInfo.ReceiptState = true;
                    }
                }
                if (DoctorPhone.ServiceState)
                {
                    if (PhoneFull && (!DoctorImageText.ServiceState || ImageFull))
                    {
                        DoctorInfo.ReceiptState = false;
                    }
                    else
                    {
                        DoctorInfo.ReceiptState = true;
                    }
                }
                await _repository.UpdateAsync(DoctorInfo);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
