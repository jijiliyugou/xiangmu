using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.DoctorRule.Dto;

namespace Yaeher.DoctorRule
{
    /// <summary>
    /// 医生规则 制度 指南
    /// </summary>
    public class DoctorRulesService : IDoctorRulesService
    {
        private readonly IRepository<DoctorRules> _repository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        private readonly IRepository<ClinicDoctorReltion> _clireltorrepository;
        /// <summary>
        /// 医生规则 制度 指南 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userrepository"></param>
        /// <param name="doctorrepository"></param>
        /// <param name="clirelrepository"></param>
        public DoctorRulesService(IRepository<DoctorRules> repository, IRepository<YaeherUser> userrepository, IRepository<YaeherDoctor> doctorrepository, IRepository<ClinicDoctorReltion> clirelrepository)
        {
            _repository = repository;
            _userrepository = userrepository;
            _doctorrepository = doctorrepository;
            _clireltorrepository = clirelrepository;
        }

        /// <summary>
        /// 查询医生规则 List
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorRules>> DoctorRulesList(DoctorRulesIn DoctorRulesInfo)
        {
            var DoctorRuless =  _repository.GetAll().Where(DoctorRulesInfo.Expression);
            if (!string.IsNullOrEmpty(DoctorRulesInfo.IdCheck))
            {
                DoctorRuless = DoctorRuless.Where(t => t.ApplyClinicID.Contains(DoctorRulesInfo.IdCheck));
            }
            return await DoctorRuless.ToListAsync();
        }

        /// <summary>
        /// 查询医生规则byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRules> DoctorRulesByID(int Id)
        {
            var DoctorRuless = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorRuless;
        }
        /// <summary>
        /// 查询医生规则 page
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorRules>> DoctorRulesPage(DoctorRulesIn DoctorRulesInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(DoctorRulesInfo.Expression);
            //var queryclinicrel = _clireltorrepository.GetAll();
            //var querydoctor = _doctorrepository.GetAll();
            //var queryuser = _userrepository.GetAll();

            //var querylist = from a in query
            //join b in queryclinicrel on a.ApplyClinicID equals b.ClinicID
            //join c in querydoctor on b.DoctorID equals c.Id
            //join d in queryuser on c.UserID equals d.Id
            //where !a.IsDelete && !b.IsDelete && !c.IsDelete && !d.IsDelete && d.Id == DoctorRulesInfo.userid
            //select new DoctorRulesView
            //{
            //    CreatedOnUtc = a.CreatedOn.ToString("yyyy-MM-ddTHH:mm:ss"),
            //    Id=a.Id,
            //    RulesType = a.RulesType,
            //    RulesTitle = a.RulesTitle,
            //    RulesContent = a.RulesContent,
            //    ApplyClinicName = a.ApplyClinicName,
            //    ImageFie = a.ImageFie,
            //};
           
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorRulesInfo.MaxResultCount;
            var DoctorRulesList = await query.OrderByDescending(t=>t.CreatedOn).PageBy(DoctorRulesInfo.SkipTotal, DoctorRulesInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorRules>(tasksCount, DoctorRulesList.MapTo<List<DoctorRules>>());
        }
        /// <summary>
        /// 新建医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRules> CreateDoctorRules(DoctorRules DoctorRulesInfo)
        {
            DoctorRulesInfo.Id = await _repository.InsertAndGetIdAsync(DoctorRulesInfo);
            return DoctorRulesInfo;
        }

        /// <summary>
        /// 修改医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRules> UpdateDoctorRules(DoctorRules DoctorRulesInfo)
        {
            return await _repository.UpdateAsync(DoctorRulesInfo);
        }

        /// <summary>
        /// 删除医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRules> DeleteDoctorRules(DoctorRules DoctorRulesInfo)
        {
            return await _repository.UpdateAsync(DoctorRulesInfo);
        }
    }
}
