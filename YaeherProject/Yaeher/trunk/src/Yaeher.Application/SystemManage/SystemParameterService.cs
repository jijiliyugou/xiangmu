using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SystemParameterService : ISystemParameterService
    {
        private readonly IRepository<SystemParameter> _repository;
        private readonly IRepository<DoctorParaSet> _Setrepository;
        /// <summary>
        /// 系统参数 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="Setrepository"></param>
        public SystemParameterService(IRepository<SystemParameter> repository, IRepository<DoctorParaSet> Setrepository)
        {
            _repository = repository;
            _Setrepository = Setrepository;
        }
        /// <summary>
        /// 查询系统参数 List
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<SystemParameter>> SystemParameterList(SystemParameterIn SystemParameterInfo)
        {
            var SystemParameters = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SystemParameterInfo.Expression);
            return await SystemParameters.ToListAsync();
        }

        /// <summary>
        /// 查询系统参数byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemParameter> SystemParameterByID(int Id)
        {
            var SystemParameters = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return SystemParameters;
        }
        /// <summary>
        /// 查询系统参数 page
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<SystemParameter>> SystemParameterPage(SystemParameterIn SystemParameterInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SystemParameterInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / SystemParameterInfo.MaxResultCount;
            var SystemParameterList = await query.PageBy(SystemParameterInfo.SkipTotal, SystemParameterInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<SystemParameter>(tasksCount, SystemParameterList.MapTo<List<SystemParameter>>());
        }
        /// <summary>
        /// 新建系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemParameter> CreateSystemParameter(SystemParameter SystemParameterInfo)
        {
            SystemParameterInfo.Id = await _repository.InsertAndGetIdAsync(SystemParameterInfo);
            return SystemParameterInfo;
        }

        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemParameter> UpdateSystemParameter(SystemParameter SystemParameterInfo)
        {
            return await _repository.UpdateAsync(SystemParameterInfo);
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemParameter> DeleteSystemParameter(SystemParameter SystemParameterInfo)
        {
            return await _repository.UpdateAsync(SystemParameterInfo);
        }
        /// <summary>
        /// 患者端用
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<SystemParameter>> PatientParameterList(SystemParameterIn SystemParameterInfo)
        {
            //参数类型 ConfigPar配置参数 SetPar 系统参数
            if (SystemParameterInfo.Type == "ConfigPar")
            {
                var SystemParameters = await _repository.GetAllListAsync(t => !t.IsDelete && t.SystemCode == SystemParameterInfo.SystemCode);
                return SystemParameters.ToList();
            }
            else
            {
                List<SystemParameter> SystemParameterList = new List<SystemParameter>();
                var doctorParaSets = await _Setrepository.GetAllListAsync(a => a.DoctorParaSetCode == SystemParameterInfo.SystemCode && a.IsDelete == false);
                if (doctorParaSets.Count > 0)
                {
                    foreach (var doctorParaSet in doctorParaSets)
                    {
                        SystemParameter systemParameter = new SystemParameter();
                        systemParameter.SystemType = doctorParaSet.DoctorParaSetCode;
                        systemParameter.SystemCode = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Code = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Name = doctorParaSet.DoctorParaSetName;
                        systemParameter.ItemValue = doctorParaSet.ItemValue;
                        systemParameter.Remark = doctorParaSet.DoctorParaSetName;
                        SystemParameterList.Add(systemParameter);
                    }
                }
                return SystemParameterList.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<SystemParameter>> ParameterList(SystemParameterIn SystemParameterInfo)
        {
            if (!string.IsNullOrEmpty(SystemParameterInfo.SystemType))
            {
                SystemParameterInfo.AndAlso(t => !t.SystemCode.Contains(SystemParameterInfo.SystemType));
            }
            //参数类型 ConfigPar配置参数 SetPar 系统参数
            if (SystemParameterInfo.Type == "ConfigPar")
            {
                var SystemParameters = await _repository.GetAllListAsync(SystemParameterInfo.Expression);
                return SystemParameters.ToList();
            }
            else
            {
                List<SystemParameter> SystemParameterList = new List<SystemParameter>();
                var doctorParaSets = await _Setrepository.GetAllListAsync(a => a.DoctorParaSetCode == SystemParameterInfo.SystemType && a.IsDelete == false);
                if (doctorParaSets.Count > 0)
                {
                    foreach (var doctorParaSet in doctorParaSets)
                    {
                        SystemParameter systemParameter = new SystemParameter();
                        systemParameter.SystemType = doctorParaSet.DoctorParaSetCode;
                        systemParameter.SystemCode = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Code = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Name = doctorParaSet.DoctorParaSetName;
                        systemParameter.ItemValue = doctorParaSet.ItemValue;
                        systemParameter.Remark = doctorParaSet.DoctorParaSetName;
                        SystemParameterList.Add(systemParameter);
                    }
                }
                return SystemParameterList.ToList();
            }
        }

    }
}
