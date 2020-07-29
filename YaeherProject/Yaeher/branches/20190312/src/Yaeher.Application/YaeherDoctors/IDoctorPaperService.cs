using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生发布文章
    /// </summary>
    public interface IDoctorPaperService: IApplicationService
    {
        /// <summary>
        /// 新建医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<DoctorPaper> CreateDoctorPaper(DoctorPaper DoctorPaperInfo);
        /// <summary>
        /// 删除医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<DoctorPaper> DeleteDoctorPaper(DoctorPaper DoctorPaperInfo);
        /// <summary>
        /// 查询医生发布文章byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorPaper> DoctorPaperByID(int Id);
        /// <summary>
        /// 查询医生发布文章 List
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorPaper>> DoctorPaperList(DoctorPaperIn DoctorPaperInfo);
        /// <summary>
        /// 查询医生发布文章 List
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorPaperView>> DoctorPaperViewList(DoctorPaperIn DoctorPaperInfo);
        /// <summary>
        /// 查询医生发布文章 page
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorPaper>> DoctorPaperPage(DoctorPaperIn DoctorPaperInfo);
        /// <summary>
        /// 查询医生发布文章 page
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorPaperView>> DoctorPaperViewPage(DoctorPaperIn DoctorPaperInfo);
        /// <summary>
        /// 修改医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        Task<DoctorPaper> UpdateDoctorPaper(DoctorPaper DoctorPaperInfo);
    }
}