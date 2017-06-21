using System;
using System.ComponentModel;
using InfrastructurePlugins.BaseModule.Application.Dtos;

namespace BusinessPlugins.CrudExampleModule.Application.Dtos
{
    [Description("数据的传入信息")]
    public class ExampleDataInputDto : IInputDto
    {
        [Description("数据Id")]
        public Guid Id { get; set; }
        [Description("数据名称")]
        public string Name { get; set; }
        [Description("数据描述")]
        public string Description { get; set; }
    }
}
