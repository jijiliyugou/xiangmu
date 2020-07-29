using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.Consultation.Dto;
using System;

namespace Yaeher.Consultation
{

    /// <summary>
    /// 咨询评分
    /// </summary>
    public class ConsultationEvaluationService : IConsultationEvaluationService
    {
        private readonly IRepository<ConsultationEvaluation> _repository;
        /// <summary>
        ///  咨询评分
        /// </summary>
        /// <param name="repository"></param>
        public ConsultationEvaluationService(IRepository<ConsultationEvaluation> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 咨询评分 List
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<ConsultationEvaluation>> ConsultationEvaluationList(ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            try
            {
                var ConsultationEvaluations = await _repository.GetAllListAsync(ConsultationEvaluationInfo.Expression);
                return ConsultationEvaluations.ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 咨询评分byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationEvaluation> ConsultationEvaluationByID(int Id)
        {
            var ConsultationEvaluations = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ConsultationEvaluations;
        }
        /// <summary>
        /// 咨询评分ByNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationEvaluation> ConsultationEvaluationByNumber(string ConsultNumber)
        {
            var ConsultationEvaluations = await _repository.FirstOrDefaultAsync(t => t.ConsultNumber == ConsultNumber && !t.IsDelete);
            return ConsultationEvaluations;
        }
        /// <summary>
        /// 咨询评分 page
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ConsultationEvaluation>> ConsultationEvaluationPage(ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(t=>t.CreatedOn).Where(ConsultationEvaluationInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ConsultationEvaluationInfo.MaxResultCount;
            var ConsultationEvaluationList = await query.PageBy(ConsultationEvaluationInfo.SkipTotal, ConsultationEvaluationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ConsultationEvaluation>(tasksCount, ConsultationEvaluationList.MapTo<List<ConsultationEvaluation>>());
        }
        /// <summary>
        /// 新建咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationEvaluation> CreateConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo)
        {
            ConsultationEvaluationInfo.Id= await _repository.InsertAndGetIdAsync(ConsultationEvaluationInfo);
            return ConsultationEvaluationInfo;
        }

        /// <summary>
        /// 修改咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationEvaluation> UpdateConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo)
        {
            return await _repository.UpdateAsync(ConsultationEvaluationInfo);
        }

        /// <summary>
        /// 删除咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationEvaluation> DeleteConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo)
        {
            return await _repository.UpdateAsync(ConsultationEvaluationInfo);
        }

    }
}
