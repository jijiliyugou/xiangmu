using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public interface IYaeherUserService : IApplicationService
    {
        /// <summary>
        /// 新建用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        Task<YaeherUser> CreateYaeherUser(YaeherUser YaeherUserInfo);
        /// <summary>
        /// 新建用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        YaeherUser CreateYaeherUserSingle(YaeherUser YaeherUserInfo);
        /// <summary>
        /// 删除用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        Task<YaeherUser> DeleteYaeherUser(YaeherUser YaeherUserInfo);
        /// <summary>
        /// 修改用户基础表
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        Task<YaeherUser> UpdateYaeherUser(YaeherUser YaeherUserInfo);
        /// <summary>
        /// 用户基础表byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherUser> YaeherUserByID(int Id);
        /// <summary>
        /// 用户基础表 List
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        Task<List<YaeherUser>> YaeherUserList(YaeherUserIn YaeherUserInfo);
        /// <summary>
        /// 用户基础表 page
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherUser>> YaeherUserPage(YaeherUserIn YaeherUserInfo);
        /// <summary>
        /// 用户基础表 Expression查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<YaeherUser> YaeherUserByExpress(Expression<Func<YaeherUser, bool>> whereExpression);
        /// <summary>
        /// 用户基础表 Expression查询
        /// </summary>
        /// <param name="IdArray"></param>
        /// <returns></returns>
        Task<List<YaeherUserDocMsg>> YaeherUserListByArray(string IdArray);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserOpenID"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        YaeherUser YaeherUserInfoNew(String UserOpenID,string access_token);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usermsg"></param>
        /// <param name="access_token"></param>
        /// <param name="OperType"></param>
        /// <returns></returns>
        Task<YaeherUser> YaeherUserInfo(TencentFocusUser usermsg, string access_token,String OperType);
    }
}