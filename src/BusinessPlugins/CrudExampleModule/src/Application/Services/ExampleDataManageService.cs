using AutoMapper;
using System;
using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Services.Bases;
using BusinessPlugins.OrganizationModule.Components.ActionFilters;
using BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces;
using BusinessPlugins.CrudExampleModule.Application.Dtos;
using BusinessPlugins.CrudExampleModule.Domain.Entities;
using BusinessPlugins.CrudExampleModule.Domain.Services;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.CrudExampleModule.Application.Services
{
    /// <summary>
    /// 示例数据管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("示例数据管理服务")]
    public class ExampleDataManageService : ApplicationServiceBase
    {
        private ExampleDataManager _exampleDataManager;

        public ExampleDataManageService(ExampleDataManager exampleDataManager)
        {
            _exampleDataManager = exampleDataManager;
        }

        [Description("搜索数据")]
        [CheckPrivilege(typeof(IAmAdmin), "ExampleData:View")]
        public GridSearchResponseDto Search(GridSearchRequestDto request)
        {
            return request.BuildResponse<ExampleData, Guid>()
                .FilterKeywordWith(t => t.Name)
                .FilterKeywordWith(t => t.Description)
                .ToResponse<ExampleDataOutputDto>();
        }

        [Description("编辑数据")]
        [CheckPrivilege(typeof(IAmAdmin), "ExampleData:Edit")]
        public ActionResponseDto Edit(ExampleDataInputDto dto)
        {
            var data = _exampleDataManager.Get(dto.Id) ?? new ExampleData();
            Mapper.Map(dto, data);
            _exampleDataManager.Save(ref data);
            return ActionResponseDto.CreateSuccess("Saved Successfully");
        }

        [Description("删除数据")]
        [CheckPrivilege(typeof(IAmAdmin), "ExampleData:Remove")]
        public ActionResponseDto Remove(Guid id)
        {
            _exampleDataManager.Delete(id);
            return ActionResponseDto.CreateSuccess("Deleted Successfully");
        }
    }
}
