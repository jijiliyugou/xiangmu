using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Doctor;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    public interface IDoctorEmploymentService: IApplicationService
    {
        Task<DoctorEmployment> CreateDoctorEmployment(DoctorEmployment DoctorFileApplyInfo);
        Task<DoctorEmployment> DeleteDoctorEmployment(DoctorEmployment DoctorFileApplyInfo);
        Task<DoctorEmployment> DoctorEmploymentByID(int Id);
        Task<IList<DoctorEmployment>> DoctorEmploymentList(DoctorEmploymentIn DoctorFileApplyInfo);
        Task<PagedResultDto<DoctorEmployment>> DoctorEmploymentPage(DoctorEmploymentIn DoctorFileApplyInfo);
        Task<DoctorEmployment> UpdateDoctorEmployment(DoctorEmployment DoctorFileApplyInfo);
    }
}