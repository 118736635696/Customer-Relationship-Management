﻿using AutoMapper;
using CustomerRelationshipManagement.Dtos;
using CustomerRelationshipManagement.Users;

namespace CustomerRelationshipManagement;

public class CustomerRelationshipManagementApplicationAutoMapperProfile : Profile
{
    public CustomerRelationshipManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        //用户信息映射
        CreateMap<UserInfo, UserInfoDto>().ReverseMap();
    }
}
