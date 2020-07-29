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

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生费用表
    /// </summary>
    public class ServiceMoneyListService : IServiceMoneyListService
    {
        private readonly IRepository<ServiceMoneyList> _repository;
        /// <summary>
        /// 医生费用表 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public ServiceMoneyListService(IRepository<ServiceMoneyList> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生费用表 List
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ServiceMoneyList>> ServiceMoneyListList(ServiceMoneyListIn ServiceMoneyListInfo)
        {
            var ServiceMoneyLists = await _repository.GetAllListAsync(ServiceMoneyListInfo.Expression);
            return ServiceMoneyLists.ToList();
        }
        /// <summary>
        /// 查询医生费用表 List
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ServiceMoneyStateList>> ServiceMoneyStateList(ServiceMoneyListIn ServiceMoneyListInfo)
        {
            var ServiceMoneyLists = await _repository.GetAllListAsync(ServiceMoneyListInfo.Expression);
            var query = from a in ServiceMoneyLists
                        select new ServiceMoneyStateList
                        {
                            DoctorName=a.DoctorName,
                            DoctorID = a.DoctorID,
                            ServiceType = a.ServiceType,
                            ServiceTypeValue = a.ServiceTypeValue,
                            ServiceDuration = a.ServiceDuration,
                            ServiceExpense = a.ServiceExpense   ,
                            ServiceFrequency = a.ServiceFrequency,
                            ServiceState = a.ServiceState,
                            ServiceTime = a.ServiceTime,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                            ReceiptState=a.ServiceState,
                        };
            return query.OrderByDescending(t=>t.ServiceType).ToList();
        }
        /// <summary>
        /// 查询医生费用表byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ServiceMoneyList> ServiceMoneyListByID(int Id)
        {
            var ServiceMoneyLists = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ServiceMoneyLists;
        }
        /// <summary>
        /// 查询医生费用表 page
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ServiceMoneyList>> ServiceMoneyListPage(ServiceMoneyListIn ServiceMoneyListInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ServiceMoneyListInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ServiceMoneyListInfo.MaxResultCount;
            var ServiceMoneyListList = await query.PageBy(ServiceMoneyListInfo.SkipTotal, ServiceMoneyListInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ServiceMoneyList>(tasksCount, ServiceMoneyListList.MapTo<List<ServiceMoneyList>>());
        }
        /// <summary>
        /// 新建医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ServiceMoneyList> CreateServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo)
        {
            ServiceMoneyListInfo.Id= await _repository.InsertAndGetIdAsync(ServiceMoneyListInfo);
            return ServiceMoneyListInfo;
        }

        /// <summary>
        /// 修改医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ServiceMoneyList> UpdateServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo)
        {
            return await _repository.UpdateAsync(ServiceMoneyListInfo);
        }

        /// <summary>
        /// 删除医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ServiceMoneyList> DeleteServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo)
        {
            return await _repository.UpdateAsync(ServiceMoneyListInfo);
        }
    }
}
