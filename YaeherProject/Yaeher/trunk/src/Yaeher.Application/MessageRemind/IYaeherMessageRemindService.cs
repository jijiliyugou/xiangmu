using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.MessageRemind.Dto;

namespace Yaeher.MessageRemind
{
    /// <summary>
    /// 短信对接
    /// </summary>
    public interface IYaeherMessageRemindService : IApplicationService
    {
        /// <summary>
        /// 新建短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        Task<YaeherMessageRemind> CreateYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo);
        /// <summary>
        /// 删除短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        Task<YaeherMessageRemind> DeleteYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo);
        /// <summary>
        /// 修改短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        Task<YaeherMessageRemind> UpdateYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo);
        /// <summary>
        /// 查询短信对接byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherMessageRemind> YaeherMessageRemindByID(int Id);
        /// <summary>
        /// 查询短信对接 List
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherMessageRemind>> YaeherMessageRemindList(YaeherMessageRemindIn YaeherMessageRemindInfo);
        /// <summary>
        /// 查询短信对接 page
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherMessageRemind>> YaeherMessageRemindPage(YaeherMessageRemindIn YaeherMessageRemindInfo);
    }
}