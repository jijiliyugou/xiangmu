using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using System.Linq;
using Abp.Application.Services;

namespace Yaeher
{
    /// <summary>
    /// 获取用户基本信息
    /// </summary>
    public class UserManagerService : IUserManagerService
    {
        private readonly IRepository<YaeherUser> _repository;
        private readonly IRepository<YaeherDoctor> _Doctorrepository;
        
        private readonly IRepository<YaeherRole> _Rolerepository;
        private readonly IRepository<YaeherUserRole> _UserRolerepository;
        
        private readonly IRepository<YaeherModule> _Modulerepository;
        private readonly IRepository<YaeherRoleModule> _RoleModulerepository;
        private readonly IRepository<DoctorRelation> _DoctorRelationrepository;
        private readonly IRepository<ServiceMoneyList> _ServiceMoneyListrepository;
        private readonly IRepository<YaeherUserPayment> _YaeherUserPaymentrepository;
        private readonly IRepository<LableManage> _LableManagerepository;
        private readonly IRepository<SystemParameter> _SystemParameterrepository;
        private readonly IRepository<DoctorParaSet> _DoctorParaSetrepository;
        private readonly IRepository<DoctorOnlineRecord> _DoctorOnlineRecordrepository;
        /// <summary>
        /// 用户基础表 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="Doctorrepository"></param>
        /// <param name="Rolerepository"></param>
        /// <param name="UserRolerepository"></param>
        /// <param name="Modulerepository"></param>
        /// <param name="RoleModulerepository"></param>
        /// <param name="DoctorRelationrepository"></param>
        /// <param name="ServiceMoneyListrepository"></param>
        /// <param name="YaeherUserPaymentrepository"></param>
        /// <param name="LableManagerepository"></param>
        /// <param name="SystemParameterrepository"></param>
        /// <param name="DoctorParaSetrepository"></param>
        /// <param name="DoctorOnlineRecordrepository"></param>
        public UserManagerService(IRepository<YaeherUser> repository, 
                                  IRepository<YaeherDoctor> Doctorrepository,
                                  IRepository<YaeherRole> Rolerepository,
                                  IRepository<YaeherUserRole> UserRolerepository,
                                  IRepository<YaeherModule> Modulerepository,
                                  IRepository<YaeherRoleModule> RoleModulerepository,
                                  IRepository<DoctorRelation> DoctorRelationrepository,
                                  IRepository<ServiceMoneyList> ServiceMoneyListrepository,
                                  IRepository<YaeherUserPayment> YaeherUserPaymentrepository,
                                  IRepository<LableManage> LableManagerepository,
                                  IRepository<SystemParameter> SystemParameterrepository,
                                  IRepository<DoctorParaSet> DoctorParaSetrepository,
                                  IRepository<DoctorOnlineRecord> DoctorOnlineRecordrepository
                                  )
        {
            // 获取个人信息
            _repository = repository;
            _Doctorrepository = Doctorrepository;
            _Rolerepository = Rolerepository;
            _UserRolerepository = UserRolerepository;
            _Modulerepository = Modulerepository;
            _RoleModulerepository = RoleModulerepository;

             // 获取医生信息
            _DoctorRelationrepository = DoctorRelationrepository;
            _ServiceMoneyListrepository = ServiceMoneyListrepository;
            _YaeherUserPaymentrepository = YaeherUserPaymentrepository;
            _LableManagerepository = LableManagerepository;
            _SystemParameterrepository = SystemParameterrepository;
            _DoctorParaSetrepository = DoctorParaSetrepository;
            _DoctorOnlineRecordrepository=DoctorOnlineRecordrepository;
        }
        /// <summary>
        /// 根据用户ID获取用户基本信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public UserManager UserManager(int UserID)
        {
            UserManager userManager = new UserManager();
            userManager.YaeherUserInfo =  _repository.FirstOrDefault(t => t.Id == UserID && t.IsDelete==false);
            userManager.MobileRoleName = userManager.YaeherUserInfo.RoleName;
            userManager.YaeherDoctorInfo = _Doctorrepository.FirstOrDefault(t => t.UserID == UserID && t.IsDelete==false);
            // 查询用户角色信息
            var YaeherUserRoles = _UserRolerepository.GetAll().Where(a => a.UserID == UserID && a.IsDelete == false);
            var YaeherRoles =  _Rolerepository.GetAll().Where(a => a.IsDelete==false && a.Enabled==true);

            var YaeherRoleModule = _RoleModulerepository.GetAll().Where(a =>a.IsDelete == false);
            var YaeherModule = _Modulerepository.GetAll().Where(a => a.IsDelete == false && a.Enabled == true);

            if (YaeherUserRoles.Count() > 0)
            {
                var RoleList = from a in YaeherUserRoles
                               join b in YaeherRoles on a.RoleID equals b.Id
                               select b;
                if (RoleList.Count() > 0)
                {
                    userManager.RoleList = RoleList.Where(a => a.Enabled == true).ToList();
                }
            }
            List<YaeherModule> ModuleList = new List<YaeherModule>();
            if (userManager.RoleList != null)
            {
                if (YaeherRoleModule.Count() > 0)
                {
                    var ModulesList = from a in userManager.RoleList
                                      join b in YaeherRoleModule on a.Id equals b.RoleId
                                      join c in YaeherModule on b.ModuleId equals c.Id
                                      select c;
                    if (ModulesList.Count() > 0)
                    {
                        ModuleList = ModulesList.Distinct().ToList();
                    }
                }
            }
            if (ModuleList.Count > 0)
            {
                userManager.ModuleList = YaeherModuleList(ModuleList);
            }
            if (userManager.RoleList != null)
            {
                if (userManager.RoleList.Count > 0)
                {
                    userManager.IsAdmin = userManager.RoleList.Where(a => a.RoleCode == "Manager").Count() > 0 ? true : false;
                    userManager.IsDoctor = userManager.RoleList.Where(a => a.RoleCode == "Doctor").Count() > 0 ? true : false;
                    userManager.IsCustomerService = userManager.RoleList.Where(a => a.RoleCode == "CustomerService").Count() > 0 ? true : false;
                    userManager.IsQC = userManager.RoleList.Where(a => a.RoleCode == "QC").Count() > 0 ? true : false;
                }
            }
            userManager.IsDoctor = userManager.MobileRoleName == "doctor" ?  true : userManager.IsDoctor;
            return  userManager;
        }
        /// <summary>
        /// 赋值权限菜单 List
        /// </summary>
        /// <param name="ModuleList"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public List<YaeherModuleNode> YaeherModuleList(List<YaeherModule> ModuleList)
        {
            List<YaeherModuleNode> yaeherModuleNodes = new List<YaeherModuleNode>();
            List<YaeherModule> ModuleNode = ModuleList.Where(a => a.ParentId == 0).ToList();
            foreach (var item in ModuleNode)
            {
                YaeherModuleNode yaeherModuleNode = new YaeherModuleNode();
                yaeherModuleNode.Id = item.Id;
                yaeherModuleNode.ParentId = item.ParentId;
                yaeherModuleNode.Names = item.Names;
                yaeherModuleNode.LinkUrls = item.LinkUrls;
                yaeherModuleNode.Areas = item.Areas;
                yaeherModuleNode.Controllers = item.Controllers;
                yaeherModuleNode.Actionss = item.Actionss;
                yaeherModuleNode.Icons = item.Icons;
                yaeherModuleNode.Codes = item.Codes;
                yaeherModuleNode.OrderSort = item.OrderSort;
                yaeherModuleNode.Description = item.Description;
                yaeherModuleNode.IsMenu = item.IsMenu;
                yaeherModuleNode.Enabled = item.Enabled;
                yaeherModuleNode.children = GetChild(ModuleList.ToList(), item.Id);
                yaeherModuleNodes.Add(yaeherModuleNode);
            }
            return yaeherModuleNodes.OrderBy(a=>a.OrderSort).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ModuleList"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public List<YaeherModuleNode> GetChild(List<YaeherModule> ModuleList, int Id)
        {
            List<YaeherModuleNode> child = new List<YaeherModuleNode>();
            var ChildModuleList = ModuleList.Where(a => a.ParentId == Id).ToList();
            if (ChildModuleList.Count > 0)
            {
                foreach (var item in ChildModuleList)
                {
                    YaeherModuleNode yaeherModuleNode = new YaeherModuleNode();
                    yaeherModuleNode.Id = item.Id;
                    yaeherModuleNode.ParentId = item.ParentId;
                    yaeherModuleNode.Names = item.Names;
                    yaeherModuleNode.LinkUrls = item.LinkUrls;
                    yaeherModuleNode.Areas = item.Areas;
                    yaeherModuleNode.Controllers = item.Controllers;
                    yaeherModuleNode.Actionss = item.Actionss;
                    yaeherModuleNode.Icons = item.Icons;
                    yaeherModuleNode.Codes = item.Codes;
                    yaeherModuleNode.OrderSort = item.OrderSort;
                    yaeherModuleNode.Description = item.Description;
                    yaeherModuleNode.IsMenu = item.IsMenu;
                    yaeherModuleNode.Enabled = item.Enabled;
                    if (ModuleList.Where(a => a.ParentId == item.Id).ToList().Count > 0)
                    {
                        yaeherModuleNode.children = GetChild(ModuleList, item.Id);
                    }
                    child.Add(yaeherModuleNode);
                }
            }
            return child.OrderBy(a => a.OrderSort).ToList(); ;
        }
        /// <summary>
        ///  通过医生ID 
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public YaeherDoctorInfo DoctorInformation(int DoctorID)
        {
            YaeherDoctorInfo yaeherDoctorInfo = new YaeherDoctorInfo();
            // 查询医生基本信息
            yaeherDoctorInfo.DoctorInfo = _Doctorrepository.FirstOrDefault(t => t.Id == DoctorID && t.IsDelete == false);
            var UserId = yaeherDoctorInfo.DoctorInfo.UserID;
            // 用户基本信息
            yaeherDoctorInfo.YaeherUserInfo= _repository.FirstOrDefault(t => t.Id == UserId && t.IsDelete == false);
            // 医生与标签关系
            yaeherDoctorInfo.DoctorRelationList= _DoctorRelationrepository.GetAll().Where(a=>a.IsDelete==false&& a.DoctorID== DoctorID).ToList();
            // 医生提供服务
            yaeherDoctorInfo.ServiceMoneyLists = _ServiceMoneyListrepository.GetAll().Where(a => a.IsDelete == false && a.DoctorID == DoctorID).ToList();
            // 医生支付方式
            yaeherDoctorInfo.YaeherUserPayment = _YaeherUserPaymentrepository.GetAll().Where(a => a.IsDelete == false && a.UserID == UserId).ToList();
            // 标签 疾病类型
            yaeherDoctorInfo.LableManageList = _LableManagerepository.GetAll().Where(a => a.IsDelete == false).ToList();
            // 医生上下线信息
            yaeherDoctorInfo.DoctorOnlineRecord = _DoctorOnlineRecordrepository.GetAll().Where(a => a.IsDelete == false&&a.DoctorID== DoctorID).OrderByDescending(a=>a.CreatedOn).FirstOrDefault();
            
            // 基础参数配置
            //var SystemParameterList = _SystemParameterrepository.GetAll().Where(a => a.IsDelete == false);
            // 医生基础参数配置
            var DoctorParaSetList = _DoctorParaSetrepository.GetAll().Where(a => a.IsDelete == false);
            List<SystemParameter> SystemParameterList = new List<SystemParameter>();
            if (DoctorParaSetList.Count() > 0)
            {
                foreach (var DoctorParInfo in DoctorParaSetList)
                {
                    SystemParameter systemParameter = new SystemParameter();
                    systemParameter.SystemType = DoctorParInfo.DoctorParaSetCode;
                    systemParameter.SystemCode = DoctorParInfo.DoctorParaSetCode;
                    systemParameter.Code = DoctorParInfo.DoctorParaSetCode;
                    systemParameter.Name = DoctorParInfo.DoctorParaSetName;
                    systemParameter.ItemValue = DoctorParInfo.ItemValue;
                    systemParameter.Remark = DoctorParInfo.DoctorParaSetName;
                    SystemParameterList.Add(systemParameter);
                }
            }
            if (SystemParameterList.Count() > 0)
            {
                yaeherDoctorInfo.InquiryMaxCount = SystemParameterList.FirstOrDefault(a=>a.SystemCode== "InquiryMaxCount");
                yaeherDoctorInfo.WxPayBusinessId = SystemParameterList.FirstOrDefault(a => a.SystemCode == "WxPayBusinessId");
                yaeherDoctorInfo.ConsultationSucessTime = SystemParameterList.FirstOrDefault(a => a.SystemCode == "ConsultationSucessTime");
                yaeherDoctorInfo.ReplyMinutesTime = SystemParameterList.FirstOrDefault(a => a.SystemCode == "ReplyMinutesTime");
            }

            return yaeherDoctorInfo;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherDoctorInfo>> DoctorInfoList()
        {
            List<YaeherDoctorInfo> yaeherDoctorInfos = new List<YaeherDoctorInfo>();
            // 查询医生基本信息
            List<YaeherDoctor>  yaeherDoctors = await _Doctorrepository.GetAllListAsync(t => t.IsDelete == false);
            if(yaeherDoctors.Count()>0)
            {
                foreach (var DoctorInfo in yaeherDoctors)
                {
                    YaeherDoctorInfo yaeherDoctorInfo = new YaeherDoctorInfo();
                    yaeherDoctorInfo.DoctorInfo = DoctorInfo;
                    var UserId = DoctorInfo.UserID;
                    // 用户基本信息
                    yaeherDoctorInfo.YaeherUserInfo = _repository.FirstOrDefault(t => t.Id == UserId && t.IsDelete == false);
                    // 医生与标签关系
                    yaeherDoctorInfo.DoctorRelationList = _DoctorRelationrepository.GetAll().Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.Id).ToList();
                    // 医生提供服务
                    yaeherDoctorInfo.ServiceMoneyLists = _ServiceMoneyListrepository.GetAll().Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.Id).ToList();
                    // 医生支付方式
                    yaeherDoctorInfo.YaeherUserPayment = _YaeherUserPaymentrepository.GetAll().Where(a => a.IsDelete == false && a.UserID == UserId).ToList();
                    // 标签 疾病类型
                    yaeherDoctorInfo.LableManageList = _LableManagerepository.GetAll().Where(a => a.IsDelete == false).ToList();
                    // 医生基础参数配置
                    var DoctorParaSetList = _DoctorParaSetrepository.GetAll().Where(a => a.IsDelete == false);
                    List<SystemParameter> SystemParameterList = new List<SystemParameter>();
                    if (DoctorParaSetList.Count() > 0)
                    {
                        foreach (var DoctorParInfo in DoctorParaSetList)
                        {
                            SystemParameter systemParameter = new SystemParameter();
                            systemParameter.SystemType = DoctorParInfo.DoctorParaSetCode;
                            systemParameter.SystemCode = DoctorParInfo.DoctorParaSetCode;
                            systemParameter.Code = DoctorParInfo.DoctorParaSetCode;
                            systemParameter.Name = DoctorParInfo.DoctorParaSetName;
                            systemParameter.ItemValue = DoctorParInfo.ItemValue;
                            systemParameter.Remark = DoctorParInfo.DoctorParaSetName;
                            SystemParameterList.Add(systemParameter);
                        }
                    }
                    if (SystemParameterList.Count() > 0)
                    {
                        yaeherDoctorInfo.InquiryMaxCount = SystemParameterList.FirstOrDefault(a => a.SystemCode == "InquiryMaxCount");
                        yaeherDoctorInfo.WxPayBusinessId = SystemParameterList.FirstOrDefault(a => a.SystemCode == "WxPayBusinessId");
                        yaeherDoctorInfo.ConsultationSucessTime = SystemParameterList.FirstOrDefault(a => a.SystemCode == "ConsultationSucessTime");
                        yaeherDoctorInfo.ReplyMinutesTime = SystemParameterList.FirstOrDefault(a => a.SystemCode == "ReplyMinutesTime");
                    }
                    yaeherDoctorInfos.Add(yaeherDoctorInfo);
                }
            }
            return yaeherDoctorInfos;
        }
    }
}
