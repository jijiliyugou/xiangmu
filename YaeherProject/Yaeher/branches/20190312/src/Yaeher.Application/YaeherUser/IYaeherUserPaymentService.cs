using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 用户支付方式
    /// </summary>
    public interface IYaeherUserPaymentService : IApplicationService
    {
        /// <summary>
        /// 新建用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> CreateYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo);
        /// <summary>
        /// 删除用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> DeleteYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo);

        /// <summary>
        /// 修改用户支付方式
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> UpdateYaeherUserPayment(YaeherUserPayment YaeherUserPaymentInfo);
        /// <summary>
        /// 用户支付方式byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> YaeherUserPaymentByID(int Id);
        /// <summary>
        /// 用户支付方式bywhereExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> YaeherUserPaymentExpression(Expression<Func<YaeherUserPayment, bool>> whereExpression); 
        /// <summary>
        /// 用户支付方式byDocId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherUserPayment> YaeherUserPaymentByUserID(int Id);
        /// <summary>
        /// 用户支付方式 List
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherUserPayment>> YaeherUserPaymentList(YaeherUserPaymentIn YaeherUserPaymentInfo);
        /// <summary>
        /// 用户支付方式 page
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherUserPayment>> YaeherUserPaymentPage(YaeherUserPaymentIn YaeherUserPaymentInfo);
    }
}