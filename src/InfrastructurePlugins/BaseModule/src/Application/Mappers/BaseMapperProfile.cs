﻿using AutoMapper;
using System;
using ZKWebStandard.Ioc;
using ZKWebStandard.Extensions;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;

namespace InfrastructurePlugins.BaseModule.Application.Mappers
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class BaseMapperProfile : Profile
    {
        public BaseMapperProfile()
        {
            // 转换时间时处理时区
            CreateMap<DateTime, string>().ConvertUsing(d => d.ToClientTimeString());
            CreateMap<string, DateTime>().ConvertUsing(s => s.ConvertOrDefault<DateTime>().FromClientTime());

            CreateMap<GridSearchColumnFilter, ColumnQueryCondition>();
            CreateMap<ColumnQueryCondition, GridSearchColumnFilter>();
        }
    }
}
