using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 发送微信状态
    /// </summary>
    public class AcceptWecharStateService : IAcceptWecharStateService
    {
        private readonly IRepository<AcceptWecharState> _repository;
        private readonly IRepository<YaeherUser> _YaeherUserrepository;

        /// <summary>
        /// 发送微信状态 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="YaeherUserrepository"></param>
        public AcceptWecharStateService(IRepository<AcceptWecharState> repository, IRepository<YaeherUser> YaeherUserrepository)
        {
            _repository = repository;
            _YaeherUserrepository = YaeherUserrepository;
        }

        /// <summary>
        /// 发送微信状态 List
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<WecharState>> AcceptWecharStateList(AcceptWecharStateIn AcceptWecharStateInfo)
        {
            //初步过滤
            var AcceptTencentWechars = _repository.GetAll().Where(AcceptWecharStateInfo.Expression);
            
            var AcceptWechars =await AcceptTencentWechars.ToListAsync();
            var UserList = _YaeherUserrepository.GetAll().Where(a=>a.IsDelete==false&& a.Enabled==true).ToList();
            List<WecharState> WecharStateList = new List<WecharState>();
            if (AcceptWechars.Count>0)
            {
                // 将已完成的回复没有消息的关闭
                var WecharState = from a in AcceptWechars
                                  join b in UserList on a.ConsultantID equals b.Id
                                  select new WecharState
                                  {
                                      Id = a.Id,
                                      ConsultantID = a.ConsultantID,
                                      ConsultantName = a.ConsultantName,
                                      ConsultantJSON = a.ConsultantJSON,
                                      ConsultantOpenID = a.ConsultantOpenID,
                                      CustomerServiceID = a.CustomerServiceID,
                                      CustomerServiceName = a.CustomerServiceName,
                                      CustomerServiceJson = a.CustomerServiceJson,
                                      AcceptState = a.AcceptState,
                                      IsReady = a.IsReady,
                                      UserImg = b.UserImage,
                                      WecharName = b.WecharName,
                                      AcceptTime=a.AcceptTime,
                                  };
                if (WecharState.Count() > 0)
                {
                    WecharStateList = WecharState.ToList();
                }
            }
            return WecharStateList.OrderBy(a=>a.AcceptState).ThenBy(a=>a.IsReady).ThenByDescending(a => a.AcceptTime).ToList();
        }

        /// <summary>
        /// 发送微信状态 List
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<AcceptWecharState>> WecharStateList(AcceptWecharStateIn AcceptWecharStateInfo)
        {
            var AcceptTencentWechars = _repository.GetAll().OrderBy(a => a.AcceptState).ThenByDescending(a => a.AcceptTime).Where(AcceptWecharStateInfo.Expression);
            return await AcceptTencentWechars.ToListAsync();
        }

        /// <summary>
        /// 发送微信状态 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptWecharState> AcceptWecharStateByID(int Id)
        {
            var AcceptTencentWechars = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return AcceptTencentWechars;
        }
        /// <summary>
        /// 发送微信状态 page
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<AcceptWecharState>> AcceptWecharStatePage(AcceptWecharStateIn AcceptWecharStateInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderBy(a => a.AcceptState).ThenByDescending(a => a.AcceptTime).Where(AcceptWecharStateInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / AcceptWecharStateInfo.MaxResultCount;
            var AcceptTencentWecharList = await query.PageBy(AcceptWecharStateInfo.SkipTotal, AcceptWecharStateInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<AcceptWecharState>(tasksCount, AcceptTencentWecharList.MapTo<List<AcceptWecharState>>());
        }
        /// <summary>
        /// 新建 发送微信状态
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptWecharState> CreateAcceptWecharState(AcceptWecharState AcceptWecharStateInfo)
        {
            //AcceptWecharStateInfo.Id= _repository.InsertAndGetId(AcceptWecharStateInfo);
            //return AcceptWecharStateInfo;
            // 不做数据返回操作
            return await _repository.InsertAsync(AcceptWecharStateInfo);
        }
        /// <summary>
        /// 新建 发送微信状态
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AcceptWecharState> UpdateAcceptWecharState(AcceptWecharState AcceptWecharStateInfo)
        {
            return await _repository.UpdateAsync(AcceptWecharStateInfo);
        }
    }
}
