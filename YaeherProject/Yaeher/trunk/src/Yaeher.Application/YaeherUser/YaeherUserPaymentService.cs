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
    /// 用户支付方式
    /// </summary>
    public class YaeherUserPaymentService : IYaeherUserPaymentService
    {
        private readonly IRepository<YaeherUserPayment> _repository;
        /// <summary>
        /// 用户支付方式 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherUserPaymentService(IRepository<YaeherUserPayment> repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// 用户支付方式 List
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherUserPayment>> YaeherUserPaymentList(YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            var YaeherUserPayments = await _repository.GetAllListAsync(YaeherUserPaymentInfo.Expression);
            return YaeherUserPayments.ToList();
        }

        /// <summary>
        /// 用户支付方式byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> YaeherUserPaymentByID(int Id)
        {
            var YaeherUserPayments = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherUserPayments;
        }

        /// <summary>
        /// 用户支付方式byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> YaeherUserPaymentExpression(Expression<Func<YaeherUserPayment, bool>> whereExpression)
        {
            var UserPayment = await _repository.FirstOrDefaultAsync(whereExpression);
            return UserPayment;
        }
        /// <summary>
        /// 用户支付方式byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> YaeherUserPaymentByUserID(int Id)
        {
           // var YaeherUserPayments = await _repository.FirstOrDefaultAsync(t => t.UserID == Id && t.IsDefault&& !t.IsDelete);
           //更改注意
            var YaeherUserPayments = await _repository.FirstOrDefaultAsync(t => t.UserID == Id && !t.IsDelete);
            return YaeherUserPayments;
        }
        

        /// <summary>
        /// 用户支付方式 page
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherUserPayment>> YaeherUserPaymentPage(YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderBy(a => a.CreatedOn).Where(YaeherUserPaymentInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherUserPaymentInfo.MaxResultCount;
            var YaeherUserPaymentList = await query.PageBy(YaeherUserPaymentInfo.SkipTotal, YaeherUserPaymentInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherUserPayment>(tasksCount, YaeherUserPaymentList.MapTo<List<YaeherUserPayment>>());
        }
        /// <summary>
        /// 新建用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> CreateYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo)
        {
            YaeherUserPaymentInfo.Id= await _repository.InsertAndGetIdAsync(YaeherUserPaymentInfo);
            return YaeherUserPaymentInfo;
        }

        /// <summary>
        /// 修改用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> UpdateYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo)
        {
            return await _repository.UpdateAsync(YaeherUserPaymentInfo);
        }

        /// <summary>
        /// 删除用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserPayment> DeleteYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo)
        {
            return await _repository.UpdateAsync(YaeherUserPaymentInfo);
        }
    }
}
