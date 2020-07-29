using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yaeher.ClinicManage.Dto;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊 标签  医生 关系
    /// </summary>
    public class ClinicManageService : IClinicManageService
    {
        private readonly IRepository<ClinicInfomation> _ClinicInfomationrepository;
        private readonly IRepository<ClinicDoctorReltion> _ClinicDoctorReltionrepository;
        private readonly IRepository<ClinicLableReltion> _ClinicLableReltionrepository;
        private readonly IRepository<LableManage> _LableManagerepository;
        private readonly IRepository<YaeherDoctor> _YaeherDoctorrepository;
        private readonly IRepository<DoctorRelation> _DoctorRelationrepository;
        private readonly IRepository<DoctorOnlineRecord> _DoctorOnlineRecordrepository;
        private readonly IRepository<DoctorFileApply> _DoctorFileApplyrepository;
        private readonly IRepository<DoctorScheduling> _DoctorSchedulingrepository;
        private readonly IRepository<ServiceMoneyList> _ServiceMoneyListrepository;

        /// <summary>
        /// 门诊 标签  医生 关系
        /// </summary>
        /// <param name="ClinicInfomationrepository"></param>
        /// <param name="ClinicDoctorReltionrepository"></param>
        /// <param name="ClinicLableReltionrepository"></param>
        /// <param name="YaeherDoctorrepository"></param>
        /// <param name="DoctorRelationrepository"></param>
        /// <param name="DoctorOnlineRecordrepository"></param>
        /// <param name="DoctorFileApplyrepository"></param>
        /// <param name="DoctorSchedulingrepository"></param>
        /// <param name="ServiceMoneyListrepository"></param>
        /// <param name="LableManagerepository"></param>
        public ClinicManageService(IRepository<ClinicInfomation> ClinicInfomationrepository,
                                   IRepository<ClinicDoctorReltion> ClinicDoctorReltionrepository,
                                   IRepository<ClinicLableReltion> ClinicLableReltionrepository,
                                   IRepository<YaeherDoctor> YaeherDoctorrepository,
                                   IRepository<DoctorRelation> DoctorRelationrepository,
                                   IRepository<DoctorOnlineRecord> DoctorOnlineRecordrepository,
                                   IRepository<DoctorFileApply> DoctorFileApplyrepository,
                                   IRepository<DoctorScheduling> DoctorSchedulingrepository,
                                   IRepository<ServiceMoneyList> ServiceMoneyListrepository,
                                   IRepository<LableManage> LableManagerepository
                                   )
        {
            _ClinicInfomationrepository = ClinicInfomationrepository;
            _ClinicDoctorReltionrepository = ClinicDoctorReltionrepository;
            _ClinicLableReltionrepository = ClinicLableReltionrepository;
            _YaeherDoctorrepository = YaeherDoctorrepository;
            _DoctorRelationrepository = DoctorRelationrepository;
            _DoctorOnlineRecordrepository = DoctorOnlineRecordrepository;
            _DoctorFileApplyrepository = DoctorFileApplyrepository;
            _DoctorSchedulingrepository = DoctorSchedulingrepository;
            _ServiceMoneyListrepository = ServiceMoneyListrepository;
            _LableManagerepository = LableManagerepository;
        }

        /// <summary>
        /// 门诊 标签  医生 关系
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<ClinicInfo>> ClinicInfoList(ClinicInfomationIn ClinicInfomationInfo)
        {
            List<ClinicInfo> clinicInfoList = new List<ClinicInfo>();
            // 科室数据Lsit
            var clinicList =await _ClinicInfomationrepository.GetAll().OrderBy(a => a.CreatedOn).Where(ClinicInfomationInfo.Expression).ToListAsync();
            // 科室与医生关系
            var clinicDoctorReltionList = _ClinicDoctorReltionrepository.GetAll().Where(a => a.IsDelete == false).ToList();
            // 科室与标签关系
            var clinicLableReltionList = _ClinicLableReltionrepository.GetAll().Where(a => a.IsDelete == false).ToList();
            // 标签数据list
            var LableList = _LableManagerepository.GetAll().Where(a => a.IsDelete == false).ToList();
            // 医生基本信息list
            var DoctorList = _YaeherDoctorrepository.GetAll().Where(a => a.IsDelete == false).ToList();
            // 医生与标签关系
            var DoctorRelationList = _DoctorRelationrepository.GetAll().Where(a => a.IsDelete == false).ToList();
            if (clinicList.Count > 0)
            {
                foreach (var clinicInfo in clinicList)
                {
                    ClinicInfo clinicInfos = new ClinicInfo();
                    clinicInfos.clinicInfomation = clinicInfo;
                    // 科室与医生关系
                    clinicInfos.clinicDoctorReltion = clinicDoctorReltionList.Where(a => a.ClinicID == clinicInfo.Id).ToList();
                    // 科室与标签关系
                    clinicInfos.clinicLableReltion = clinicLableReltionList.Where(a => a.ClinicID == clinicInfo.Id).ToList();
                    clinicInfoList.Add(clinicInfos);
                }
            }
            if (clinicInfoList.Count > 0)
            {
                foreach (var clinicInfo in clinicInfoList)
                {
                    if (clinicInfo.clinicDoctorReltion.Count() > 0)
                    {
                        List<YaeherDoctorList> yaeherDoctorList = new List<YaeherDoctorList>();
                        foreach (var DoctorInfo in clinicInfo.clinicDoctorReltion)
                        {
                            YaeherDoctorList yaeherDoctor = new YaeherDoctorList();
                            //医生基本信息
                            yaeherDoctor.yaeherDoctor = DoctorList.Where(a=>a.Id== DoctorInfo.DoctorID).FirstOrDefault();
                            //医生与标签关系
                            yaeherDoctor.doctorRelation = _DoctorRelationrepository.GetAll().Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                            //医生上下线设置状态
                            yaeherDoctor.doctorOnlineRecord = _DoctorOnlineRecordrepository.GetAll().FirstOrDefault(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false);
                            //医生申请文件
                            yaeherDoctor.doctorFileApply = _DoctorFileApplyrepository.GetAll().Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                            //医生排班
                            yaeherDoctor.doctorScheduling = _DoctorSchedulingrepository.GetAll().Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                            //医生提供服务信息
                            yaeherDoctor.serviceMoneyList = _ServiceMoneyListrepository.GetAll().Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                            yaeherDoctorList.Add(yaeherDoctor);
                        }
                        if (DoctorList.Count() > 0)
                        {
                            clinicInfo.yaeherDoctorList = yaeherDoctorList;
                        }
                    }
                }
            }
            return clinicInfoList;
        }
    }
}
