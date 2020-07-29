using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System;
using System.Linq.Expressions;
using Yaeher.Common.TencentCustom;
using Yaeher.SystemConfig;
using Yaeher.Common.Constants;
using Yaeher.Common;
using Abp.Domain.Uow;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public class YaeherUserService : IYaeherUserService
    {
        private readonly IRepository<YaeherUser> _repository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        private readonly IRepository<SystemToken> _systemTokenrepository;
        private readonly IRepository<SystemParameter> _SystemParameterrepository;
        private readonly IRepository<YaeherUserPayment> _YaeherUserPaymentrepository;
        private readonly IRepository<SystemConfigs> _SystemConfigsrepository;
        private readonly IRepository<YaeherOperList> _YaeherOperListrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
      
        /// <summary>
        /// 用户基础表 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="doctorrepository"></param>
        /// <param name="systemTokenrepository"></param>
        /// <param name="SystemParameterrepository"></param>
        /// <param name="YaeherUserPaymentrepository"></param>
        /// <param name="SystemConfigsrepository"></param>
        /// <param name="YaeherOperListrepository"></param>
        /// <param name="unitOfWorkManager"></param>
        public YaeherUserService(IRepository<YaeherUser> repository, 
                                 IRepository<YaeherDoctor> doctorrepository,
                                 IRepository<SystemToken>  systemTokenrepository,
                                 IRepository<SystemParameter> SystemParameterrepository,
                                 IRepository<YaeherUserPayment> YaeherUserPaymentrepository,
                                 IRepository<SystemConfigs> SystemConfigsrepository,
                                 IRepository<YaeherOperList> YaeherOperListrepository,
                                 IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _doctorrepository = doctorrepository;
            _systemTokenrepository = systemTokenrepository;
            _SystemParameterrepository = SystemParameterrepository;
            _YaeherUserPaymentrepository = YaeherUserPaymentrepository;
            _SystemConfigsrepository = SystemConfigsrepository;
            _YaeherOperListrepository = YaeherOperListrepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 用户基础表 List
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherUser>> YaeherUserList(YaeherUserIn YaeherUserInfo)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherUserInfo.Expression);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 用户基础表byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> YaeherUserByID(int Id)
        {
            var YaeherUsers = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherUsers;
        }
        /// <summary>
        /// 用户基础表 page
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherUser>> YaeherUserPage(YaeherUserIn YaeherUserInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherUserInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherUserInfo.MaxResultCount;
            var YaeherUserList = await query.PageBy(YaeherUserInfo.SkipTotal, YaeherUserInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherUser>(tasksCount, YaeherUserList.MapTo<List<YaeherUser>>());
        }
        /// <summary>
        /// 新建用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> CreateYaeherUser(YaeherUser YaeherUserInfo)
        {
            try
            {
                YaeherUserInfo.Id = await _repository.InsertAndGetIdAsync(YaeherUserInfo);
                return YaeherUserInfo;
            }
            catch (Exception ex)
            {
                return new YaeherUser();
            }
        }
        /// <summary>
        /// 新建用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public  YaeherUser CreateYaeherUserSingle(YaeherUser YaeherUserInfo)
        {
            try
            {
                YaeherUserInfo.Id =  _repository.InsertAndGetId(YaeherUserInfo);
                return YaeherUserInfo;
            }
            catch (Exception ex)
            {
                return new YaeherUser();
            }
        }
        
        /// <summary>
        /// 修改用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> UpdateYaeherUser(YaeherUser YaeherUserInfo)
        {
            return await _repository.UpdateAsync(YaeherUserInfo);
        }

        /// <summary>
        /// 删除用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> DeleteYaeherUser(YaeherUser YaeherUserInfo)
        {
            return await _repository.UpdateAsync(YaeherUserInfo);
        }
        /// <summary>
        /// 用户基础表 Expression查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> YaeherUserByExpress(Expression<Func<YaeherUser, bool>> whereExpression)
        {
            return await _repository.FirstOrDefaultAsync(whereExpression);
        }
        /// <summary>
        /// 用户基础表list Expression查询
        /// </summary>
        /// <param name="IdArray"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<YaeherUserDocMsg>> YaeherUserListByArray(string  IdArray)
        {
            var sourceIdList = IdArray.Split(',');
            var idList = new int[100];
            for (var i = 0; i < sourceIdList.Count(); i++)
            {
                idList[i] = int.Parse(sourceIdList[i]);
            }
            var queryDoc = _doctorrepository.GetAll();
            var queryUser = _repository.GetAll();

            var query = from a in queryDoc
                        join b in queryUser on a.UserID equals b.Id
                        select new YaeherUserDocMsg
                        {
                            YaeherUserId = b.Id,
                            YaeherDoctorId =a.Id,
                            UserImage=b.UserImage,
                        };
            return await query.Where(t=>idList.Contains(t.YaeherDoctorId)).ToListAsync();
        }
       /// <summary>
       /// 检测用户OpenID
       /// </summary>
       /// <param name="UserOpenID"></param>
       /// <param name="access_token"></param>
       /// <returns></returns>
        [RemoteService(false)]
        public YaeherUser YaeherUserInfoNew(String UserOpenID,string access_token)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    TencentUserManage usermanage = new TencentUserManage();
                    TencentWXPay tencentWXPay = new TencentWXPay();
                    // 通过Openid查询是否存在用户
                    var UserInfo = _repository.GetAll().Where(a => a.WecharOpenID == UserOpenID && a.IsDelete == false).FirstOrDefault();
                    // 获取标签参数
                    var paramlist = _SystemParameterrepository.GetAll().Where(a => a.IsDelete == false && a.SystemCode == "WXRole").ToListAsync().Result;
                    List<Tag> tags = new List<Tag>();
                    if (paramlist != null)
                    {
                        foreach (var tagItem in paramlist)
                        {
                            var Tag = JsonHelper.FromJson<Tag>(tagItem.ItemValue);
                            tags.Add(Tag);
                        }
                    }
                    // 通过OpenId获取用户微信信息
                    var usermsg = usermanage.WeiXinUserInfoUtils(UserOpenID, access_token).Result;
                    // 当OpenID查询无用户时   未关注的用户不做任何处理
                    if (usermsg.subscribe != 0)
                    {
                        if (UserInfo == null)
                        {
                            // 通过昵称判断是否存在 不存在则新增用户  增加返回标签判断
                            int WecharLable = 0;
                            if (usermsg.tagid_list != null && usermsg.tagid_list.Count > 0)
                            {
                                // 当用户存在多标签时，查询是否是医生标签 
                                int DoctorLableId = tags.Where(a => a.name == "doctor").FirstOrDefault().id;
                                if (usermsg.tagid_list.Exists(a => a == DoctorLableId))
                                {
                                    WecharLable = DoctorLableId;
                                }
                                else
                                {
                                    WecharLable = usermsg.tagid_list[0];
                                }
                            }
                            // 通过昵称匹配到的历史数据 
                            if (WecharLable != 0)
                            {
                                UserInfo = _repository.GetAll().Where(a => a.NickName == usermsg.nickname && a.WecharLableId == WecharLable && a.IsDelete == false).FirstOrDefault();
                            }
                            if (UserInfo == null)
                            {
                                string FullName = "YH" + DateTime.Now.ToString("yyyyMMdd") + new RandomCode().GenerateCheckCode(5);
                                var newuser = new YaeherUser();
                                newuser.WecharOpenID = UserOpenID;
                                newuser.UserImage = usermsg.headimgurl;
                                newuser.WecharName = FullName;
                                newuser.FullName = FullName;
                                newuser.Sex = usermsg.sex;
                                newuser.RoleName = "patient";
                                newuser.Userorigin = "WXGZH";//微信公众号
                                newuser.Enabled = true;  // 默认激活
                                newuser.WecharLable = "patient";
                                newuser.LoginName = FullName;
                                newuser.NickName = usermsg.nickname;
                                newuser.WecharLableId = tags.Where(a => a.name == "patient").FirstOrDefault().id; // 将咨询者的标签赋值   //JsonHelper.FromJson<Tag>(Lableparmlist.ItemValue).id;
                                //newuser.WeCharUserJson = JsonHelper.ToJson(usermsg);
                                newuser.CreatedOn = DateTime.Now;
                                newuser.LoginPwd = Commons.DefaultPassword;  // 默认增加密码 
                                newuser.IsLabel = true;
                                newuser.IsPay = false;
                                newuser.IsUpdate = false;
                                newuser.IsProfitSharing = false;
                                newuser.Id = _repository.InsertAndGetIdAsync(newuser).Id;
                                UserInfo = newuser;
                            }
                            else
                            {
                                if (UserInfo.WecharOpenID != UserOpenID)
                                {
                                    UserInfo.IsUpdate = true;
                                    UserInfo.OpenID = "将现有科技注册数据转换：" + UserInfo.WecharOpenID;  //将历史的OpenID存储起来
                                    UserInfo.WecharOpenID = UserOpenID;  // 重新给新的OpenID
                                }
                                if (UserInfo.RoleName != "doctor")
                                {
                                    UserInfo.UserImage = usermsg.headimgurl;
                                }
                                UserInfo.NickName = usermsg.nickname;
                                UserInfo = _repository.Update(UserInfo);
                            }
                        }
                        else
                        {
                            if (UserInfo.WecharOpenID != UserOpenID)
                            {
                                UserInfo.IsUpdate = true;
                                UserInfo.OpenID = "将现有科技注册数据转换：" + UserInfo.WecharOpenID;  //将历史的OpenID存储起来
                                UserInfo.WecharOpenID = UserOpenID;  // 重新给新的OpenID
                            }
                            if (UserInfo.RoleName != "doctor")
                            {
                                UserInfo.UserImage = usermsg.headimgurl;
                            }
                            UserInfo.NickName = usermsg.nickname;
                            UserInfo = _repository.Update(UserInfo);
                        }
                    }
                    unitOfWork.Complete();
                    return UserInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 检测用户OpenID
        /// </summary>
        /// <param name="usermsg"></param>
        /// <param name="access_token"></param>
        /// <param name="OperType"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUser> YaeherUserInfo(TencentFocusUser usermsg,string access_token,String OperType)
        {
            string UserOpenID = usermsg.openid;
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    TencentUserManage usermanage = new TencentUserManage();
                    TencentWXPay tencentWXPay = new TencentWXPay();
                    // 通过Openid查询是否存在用户
                    var UserInfo = await _repository.FirstOrDefaultAsync(a => a.WecharOpenID == usermsg.openid && a.IsDelete == false);
                    // 获取标签参数
                    var paramlist = await _SystemParameterrepository.GetAllListAsync(a => a.IsDelete == false && a.SystemCode == "WXRole");
                   // var paramlist = await _SystemParameterrepository.GetAll().Where(a => a.IsDelete == false && a.SystemCode == "WXRole").ToListAsync();
                    List<Tag> tags = new List<Tag>();
                    if (paramlist != null)
                    {
                        foreach (var tagItem in paramlist)
                        {
                            var Tag = JsonHelper.FromJson<Tag>(tagItem.ItemValue);
                            tags.Add(Tag);
                        }
                    }
                    //  取消获取用户微信信息
                    //var usermsg =await usermanage.WeiXinUserInfoUtils(UserOpenID, access_token);
                    bool IsHistory = false;
                    // 当OpenID查询无用户时   未关注的用户不做任何处理
                    if (usermsg.subscribe != 0)
                    {
                        #region 去掉昵称检查
                        //if (UserInfo == null)
                        //{
                        //    通过昵称判断是否存在 不存在则新增用户  增加返回标签判断
                        //    int WecharLable = 0;
                        //    if (usermsg.tagid_list != null && usermsg.tagid_list.Count > 0)
                        //    {
                        //        // 当用户存在多标签时，查询是否是医生标签 
                        //        int DoctorLableId = tags.Where(a => a.name == "doctor").FirstOrDefault().id;
                        //        if (usermsg.tagid_list.Exists(a => a == DoctorLableId))
                        //        {
                        //            WecharLable = DoctorLableId;
                        //        }
                        //        else
                        //        {
                        //            WecharLable = usermsg.tagid_list[0];
                        //        }
                        //    }
                        //    通过昵称匹配到的历史数据
                        //    if (WecharLable != 0)
                        //    {
                        //        UserInfo = _repository.GetAll().Where(a => a.NickName == usermsg.nickname && a.WecharLableId == WecharLable && a.IsDelete == false).FirstOrDefault();
                        //    }
                        //}
                        #endregion
                        if (UserInfo == null)
                        {
                            string FullName = "YH" + DateTime.Now.ToString("yyyyMMdd") + new RandomCode().GenerateCheckCode(5);
                            var newuser = new YaeherUser();
                            newuser.WecharOpenID = UserOpenID;
                            newuser.UserImage = usermsg.headimgurl;
                            newuser.WecharName = usermsg.nickname;
                            newuser.FullName = usermsg.nickname;
                            newuser.Sex = usermsg.sex;
                            newuser.RoleName = "patient";
                            newuser.Userorigin = "WXGZH";//微信公众号
                            newuser.Enabled = true;  // 默认激活
                            newuser.WecharLable = "patient";
                            newuser.LoginName = FullName;
                            newuser.NickName = usermsg.nickname;
                            newuser.WecharLableId = tags.Where(a => a.name == "patient").FirstOrDefault().id; // 将咨询者的标签赋值   //JsonHelper.FromJson<Tag>(Lableparmlist.ItemValue).id;
                            // newuser.WeCharUserJson = JsonHelper.ToJson(usermsg);
                            newuser.CreatedOn = DateTime.Now;
                            newuser.LoginPwd = Commons.DefaultPassword;  // 默认增加密码 
                            newuser.IsLabel = true;
                            newuser.IsPay = false;
                            newuser.IsUpdate = false;
                            newuser.IsProfitSharing = false;
                            newuser.Id = await _repository.InsertAndGetIdAsync(newuser);
                            UserInfo = newuser;
                        }
                        else
                        {
                            IsHistory = true;  // 如果查询有数据则设置为true
                            UserInfo.IsUpdate = false;
                        }
                        if (!UserInfo.IsPay)
                        {
                            var payment =await _YaeherUserPaymentrepository.FirstOrDefaultAsync(a => a.IsDelete == false && a.UserID == UserInfo.Id);
                            if (payment == null || payment.Id < 1)
                            {
                                //http请求微信信息，获取账户的信息 新增用户payment
                                var CreateUserPayment = new YaeherUserPayment()
                                {
                                    UserID = UserInfo.Id,
                                    FullName = UserInfo.FullName,
                                    PayMethod = "wxpay",
                                    PayMethodName = "微信支付",
                                    PaymentAccout = UserInfo.WecharName,
                                    BankName = "wx",
                                    Subbranch = "wx",
                                    BandAdd = "wx",
                                    BankNo = "wx",
                                    CreatedOn = DateTime.Now,
                                    IsDefault = true,
                                };
                                await _YaeherUserPaymentrepository.InsertAsync(CreateUserPayment);
                            }
                            UserInfo.IsPay = true;
                        }
                        if (!UserInfo.IsUpdate)
                        {
                            // 通过昵称获取的用户信息 去更新历史OpenID 
                            if (IsHistory)
                            {
                                UserInfo.OpenID = "将现有科技注册数据转换：" + UserInfo.WecharOpenID;  //将历史的OpenID存储起来
                                UserInfo.WecharOpenID = UserOpenID;  // 重新给新的OpenID
                            }
                            if (UserInfo.RoleName != "doctor")
                            {
                                UserInfo.UserImage = usermsg.headimgurl;
                            }
                            UserInfo.IsUpdate = true;
                            //当用户为医生时查询分账信息
                            if (UserInfo.RoleName == "doctor")
                            {
                                var DoctorInfo = await _doctorrepository.FirstOrDefaultAsync(a => a.IsDelete == false && a.UserID == UserInfo.Id);
                                if (DoctorInfo != null && DoctorInfo.IsSharing && UserInfo.IsProfitSharing == false)   //医生角色切没有生成分账账号
                                {
                                    //查询分账配置
                                    var configs = await _SystemConfigsrepository.GetAll().Where(a => a.IsDelete == false && a.SystemType == "TencentWechar").ToListAsync();
                                    // 查询医生信息
                                    var tencentparam = configs.FirstOrDefault();
                                    var receiver = new receiver();
                                    receiver.name = DoctorInfo.DoctorName;
                                    receiver.type = "PERSONAL_OPENID";
                                    receiver.account = UserOpenID;
                                    var addresult = await tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam);
                                    if (addresult.result_code == "SUCCESS")  //插入成功后更新状态
                                    {
                                        UserInfo.IsProfitSharing = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (UserInfo.RoleName != "doctor")
                            {
                                UserInfo.UserImage = usermsg.headimgurl;
                            }
                            UserInfo.IsUpdate = true;
                        }
                        await usermanage.YaeherUserLable(usermsg,UserInfo,access_token);
                        // 将用户信息更新
                        if(!UserInfo.IsLabel)
                        {
                            UserInfo.IsLabel = true;
                        }
                        UserInfo.NickName = usermsg.nickname;
                        UserInfo.WecharName = usermsg.nickname;
                        UserInfo.FullName = usermsg.nickname;
                        UserInfo.ModifyOn = DateTime.Now;
                        UserInfo.ModifyBy = UserInfo.Id;
                        UserInfo =await _repository.UpdateAsync(UserInfo);
                        #region 增加有效用户访问记录  暂时取消有效访问量
                        //YaeherOperList yaeherOperList = new YaeherOperList();
                        //yaeherOperList.CreatedOn = DateTime.Now;
                        //yaeherOperList.OperExplain = "UserInfo:" + UserOpenID;
                        //yaeherOperList.OperContent = JsonHelper.ToJson(UserInfo);
                        //yaeherOperList.OperType = OperType;
                        //await _YaeherOperListrepository.InsertAsync(yaeherOperList);
                        #endregion
                    }
                    unitOfWork.Complete();
                    return UserInfo;
                }
            }
            catch (Exception ex)
            {
                #region 增加异常记录信息
                YaeherOperList yaeherOperList = new YaeherOperList();
                yaeherOperList.CreatedOn = DateTime.Now;
                yaeherOperList.OperExplain = "UserInfo:" + UserOpenID;
                yaeherOperList.OperContent ="error:" + ex.ToString();
                yaeherOperList.OperType = "用户操作错误：" + OperType;
                await _YaeherOperListrepository.InsertAsync(yaeherOperList);
                #endregion
                throw ex;
            }
        }
    }
}
