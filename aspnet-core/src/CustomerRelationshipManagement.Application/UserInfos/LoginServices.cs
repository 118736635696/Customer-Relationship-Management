﻿using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos;
using CustomerRelationshipManagement.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.UserInfos
{
    public class LoginServices : ApplicationService, ILogServices
    {
        /// <summary>
        /// 用户信息仓储
        /// </summary>
        private readonly IRepository<UserInfo, Guid> userInfoRepository;
        private readonly IPasswordHasher<UserInfo> passwordHasher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfoRepository"></param>
        public LoginServices(IRepository<UserInfo, Guid> userInfoRepository, IPasswordHasher<UserInfo> passwordHasher)
        {
            this.userInfoRepository = userInfoRepository;
            this.passwordHasher = passwordHasher;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto">用户名和密码</param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfoDto>> Login([FromQuery] LoginDto loginDto)
        {
            try
            {
                var userInfo = await userInfoRepository.FindAsync(x => x.UserName == loginDto.UserName);
                if (userInfo == null)
                {
                    throw new UserFriendlyException("用户不存在");
                }
                //密码加密
                var password = (passwordHasher.VerifyHashedPassword(userInfo, userInfo.Password, loginDto.Password));
                if (!userInfo.IsActive)
                {
                    throw new UserFriendlyException("用户已被禁用");
                }
                if (password != PasswordVerificationResult.Success)
                {
                    throw new UserFriendlyException("密码错误");
                }
                //登录成功，返回用户信息和令牌
                var userInfoDto = ObjectMapper.Map<UserInfo, UserInfoDto>(userInfo);
                //返回数据
                return ApiResult<UserInfoDto>.Success(ResultCode.Success, userInfoDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
