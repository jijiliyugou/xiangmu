using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.DoctorReport.Dto
{
    /// <summary>
    /// 医生收入汇总
    /// </summary>
    public class DoctorIncomeOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorIncomeDto"></param>
        /// <param name="DoctorIncomeInfo"></param>
        public DoctorIncomeOut(PagedResultDto<DoctorIncome> DoctorIncomeDto, DoctorIncomeIn DoctorIncomeInfo)
        {
            Items = DoctorIncomeDto.Items;
            TotalCount = DoctorIncomeDto.TotalCount;
            TotalPage = DoctorIncomeDto.TotalCount / DoctorIncomeInfo.MaxResultCount;
            SkipCount = DoctorIncomeInfo.SkipCount;
            MaxResultCount = DoctorIncomeInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorIncome> Items { get; set; }
    }
}
