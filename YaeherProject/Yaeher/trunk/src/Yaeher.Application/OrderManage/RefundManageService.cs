using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System;

namespace Yaeher
{
    /// <summary>
    /// 订单退单管理
    /// </summary>
    public class RefundManageService : IRefundManageService
    {
        private readonly IRepository<RefundManage> _repository;
        private readonly IRepository<YaeherConsultation> _consrepository;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="repository"></param>
        public RefundManageService(IRepository<RefundManage> repository, IRepository<YaeherConsultation> conrepository)
        {
            _repository = repository;
            _consrepository=conrepository;
        }

        /// <summary>
        /// 查询订单退单管理 List
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<RefundManage>> RefundManageList(RefundManageIn RefundManageInfo)
        {
            var RefundManages = await _repository.GetAllListAsync(RefundManageInfo.Expression);
            return RefundManages.ToList();
        }
        /// <summary>
        /// 查询订单退单管理 List
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<RefundManage>> PayCheckRefundManageList(RefundManageIn RefundManageInfo)
        {
            var datetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var RefundManages =  _repository.GetAll().Where(RefundManageInfo.Expression);
            
            var consul =  _consrepository.GetAll().Where(t=>t.IsDelete==false&&t.DoctorID==RefundManageInfo.DoctorID&&t.ServiceMoneyListId==RefundManageInfo.ServiceID);
            var query = from a in RefundManages
                        join b in consul on a.ConsultNumber equals b.ConsultNumber
                        select a;
            return await query.ToListAsync();
        }
        
        /// <summary>
        /// 查询订单退单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> RefundManageByID(int Id)
        {
            var RefundManages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return RefundManages;
        }
        /// <summary>
        /// 查询订单退单管理bynumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> RefundManageByNumber(string number)
        {
            var RefundManages = await _repository.FirstOrDefaultAsync(t => t.IsDelete==false&& t.RefundNumber == number);
            return RefundManages;
        }
        
        /// <summary>
        /// 查询订单退单管理by咨询Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> RefundManageByConsulID(int Id)
        {
            var RefundManages = await _repository.FirstOrDefaultAsync(t => t.ConsultID == Id && !t.IsDelete);
            return RefundManages;
        }
        

        /// <summary>
        /// 查询订单退单管理 page
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<RefundManage>> RefundManagePage(RefundManageIn RefundManageInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(RefundManageInfo.Expression);
            var cons = _consrepository.GetAll().Where(t=>!t.IsDelete);

            if (string.IsNullOrEmpty(RefundManageInfo.RefundType))
            {
                //获取总数
                var tasksCount = query1.Count();
                //获取总数
                var totalpage = tasksCount / RefundManageInfo.MaxResultCount;
                var RefundManageList = await query1.PageBy(RefundManageInfo.SkipTotal, RefundManageInfo.MaxResultCount).ToListAsync();
                return new PagedResultDto<RefundManage>(tasksCount, RefundManageList.MapTo<List<RefundManage>>());
            }
            else
            {
                var query = from a in query1
                            join b in cons on a.ConsultNumber equals b.ConsultNumber
                            where b.RefundType == RefundManageInfo.RefundType
                            select a;

                //获取总数
                var tasksCount = query.Count();
                //获取总数
                var totalpage = tasksCount / RefundManageInfo.MaxResultCount;
                var RefundManageList = await query.PageBy(RefundManageInfo.SkipTotal, RefundManageInfo.MaxResultCount).ToListAsync();
                return new PagedResultDto<RefundManage>(tasksCount, RefundManageList.MapTo<List<RefundManage>>());
            }
        }
        /// <summary>
        /// 查询订单退单管理 page
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<RefundManage>> CheckRefundManagePage(RefundManageIn RefundManageInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().OrderByDescending(a => a.CreatedOn).OrderByDescending(t=>t.CheckState).Where(RefundManageInfo.Expression);
            //获取总数
            var tasksCount = query1.Count();
            //获取总数
            var totalpage = tasksCount / RefundManageInfo.MaxResultCount;
            var RefundManageList = await query1.PageBy(RefundManageInfo.SkipTotal, RefundManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<RefundManage>(tasksCount, RefundManageList.MapTo<List<RefundManage>>());
        }

        


        /// <summary>
        /// 新建订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> CreateRefundManage(RefundManage RefundManageInfo)
        {
            RefundManageInfo.Id= await _repository.InsertAndGetIdAsync(RefundManageInfo);
            return RefundManageInfo;
        }

        /// <summary>
        /// 修改订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> UpdateRefundManage(RefundManage RefundManageInfo)
        {
            return  await _repository.UpdateAsync(RefundManageInfo);
        }

        /// <summary>
        /// 删除订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RefundManage> DeleteRefundManage(RefundManage RefundManageInfo)
        {
            return await _repository.UpdateAsync(RefundManageInfo);
            
        }

    }
}
