using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 问答
    /// </summary>
    public interface IQuestionReleaseService : IApplicationService
    {
        /// <summary>
        /// 新建问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        Task<QuestionRelease> CreateQuestionRelease(QuestionRelease QuestionReleaseInfo);
        /// <summary>
        /// 删除问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        Task<QuestionRelease> DeleteQuestionRelease(QuestionRelease QuestionReleaseInfo);
        /// <summary>
        /// 查询问答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QuestionRelease> QuestionReleaseByID(int Id);
        /// <summary>
        /// 查询问答byExpress
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<QuestionRelease> QuestionReleaseByExpress(Expression<Func<QuestionRelease, bool>> whereExpression);

        /// <summary>
        /// 查询问答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QuestionRelease> PatientQuestionReleaseByID(int Id);
        /// <summary>
        /// 查询问答 List
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        Task<IList<QuestionRelease>> QuestionReleaseList(QuestionReleaseIn QuestionReleaseInfo);
        /// <summary>
        /// 查询问答 page
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QuestionRelease>> QuestionReleasePage(QuestionReleaseIn QuestionReleaseInfo);
        /// <summary>
        /// 修改问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        Task<QuestionRelease> UpdateQuestionRelease(QuestionRelease QuestionReleaseInfo);
    }
}