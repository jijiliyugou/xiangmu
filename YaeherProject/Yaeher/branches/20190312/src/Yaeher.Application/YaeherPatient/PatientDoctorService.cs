using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System.Linq.Expressions;
using System;

namespace Yaeher
{
    /// <summary>
    /// 我的医生
    /// </summary>
    public class PatientDoctorService : IPatientDoctorService
    {
        private readonly IRepository<YaeherPatientDoctor> _repository;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="repository"></param>
        public PatientDoctorService(IRepository<YaeherPatientDoctor> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 我的医生 List
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherPatientDoctor>> YaeherPatientDoctorList(YaeherPatientDoctorIn YaeherPatientDoctorInfo)
        {
            var YaeherPatientDoctors = await _repository.GetAllListAsync(YaeherPatientDoctorInfo.Expression);
            return YaeherPatientDoctors.ToList();
        }

        /// <summary>
        /// 我的医生byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientDoctor> YaeherPatientDoctorByID(int Id)
        {
            var YaeherPatientDoctors = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherPatientDoctors;
        }
        /// <summary>
        /// 我的医生byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientDoctor> YaeherPatientDoctorByExpression(Expression<Func<YaeherPatientDoctor, bool>> whereExpression)
        {
            var YaeherPatientDoctors = await _repository.FirstOrDefaultAsync(whereExpression);
            return YaeherPatientDoctors;
        }
        /// <summary>
        /// 我的医生 page
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherPatientDoctor>> YaeherPatientDoctorPage(YaeherPatientDoctorIn YaeherPatientDoctorInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherPatientDoctorInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherPatientDoctorInfo.MaxResultCount;
            var YaeherPatientDoctorList = await query.PageBy(YaeherPatientDoctorInfo.SkipTotal, YaeherPatientDoctorInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherPatientDoctor>(tasksCount, YaeherPatientDoctorList.MapTo<List<YaeherPatientDoctor>>());
        }
        /// <summary>
        /// 新建我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientDoctor> CreateYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo)
        {
            YaeherPatientDoctorInfo.Id=await _repository.InsertAndGetIdAsync(YaeherPatientDoctorInfo);
            return YaeherPatientDoctorInfo;
        }

        /// <summary>
        /// 修改我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientDoctor> UpdateYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo)
        {
            return await _repository.UpdateAsync(YaeherPatientDoctorInfo);
        }

        /// <summary>
        /// 删除我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientDoctor> DeleteYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo)
        {
            return await _repository.UpdateAsync(YaeherPatientDoctorInfo);
        }


    }
}
