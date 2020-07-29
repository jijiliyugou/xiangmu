using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common;
using Yaeher.Common.TencentCustom;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 获取token值处理
    /// </summary>
    public class SystemTokenService : ISystemTokenService
    {
        TencentToken tencentToken = new TencentToken();
        private readonly IRepository<SystemToken> _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWorkManager"></param>
        public SystemTokenService(IRepository<SystemToken> repository,
                                  IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 获取token值处理 List
        /// </summary>
        /// <param name="TokenType"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemToken> SystemTokenList(string TokenType)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var Tokens = new TencentTokens();
                var SystemTokens = _repository.GetAll().Where(a => a.IsDelete == false && a.TokenType == TokenType);
                var TokenInfos = await SystemTokens.ToListAsync();
                var TokenInfo = TokenInfos.FirstOrDefault();
                if (TokenInfo != null)
                {
                    // 如果数据超时了重新获取
                    if (TokenInfo.EffectiveTime <= DateTime.Now.AddHours(1))
                    {
                        Tokens = tencentToken.TencentAccessToken(TokenInfo).Result;
                        TokenInfo.access_token = Tokens.access_token;
                        TokenInfo.EffectiveLength = int.Parse(Tokens.expires_in);
                        TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                        TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                        TokenInfo.RefreshEffectiveTime = DateTime.Now;
                        TokenInfo = await _repository.UpdateAsync(TokenInfo);
                    }
                    if (string.IsNullOrEmpty(TokenInfo.access_token))
                    {
                        Tokens = tencentToken.TencentAccessToken(TokenInfo).Result;
                        TokenInfo.access_token = Tokens.access_token;
                        TokenInfo.EffectiveLength = int.Parse(Tokens.expires_in);
                        TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                        TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                        TokenInfo.RefreshEffectiveTime = DateTime.Now;
                        TokenInfo = await _repository.UpdateAsync(TokenInfo);
                    }
                    // 最后验证token有效性
                    var WeCharIP = tencentToken.WeCharIP(TokenInfo).Result;
                    if (WeCharIP.ip_list == null)
                    {
                        Tokens =  tencentToken.TencentAccessToken(TokenInfo).Result;
                        TokenInfo.access_token = Tokens.access_token;
                        TokenInfo.EffectiveLength = int.Parse(Tokens.expires_in);
                        TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                        TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                        TokenInfo.RefreshEffectiveTime = DateTime.Now;
                        TokenInfo = await _repository.UpdateAsync(TokenInfo);
                    }
                }
                else
                {
                    // 当没数据时先获取数据再保存 
                    Tokens = tencentToken.TencentAccessToken(TokenInfo).Result;
                    TokenInfo.access_token = Tokens.access_token;
                    TokenInfo.EffectiveLength = int.Parse(Tokens.expires_in);
                    TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                    TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                    TokenInfo.RefreshEffectiveTime = DateTime.Now;
                    TokenInfo.Id = await _repository.InsertAndGetIdAsync(TokenInfo);
                }
                unitOfWork.Complete();
                return TokenInfo;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenType"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemToken> JSWecharTokenList(string TokenType, SystemTokenIn accesstoken)
        {
            var SystemTokens = _repository.GetAll().Where(a => a.IsDelete == false && a.TokenType == TokenType).OrderByDescending(a => a.CreatedOn);
            var TokenInfos = await SystemTokens.ToListAsync();
            var TokenInfo = TokenInfos.FirstOrDefault();
            if (TokenInfo != null )
            {
                // 如果数据超时了重新获取
                if (TokenInfo.EffectiveTime <= DateTime.Now.AddMinutes(5))
                {
                    var Tokens = await tencentToken.TencentTicket("jsapi", accesstoken.access_token);
                    TokenInfo.access_token = Tokens.ticket;
                    TokenInfo.EffectiveLength = Tokens.expires_in;
                    TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                    TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                    TokenInfo.RefreshEffectiveTime = DateTime.Now;
                    TokenInfo = await _repository.UpdateAsync(TokenInfo);
                }
                if (string.IsNullOrEmpty(TokenInfo.access_token))
                {
                    var Tokens = await tencentToken.TencentTicket("jsapi", accesstoken.access_token);
                    TokenInfo.access_token = Tokens.ticket;
                    TokenInfo.EffectiveLength = Tokens.expires_in;
                    TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                    TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                    TokenInfo.RefreshEffectiveTime = DateTime.Now;
                    TokenInfo = await _repository.UpdateAsync(TokenInfo);
                }
            }
            else
            {
                // 当没数据时先获取数据再保存 
                var Tokens = await tencentToken.TencentTicket("jsapi", accesstoken.access_token);
                TokenInfo = new SystemToken();
                TokenInfo.access_token = Tokens.ticket;
                TokenInfo.EffectiveLength = Tokens.expires_in;
                TokenInfo.EffectiveTime = DateTime.Now.AddSeconds(TokenInfo.EffectiveLength);
                TokenInfo.TokenJson = JsonHelper.ToJson(Tokens);
                TokenInfo.TokenType = TokenType;
                TokenInfo.YaeherPlatform = accesstoken.YaeherPlatform;
                TokenInfo.Appid = accesstoken.Appid;
                TokenInfo.AppSecret = accesstoken.AppSecret;
                TokenInfo.RefreshEffectiveTime = DateTime.Now;
                TokenInfo.Id = await _repository.InsertAndGetIdAsync(TokenInfo);
            }
            return TokenInfo;

        }
    }
}
