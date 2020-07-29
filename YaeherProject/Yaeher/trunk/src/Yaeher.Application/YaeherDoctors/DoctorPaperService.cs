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
    /// 医生发布文章
    /// </summary>
    public class DoctorPaperService : IDoctorPaperService
    {
        private readonly IRepository<DoctorPaper> _repository;
        private readonly IRepository<SystemParameter> _sysrepository;
        /// <summary>
        /// 医生发布文章 构造函数
        /// </summary>
        /// <param name="repository"></param>
        ///  <param name="sysrepository"></param>
        public DoctorPaperService(IRepository<DoctorPaper> repository, IRepository<SystemParameter> sysrepository)
        {
            _repository = repository;
            _sysrepository = sysrepository;
        }
        /// <summary>
        /// 查询医生发布文章 List
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorPaper>> DoctorPaperList(DoctorPaperIn DoctorPaperInfo)
        {
            var DoctorPapers = await _repository.GetAllListAsync(DoctorPaperInfo.Expression);
            return DoctorPapers.ToList();
        }
        /// <summary>
        /// 查询医生发布文章 List
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorPaperView>> DoctorPaperViewList(DoctorPaperIn DoctorPaperInfo)
        {
            var relation = _repository.GetAll().OrderByDescending(a => a.CreatedOn);
            var querylist = from a in relation
                            where !a.IsDelete && a.DoctorID == DoctorPaperInfo.DoctorId && a.CheckState == "success"
                            select new DoctorPaperView
                            {
                                ImageFie = a.ImageFie,
                                DoctorName = a.DoctorName,
                                DoctorID = a.DoctorID,
                                PaperTiltle = a.PaperTiltle,
                                PaperContent = a.PaperContent,
                                PaperFrom = a.PaperFrom,
                                ConsultNumber = a.ConsultNumber,
                                PaperAddress = a.PaperAddress,
                                Checker = a.Checker,
                                CheckState = a.CheckState,
                                CheckRemark = a.CheckRemark,
                                CheckTime = a.CheckTime,
                                Id = a.Id,
                                CreatedOn = a.CreatedOn,
                            };
            var DoctorPaperList = await querylist.PageBy(DoctorPaperInfo.SkipTotal, DoctorPaperInfo.MaxResultCount).ToListAsync();
            return DoctorPaperList;
        }
        /// <summary>
        /// 查询医生发布文章byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorPaper> DoctorPaperByID(int Id)
        {
            var DoctorPapers = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorPapers;
        }
        /// <summary>
        /// 查询医生发布文章 page
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorPaper>> DoctorPaperPage(DoctorPaperIn DoctorPaperInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(DoctorPaperInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorPaperInfo.MaxResultCount;
            var DoctorPaperList = await query.PageBy(DoctorPaperInfo.SkipTotal, DoctorPaperInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorPaper>(tasksCount, DoctorPaperList.MapTo<List<DoctorPaper>>());
        }
        /// <summary>
        /// 查询医生发布文章 page
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorPaperView>> DoctorPaperViewPage(DoctorPaperIn DoctorPaperInfo)
        {
            var relation = _repository.GetAll().OrderByDescending(a=>a.CreatedOn).Where(DoctorPaperInfo.Expression);
            var sysfrom = _sysrepository.GetAll().Where(t => !t.IsDelete && t.SystemCode == "DoctorPaperFrom");
            var sysstate = _sysrepository.GetAll().Where(t => !t.IsDelete && t.SystemCode == "DoctorPaperState");
            var query = from a in relation
                        join b in sysfrom on a.PaperFrom equals b.Code
                        join c in sysstate on a.CheckState equals c.Code
                        select new DoctorPaperView
                        {
                            ImageFie = a.ImageFie,
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            PaperTiltle = a.PaperTiltle,
                            PaperContent = a.PaperContent,
                            PaperFrom = a.PaperFrom,
                            ConsultNumber = a.ConsultNumber,
                            PaperAddress = a.PaperAddress,
                            Checker = a.Checker,
                            CheckState = a.CheckState,
                            CheckRemark = a.CheckRemark,
                            CheckTime = a.CheckTime,
                            Id = a.Id,
                            DoctorPaperFrom = b.Name,
                            CheckStatus = c.Name,
                            ConsultID=a.ConsultID,
                            CreatedOn=a.CreatedOn,
                        };
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorPaperInfo.MaxResultCount;
            var DoctorPaperList = await query.PageBy(DoctorPaperInfo.SkipTotal, DoctorPaperInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorPaperView>(tasksCount, DoctorPaperList.MapTo<List<DoctorPaperView>>());
        }

        /// <summary>
        /// 新建医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorPaper> CreateDoctorPaper(DoctorPaper DoctorPaperInfo)
        {
            DoctorPaperInfo.Id = await _repository.InsertAndGetIdAsync(DoctorPaperInfo);
            return DoctorPaperInfo;
        }

        /// <summary>
        /// 修改医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorPaper> UpdateDoctorPaper(DoctorPaper DoctorPaperInfo)
        {
            return await _repository.UpdateAsync(DoctorPaperInfo);
        }

        /// <summary>
        /// 删除医生发布文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorPaper> DeleteDoctorPaper(DoctorPaper DoctorPaperInfo)
        {
            return await _repository.UpdateAsync(DoctorPaperInfo);
        }
    }
}
