using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public class YaeherUserOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherUserDto"></param>
        /// <param name="YaeherUserInfo"></param>
        public YaeherUserOut(PagedResultDto<YaeherUser> YaeherUserDto, YaeherUserIn YaeherUserInfo)
        {
            Items = YaeherUserDto.Items;
            TotalCount = YaeherUserDto.TotalCount;
            TotalPage = YaeherUserDto.TotalCount / YaeherUserInfo.MaxResultCount;
            SkipCount = YaeherUserInfo.SkipCount;
            MaxResultCount = YaeherUserInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherUser> Items { get; set; }

    }
    #region
    //public class YaeherDecodeUserOut : PagerViewModel
    //{
    //    /// <summary>
    //    /// 输出模型
    //    /// </summary>
    //    /// <param name="YaeherUserDto"></param>
    //    /// <param name="YaeherUserInfo"></param>
    //    public YaeherDecodeUserOut(PagedResultDto<YaeherUser> YaeherUserDto, YaeherUserIn YaeherUserInfo)
    //    {
    //        Items = YaeherUserDto.Items.Select(t => new YaeherDecodeUser(t)).ToList();
    //        TotalCount = YaeherUserDto.TotalCount;
    //        TotalPage = YaeherUserDto.TotalCount / YaeherUserInfo.MaxResultCount;
    //        SkipCount = YaeherUserInfo.SkipCount;
    //        MaxResultCount = YaeherUserInfo.MaxResultCount;
    //    }
    //    /// <summary>
    //    /// 数据集
    //    /// </summary>
    //    public IReadOnlyList<YaeherDecodeUser> Items { get; set; }
    //}
    //public class YaeherDecodeUser : YaeherUser
    //{
    //    public YaeherDecodeUser(YaeherUser item)
    //    {
    //        LoginName = item.LoginName;
    //        LoginPwd = item.LoginPwd;
    //        FullName = item.FullName;
    //        PhoneNumber = item.PhoneNumber;
    //        Email = item.Email;
    //        IDCard = item.IDCard;
    //        Sex = item.Sex;
    //        Birthday = item.Birthday;
    //        Enabled = item.Enabled;
    //        ErrorCount = item.ErrorCount;
    //        LoginCount = item.LoginCount;
    //        Userorigin = item.Userorigin;
    //        WecharNo = item.WecharNo;
    //        WecharName = item.WecharName;
    //        WecharOpenID = item.WecharOpenID;
    //        UserImage = item.UserImage;
    //        RoleName = item.RoleName;
    //        WecharLable = item.WecharLable;
    //        WecharLableId = item.WecharLableId;
    //        NickName = item.NickName;
    //        OpenID = item.OpenID;
    //        IsLabel = item.IsLabel;
    //        IsPay = item.IsPay;
    //        IsUpdate = item.IsUpdate;
    //        IsProfitSharing = item.IsProfitSharing;
    //        CreatedOn = item.CreatedOn;
    //        CreatedBy = item.CreatedBy;
    //        ModifyOn = item.ModifyOn;
    //        ModifyBy = item.ModifyBy;
    //        IsDelete = item.IsDelete;
    //        Id = item.Id;
    //    }
    //}
    #endregion
}
