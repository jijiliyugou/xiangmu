using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;

namespace Yaeher
{
    /// <summary>
    /// 分享医生
    /// </summary>
    public class ShareDoctorService : IShareDoctorService
    {
        private readonly IRepository<ShareDoctor> _repository;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="repository"></param>
        public ShareDoctorService(IRepository<ShareDoctor> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询分享医生 List
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ShareDoctor>> ShareDoctorList(ShareDoctorIn ShareDoctorInfo)
        {
            var ShareDoctors = await _repository.GetAllListAsync(ShareDoctorInfo.Expression);
            return ShareDoctors.ToList();
        }

        /// <summary>
        /// 查询分享医生byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ShareDoctor> ShareDoctorByID(int Id)
        {
            var ShareDoctors = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ShareDoctors;
        }
        /// <summary>
        /// 查询分享医生 page
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ShareDoctor>> ShareDoctorPage(ShareDoctorIn ShareDoctorInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ShareDoctorInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ShareDoctorInfo.MaxResultCount;
            var ShareDoctorList = await query.PageBy(ShareDoctorInfo.SkipTotal, ShareDoctorInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ShareDoctor>(tasksCount, ShareDoctorList.MapTo<List<ShareDoctor>>());
        }
        /// <summary>
        /// 新建分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ShareDoctor> CreateShareDoctor(ShareDoctor ShareDoctorInfo)
        {
            ShareDoctorInfo.Id= await _repository.InsertAndGetIdAsync(ShareDoctorInfo);
            return ShareDoctorInfo;
        }

        /// <summary>
        /// 修改分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ShareDoctor> UpdateShareDoctor(ShareDoctor ShareDoctorInfo)
        {
            return await _repository.UpdateAsync(ShareDoctorInfo);
        }

        /// <summary>
        /// 删除分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ShareDoctor> DeleteShareDoctor(ShareDoctor ShareDoctorInfo)
        {
            return await _repository.UpdateAsync(ShareDoctorInfo);
        }

    }
}
