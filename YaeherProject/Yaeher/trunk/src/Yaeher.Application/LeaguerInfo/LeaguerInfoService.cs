using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Yaeher
{
    /// <summary>
    /// 患者成员信息
    /// </summary>
    public class LeaguerInfoService : ILeaguerInfoService
    {
        private readonly IRepository<YaeherPatientLeaguerInfo> _leaguerRepository;
        /// <summary>
        /// 患者成员信息 构造函数
        /// </summary>
        /// <param name="leaguermodel"></param>
        public LeaguerInfoService(IRepository<YaeherPatientLeaguerInfo> leaguermodel)
        {
            _leaguerRepository = leaguermodel;
        }
        /// <summary>
        /// 获取患者成员List信息
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherPatientLeaguerInfo>> LeaguerInfoList(LeaguerInfo Leaguer)
        {
            return await _leaguerRepository.GetAllListAsync(Leaguer.Expression);
        }
        /// <summary>
        /// 获取患者成员List信息
        /// </summary>
        /// <param name="LeaguerQueryIn"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherPatientLeaguerInfo>> QueryLeaguerInfoList(LeaguerInfoIn LeaguerQueryIn)
        {
            var query = _leaguerRepository.GetAll();
            var IdArray = new int[LeaguerQueryIn.leaguer.Count];
            for (var i = 0; i < LeaguerQueryIn.leaguer.Count(); i++)
            {
                IdArray[i] = LeaguerQueryIn.leaguer[i];
            }
            query = query.Where(t => IdArray.Contains(t.Id));
            return await query.ToListAsync();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientLeaguerInfo> CreateLeaguerInfo(YaeherPatientLeaguerInfo create)
        {
            create.Id = await _leaguerRepository.InsertAndGetIdAsync(create);
            return create;
        }
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientLeaguerInfo> LeaguerInfoById(int Id)
        {
            return await _leaguerRepository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientLeaguerInfo> UpdateLeaguerInfo(YaeherPatientLeaguerInfo Leaguer)
        {
            return await _leaguerRepository.UpdateAsync(Leaguer);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientLeaguerInfo> DeleteLeaguerInfo(YaeherPatientLeaguerInfo input)
        {

            return await _leaguerRepository.UpdateAsync(input);
        }
        /// <summary>
        /// 获取患者成员Page信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherPatientLeaguerInfo>> LeaguerInfoPage(LeaguerInfo input)
        {
            //初步过滤
            var query = _leaguerRepository.GetAll().OrderByDescending(a => a.CreatedOn).Where(input.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / input.MaxResultCount;
            //默认的分页方式
            //var taskList = query.Skip(input.SkipTotal).Take(input.MaxResultCount).ToList();
            //ABP提供了扩展方法PageBy分页方式
            var taskList = await query.OrderBy(t => t.LeaguerName).ThenBy(t => t.Sex).PageBy(input.SkipTotal, input.MaxResultCount).ToListAsync();

            //IQueryable<YaeherPatientLeaguerInfo> querys = null;
            //querys = from items in query orderby items.LeaguerName, items.Sex select items;
            //var taskLists = await querys.PageBy(input.SkipTotal, input.MaxResultCount).ToListAsync();

            return new PagedResultDto<YaeherPatientLeaguerInfo>(tasksCount, taskList.MapTo<List<YaeherPatientLeaguerInfo>>());
        }

    }
}
