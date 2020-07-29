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

namespace Yaeher.Release
{
    /// <summary>
    /// 问答
    /// </summary>
    public class QuestionReleaseService : IQuestionReleaseService
    {
        private readonly IRepository<QuestionRelease> _repository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        /// <summary>
        /// 问答 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userrepository"></param>
        /// <param name="doctorrepository"></param>
        public QuestionReleaseService(IRepository<QuestionRelease> repository, IRepository<YaeherUser> userrepository, IRepository<YaeherDoctor> doctorrepository)
        {
            _repository = repository;
            _userrepository = userrepository;
            _doctorrepository = doctorrepository;
        }
        /// <summary>
        /// 查询问答 List
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<QuestionRelease>> QuestionReleaseList(QuestionReleaseIn QuestionReleaseInfo)
        {
            #region
            //var release = _repository.GetAll();
            //var doctor = _doctorrepository.GetAll();
            //var user = _userrepository.GetAll();
            //var QuestionReleases = from a in release
            //                join b in doctor on a.DoctorId equals b.Id
            //                join c in user on b.UserID equals c.Id
            //                where !a.IsDelete && !b.IsDelete
            //                select new QuestionReleaseOutList
            //                {
            //                    CreatedOn = b.CreatedOn,
            //                    CreatedBy = b.CreatedBy,
            //                    ModifyBy = b.ModifyBy,
            //                    ModifyOn = b.ModifyOn,
            //                    UserImage = c.UserImage,
            //                    DoctorName = b.DoctorName,
            //                    DescriptionTiltle = a.DescriptionTiltle,
            //                    Title = a.Title,
            //                    Answer = a.Answer,
            //                    TitleDetail = a.TitleDetail,
            //                    DoctorId = a.DoctorId,
            //                    ReadTotal = a.ReadTotal,
            //                    UpvoteTotal = a.UpvoteTotal,
            //                    TransTotal = a.TransTotal,
            //                    CollectTotal = a.CollectTotal,
            //                    CheckState = a.CheckState,
            //                    ClinicName = b.Department,
            //                    CheckRemark = a.CheckRemark,
            //                    Checker = a.Checker,
            //                    CheckTime = a.CheckTime,
            //                    DoctorTitle = b.Title,
            //                    Hospital = b.HospitalName,
            //                    Id = a.Id,
            //                };
            //QuestionReleases = QuestionReleases.Where(t => !t.IsDelete);
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.DescriptionTiltle))
            //{
            //    QuestionReleases = QuestionReleases.Where(t => t.DescriptionTiltle.Contains(QuestionReleaseInfo.DescriptionTiltle));
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.KeyWord))
            //{
            //    QuestionReleases = QuestionReleases.Where(t => t.DescriptionTiltle.Contains(QuestionReleaseInfo.KeyWord) || t.Answer.Contains(QuestionReleaseInfo.KeyWord) || t.Title.Contains(QuestionReleaseInfo.KeyWord) || t.TitleDetail.Contains(QuestionReleaseInfo.KeyWord));
            //}
            //DateTime StartTime = new DateTime();
            //DateTime EndTime = new DateTime();
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            //{
            //    StartTime = DateTime.Parse(QuestionReleaseInfo.StartTime);
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
            //{
            //    EndTime = DateTime.Parse(QuestionReleaseInfo.EndTime).AddDays(+1);
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            //{
            //    QuestionReleases = QuestionReleases.Where(a => a.CreatedOn >= StartTime && a.CreatedOn < EndTime);
            //}
            //return await QuestionReleases.ToListAsync();
            #endregion
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(QuestionReleaseInfo.Expression);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询问答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> QuestionReleaseByID(int Id)
        {
            var QuestionReleases = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QuestionReleases;
        }

        
        /// <summary>
        /// 问答byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> QuestionReleaseByExpress(Expression<Func<QuestionRelease, bool>> whereExpression)
        {
            var QuestionRelease = await _repository.FirstOrDefaultAsync(whereExpression);
            return QuestionRelease;
        }

        /// <summary>
        /// 患者查询问答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> PatientQuestionReleaseByID(int Id)
        {
            //初步过滤
            #region
            //var release = _repository.GetAll();
            //var doctor = _doctorrepository.GetAll();
            //var user = _userrepository.GetAll();
            //var querylist = from a in release
            //                join b in doctor on a.DoctorId equals b.Id
            //                join c in user on b.UserID equals c.Id
            //                where !a.IsDelete && !b.IsDelete
            //                select new QuestionReleaseOutList
            //                {
            //                    CreatedOn = b.CreatedOn,
            //                    CreatedBy = b.CreatedBy,
            //                    ModifyBy = b.ModifyBy,
            //                    ModifyOn = b.ModifyOn,
            //                    UserImage = c.UserImage,
            //                    DoctorName = b.DoctorName,
            //                    DescriptionTiltle = a.DescriptionTiltle,
            //                    Title = a.Title,
            //                    Answer = a.Answer,
            //                    TitleDetail = a.TitleDetail,
            //                    DoctorId = a.DoctorId,
            //                    DoctorTitle=b.Title,
            //                    Hospital=b.HospitalName,
            //                    ReadTotal = a.ReadTotal,
            //                    ClinicName = b.Department,
            //                    UpvoteTotal = a.UpvoteTotal,
            //                    TransTotal = a.TransTotal,
            //                    CollectTotal = a.CollectTotal,
            //                    CheckState = a.CheckState,
            //                    CheckRemark = a.CheckRemark,
            //                    Checker = a.Checker,
            //                    CheckTime = a.CheckTime,
            //                    Id=a.Id,
            //                };
            //return await querylist.FirstOrDefaultAsync(t => !t.IsDelete&&t.Id==Id);
            #endregion
            var QuestionReleases = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QuestionReleases;
        }
        /// <summary>
        /// 查询问答 page
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QuestionRelease>> QuestionReleasePage(QuestionReleaseIn QuestionReleaseInfo)
        {
            //初步过滤
            #region
            //var release = _repository.GetAll();
            //var doctor = _doctorrepository.GetAll();
            //var user = _userrepository.GetAll();
            //var querylist = from a in release
            //                join b in doctor on a.DoctorId equals b.Id
            //                join c in user on b.UserID equals c.Id
            //                where !a.IsDelete && !b.IsDelete
            //                select new QuestionReleaseOutList
            //                {
            //                    CreatedOn = b.CreatedOn,
            //                    CreatedBy = b.CreatedBy,
            //                    ModifyBy = b.ModifyBy,
            //                    ModifyOn = b.ModifyOn,
            //                    UserImage = c.UserImage,
            //                    DoctorName = b.DoctorName,
            //                    DescriptionTiltle = a.DescriptionTiltle,
            //                    Title = a.Title,
            //                    Answer = a.Answer,
            //                    TitleDetail = a.TitleDetail,
            //                    DoctorId = a.DoctorId,
            //                    ReadTotal = a.ReadTotal,
            //                    UpvoteTotal = a.UpvoteTotal,
            //                    TransTotal = a.TransTotal,
            //                    CollectTotal = a.CollectTotal,
            //                    CheckState = a.CheckState,
            //                    ClinicName=b.Department,
            //                    CheckRemark = a.CheckRemark,
            //                    Checker = a.Checker,
            //                    CheckTime = a.CheckTime,
            //                    DoctorTitle = b.Title,
            //                    Hospital = b.HospitalName,
            //                    Id=a.Id,
            //                };
            //querylist = querylist.Where(t => !t.IsDelete).OrderByDescending(a => a.CreatedOn);
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.KeyWord))
            //{
            //    querylist = querylist.Where(t => (t.DescriptionTiltle.Contains(QuestionReleaseInfo.KeyWord)) || (t.Title.Contains(QuestionReleaseInfo.KeyWord)) || (t.TitleDetail.Contains(QuestionReleaseInfo.KeyWord)));
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.KeyWord))
            //{
            //    querylist = querylist.Where(t => t.DescriptionTiltle.Contains(QuestionReleaseInfo.KeyWord)||t.Answer.Contains(QuestionReleaseInfo.KeyWord) || t.Title.Contains(QuestionReleaseInfo.KeyWord) || t.TitleDetail.Contains(QuestionReleaseInfo.KeyWord));
            //}
            //DateTime StartTime = new DateTime();
            //DateTime EndTime = new DateTime();
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            //{
            //    StartTime = DateTime.Parse(QuestionReleaseInfo.StartTime);
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
            //{
            //    EndTime = DateTime.Parse(QuestionReleaseInfo.EndTime).AddDays(+1);
            //}
            //if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            //{
            //    querylist = querylist.Where(a => a.CreatedOn >= StartTime && a.CreatedOn < EndTime);
            //}
            #endregion

            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(QuestionReleaseInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / QuestionReleaseInfo.MaxResultCount;
            var QuestionReleaseList = await query.PageBy(QuestionReleaseInfo.SkipTotal, QuestionReleaseInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QuestionRelease>(tasksCount, QuestionReleaseList.MapTo<List<QuestionRelease>>());
        }
        /// <summary>
        /// 新建问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> CreateQuestionRelease(QuestionRelease QuestionReleaseInfo)
        {
            QuestionReleaseInfo.Id = await _repository.InsertAndGetIdAsync(QuestionReleaseInfo);
            return QuestionReleaseInfo;
        }

        /// <summary>
        /// 修改问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> UpdateQuestionRelease(QuestionRelease QuestionReleaseInfo)
        {
            return await _repository.UpdateAsync(QuestionReleaseInfo);
        }

        /// <summary>
        /// 删除问答
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuestionRelease> DeleteQuestionRelease(QuestionRelease QuestionReleaseInfo)
        {
            return await _repository.UpdateAsync(QuestionReleaseInfo);
        }
    }
}
