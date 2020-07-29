using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.LableManages.Dto;

namespace Yaeher.LableManages
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class LableManageService : ILableManageService
    {
        private readonly IRepository<LableManage> _repository;
        private readonly IRepository<ClinicLableReltion> _relrepository;
        private readonly IRepository<DoctorRelation> _docrelrepository;
        private readonly IRepository<YaeherDoctor> _docrepository;

        /// <summary>
        /// 标签管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="relrepository"></param>
        /// <param name="docrelrepository"></param>
        /// <param name="docrepository"></param>
        public LableManageService(IRepository<LableManage> repository,
            IRepository<ClinicLableReltion> relrepository, 
            IRepository<DoctorRelation> docrelrepository, 
            IRepository<YaeherDoctor> docrepository)
        {
            _repository = repository;
            _relrepository = relrepository;
            _docrelrepository = docrelrepository;
            _docrepository = docrepository;
        }

        /// <summary>
        /// 查询科室标签管理 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LabelClinicManage>> ClinicLableManageList()
        {
            var relation = _relrepository.GetAll();
            var label = _repository.GetAll();
            var querylist = from a in relation
                            join b in label on a.LableID equals b.Id
                            where !a.IsDelete && !b.IsDelete
                            select new LabelClinicManage
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy=b.CreatedBy,
                                ModifyBy=b.ModifyBy,
                                ModifyOn=b.ModifyOn,
                                LableName=b.LableName,
                                LableRemark=b.LableRemark,
                                ClinicID=a.ClinicID,
                                OrderSort=b.OrderSort,
                            };
            return await querylist.OrderBy(t=>t.OrderSort).ThenByDescending(t=>t.CreatedOn).ToListAsync();
        }
        /// <summary>
        /// 查询某科室标签管理 List
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LabelClinicManage>> LableClinicManageInList(LabelClinicManageIn input)
        {
            var relation = _relrepository.GetAll();
            var label = _repository.GetAll();
            var querylist = from a in relation
                            join b in label on a.LableID equals b.Id
                            where !a.IsDelete && !b.IsDelete&&a.ClinicID==input.ClinicID
                            select new LabelClinicManage
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy = b.CreatedBy,
                                ModifyBy = b.ModifyBy,
                                ModifyOn = b.ModifyOn,
                                LableName = b.LableName,
                                LableRemark = b.LableRemark,
                                ClinicID = a.ClinicID,
                                OrderSort=b.OrderSort,
                            };
            return await querylist.OrderBy(t=>t.OrderSort).ThenByDescending(t=>t.CreatedOn).ToListAsync();
        }
        /// <summary>
        /// 查询某医生标签管理 List
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LabelDoctorManage>> LableDoctorManageInList(LabelDoctorManageIn input)
        {
            var relation = _docrelrepository.GetAll();
            var label = _repository.GetAll();
            var doc = _docrepository.GetAll();
            var querylist = from a in relation
                            join b in label on a.LableID equals b.Id
                            join c in doc on a.DoctorID equals c.Id
                            where !a.IsDelete && !b.IsDelete && a.DoctorID == input.DoctorID&&c.CheckRes=="success" && c.AuthCheckRes == "success"
                            select new LabelDoctorManage
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy = b.CreatedBy,
                                ModifyBy = b.ModifyBy,
                                ModifyOn = b.ModifyOn,
                                LableName = b.LableName,
                                LableRemark = b.LableRemark,
                                DoctorID = a.DoctorID,
                                Id=b.Id,
                            };
            return await querylist.ToListAsync();
        }

      
        
        /// <summary>
        ///  查询标签管理 List
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LableManage>> LableManageList(LableManageIn LableManageInfo)
        {
            var LableManages = await _repository.GetAllListAsync(LableManageInfo.Expression);
            return LableManages.ToList();
        }
        /// <summary>
        /// 查询标签管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<LableManage> LableManageByID(int Id)
        {
            return await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
        }
        /// <summary>
        /// 查询标签管理byName
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<LableManage> LableManageByName(LableManageIn LableManageInfo)
        {
            return await _repository.FirstOrDefaultAsync(LableManageInfo.Expression);
        }
        
        /// <summary>
        /// 查询标签管理 page
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<LableManage>> LableManagePage(LableManageIn LableManageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(LableManageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / LableManageInfo.MaxResultCount;
            var LableManageList = await query.PageBy(LableManageInfo.SkipTotal, LableManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<LableManage>(tasksCount, LableManageList.MapTo<List<LableManage>>());
        }
        /// <summary>
        /// 查询医生标签管理 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LabelDoctorManage>> DoctorLableManageList()
        {
            var relation = _docrelrepository.GetAll();
            var label = _repository.GetAll();
            var doc = _docrepository.GetAll();
            var querylist = from a in relation
                            join b in label on a.LableID equals b.Id
                            join c in doc on a.DoctorID equals c.Id
                            where !a.IsDelete && !b.IsDelete&&c.CheckRes=="success" && c.AuthCheckRes == "success"
                            select new LabelDoctorManage
                            {
                                CreatedOn = b.CreatedOn,
                                CreatedBy = b.CreatedBy,
                                ModifyBy = b.ModifyBy,
                                ModifyOn = b.ModifyOn,
                                LableName = b.LableName,
                                LableRemark = b.LableRemark,
                                DoctorID = a.DoctorID,
                            };
            return await querylist.ToListAsync();
        }
        /// <summary>
        /// 新建标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<LableManage> CreateLableManage(LableManage LableManageInfo)
        {
            LableManageInfo.Id= await _repository.InsertAndGetIdAsync(LableManageInfo);
            return LableManageInfo;
        }

        /// <summary>
        /// 修改标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<LableManage> UpdateLableManage(LableManage LableManageInfo)
        {
            return await _repository.UpdateAsync(LableManageInfo);
        }

        /// <summary>
        /// 删除标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<LableManage> DeleteLableManage(LableManage LableManageInfo)
        {
            return await _repository.UpdateAsync(LableManageInfo);
        }
    }
}
