using BusinessPlugins.CrudExampleModule.Application.Dtos;
using BusinessPlugins.CrudExampleModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.CrudExampleModule.Application.Mappers
{
    /// <summary>
    /// 增删改查实例Model和Dto对应关系配置
    /// </summary>
    [ExportMany]
    public class CurdExampleDtoProfile : DtoToModelMapProfile<ExampleData, ExampleDataOutputDto, Guid>
    {
        public CurdExampleDtoProfile()
        {
            FilterKeywordWith(t => new { t.Name, t.Description })
                .ForMember(u => u.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
                .ForMember(u => u.Description, opt => opt.Map(m => m.Description))
                .ForMember(d => d.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()).MapObjectDictInfo(m => { m.Editable = true; }))
                .ForMember(d => d.Name, opt => opt.Map(u => u.Name))
                .ForMember(d => d.Id, (opt) => opt.Map(t => t.Id));
        }
    }
}
