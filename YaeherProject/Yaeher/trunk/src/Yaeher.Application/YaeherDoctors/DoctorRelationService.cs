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
using Yaeher.LableManages.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生与标签关系
    /// </summary>
    public class DoctorRelationService : IDoctorRelationService
    {
        private readonly IRepository<DoctorRelation> _repository;
        private readonly IRepository<LableManage> _labelrepository;
        private readonly IRepository<ClinicDoctorReltion> _clinicrepository;
        /// <summary>
        /// 医生与标签关系 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="labelrepository"></param>
        /// <param name="clinicrepository"></param>
        public DoctorRelationService(IRepository<DoctorRelation> repository, IRepository<LableManage>  labelrepository,IRepository<ClinicDoctorReltion> clinicrepository)
        {
            _repository = repository;
            _labelrepository = labelrepository;
            _clinicrepository = clinicrepository;
        }
        /// <summary>
        /// 查询医生与标签关系 List
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorRelation>> DoctorRelationList(DoctorRelationIn DoctorRelationInfo)
        {
            var DoctorRelations = await _repository.GetAll().Where(DoctorRelationInfo.Expression).OrderBy(t=>t.CreatedOn).ToListAsync();
            return DoctorRelations.ToList();
        }
        /// <summary>
        /// 查询标签与科室关系 List
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorRelation>> DoctorClinicRelationList(DoctorRelationIn DoctorRelationInfo)
        {
            var DoctorRelations = await _repository.GetAllListAsync(DoctorRelationInfo.Expression);
            //var clinic = _clinicrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in DoctorRelations
                        //join b in clinic on a.DoctorID equals b.DoctorID
                        //where b.ClinicID == DoctorRelationInfo.ClinicID
                        select new DoctorRelation
                        {
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            LableID = a.LableID,
                            LableName = a.LableName,
                            LableJSON = a.LableJSON,
                            CreatedBy = a.CreatedBy,
                            CreatedOn = a.CreatedOn,
                        };

            return query.ToList();
        }
        

        /// <summary>
        /// 查询医生与标签关系byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRelation> DoctorRelationByID(int Id)
        {
            var DoctorRelations = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorRelations;
        }
        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorRelation>> DoctorRelationPage(DoctorRelationIn DoctorRelationInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(DoctorRelationInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorRelationInfo.MaxResultCount;
            var DoctorRelationList = await query.PageBy(DoctorRelationInfo.SkipTotal, DoctorRelationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorRelation>(tasksCount, DoctorRelationList.MapTo<List<DoctorRelation>>());
        }
        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorRelation>> LabelDoctorRelationPage(DoctorRelationIn DoctorRelationInfo)
        {
            //初步过滤
            var querys = _repository.GetAll().Where(DoctorRelationInfo.Expression);
            var label = _labelrepository.GetAll().Where(t => !t.IsDelete);

            var query = from a in querys
                        join b in label on a.LableID equals b.Id
                        where a.DoctorID == DoctorRelationInfo.DoctorID
                        select new DoctorRelation
                        {
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            LableID = a.LableID,
                            LableName = a.LableName,
                            CreatedBy = a.CreatedBy,
                            CreatedOn = a.CreatedOn,
                            Id=a.Id,
                        };
            
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorRelationInfo.MaxResultCount;
            var DoctorRelationList = await query.PageBy(DoctorRelationInfo.SkipTotal, DoctorRelationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorRelation>(tasksCount, DoctorRelationList.MapTo<List<DoctorRelation>>());
        }
        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="lableManageIn"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorLale>> LabelDoctorRelationPage(LableManageIn lableManageIn)
        {
            //初步过滤
            var querys = _repository.GetAll().Where(t=>!t.IsDelete);
            var label = _labelrepository.GetAll().Where(lableManageIn.Expression);
            var query = from a in querys
                        join b in label on a.LableID equals b.Id
                        where a.DoctorID == lableManageIn.DoctorID
                        select new DoctorLale
                        {
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            LableID = a.Id,
                            LableName = a.LableName,
                            CreatedBy = a.CreatedBy,
                            CreatedOn = a.CreatedOn,
                            Id = b.Id,
                            OrderSort=b.OrderSort,
                            LableRemark=b.LableRemark,
                        };

            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / lableManageIn.MaxResultCount;
            var DoctorRelationList = await query.OrderBy(t=>t.CreatedOn).PageBy(lableManageIn.SkipTotal, lableManageIn.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorLale>(tasksCount, DoctorRelationList.MapTo<List<DoctorLale>>());
        }

        
        /// <summary>
        /// 新建医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRelation> CreateDoctorRelation(DoctorRelation DoctorRelationInfo)
        {
            DoctorRelationInfo.Id= await _repository.InsertAndGetIdAsync(DoctorRelationInfo);
            return DoctorRelationInfo;
        }

        /// <summary>
        /// 修改医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRelation> UpdateDoctorRelation(DoctorRelation DoctorRelationInfo)
        {
            return await _repository.UpdateAsync(DoctorRelationInfo);
        }

        /// <summary>
        /// 删除医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorRelation> DeleteDoctorRelation(DoctorRelation DoctorRelationInfo)
        {
            return await _repository.UpdateAsync(DoctorRelationInfo);
        }
    }
}
