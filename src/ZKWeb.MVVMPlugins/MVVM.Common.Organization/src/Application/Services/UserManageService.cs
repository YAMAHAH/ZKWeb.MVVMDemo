using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Template;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Module;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Entities;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("用户管理服务")]
    [ComponentClass(typeof(UserManagerModule), typeof(UserOutputDto),
        typeof(DeletedFilter), typeof(CreateTimeFilter))]
    public class UserManageService : ApplicationServiceBase
    {
        private UserManager _userManager;
        private TenantManager _teantManager;

        public UserManageService(
            UserManager userManager,
            TenantManager tenantManager)
        {
            _userManager = userManager;
            _teantManager = tenantManager;
        }

        [Action("Search", HttpMethods.POST)]
        [Description("搜索用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:View")]
        [ComponentMethod("Search", "搜索", true, true)]
        public GridSearchResponseDto Search(GridSearchRequestDto request)
        {
            return request.BuildResponse<User, Guid>()
                .FilterKeywordWith(t => t.Username)
                .FilterKeywordWith(t => t.Remark)
                .FilterColumnWith(
                    nameof(UserOutputDto.Roles),
                    (c, q) =>
                    {
                        var roleIds = c.Value.ConvertOrDefault<IList<Guid>>();
                        if (roleIds != null)
                        {
                            return q.Where(u => u.Roles.Any(r => roleIds.Contains(r.To.Id)));
                        }
                        return q;
                    })
                .ToResponse<UserOutputDto>();
        }

        [Action("test", HttpMethods.GET)]
        [Description("测试基本数据类型")]
        [CheckPrivilege(typeof(IAmAdmin), "User:Test")]
        [ComponentMethod("Test", "测试", true, true)]
        public GridSearchResponseDto Test(string testid)
        {
            return new GridSearchResponseDto(Convert.ToInt64(testid), new List<object>());
        }

        [Action("TestGet", HttpMethods.GET)]
        [Description("测试空参数")]
        [ComponentMethod("TestGet", "测试空参数", true, true)]
        public GridSearchResponseDto TestGet()
        {
            var productRepository = UnitOfWork.GetUnitRepository<Product, Guid>();
            List<Product> products = new List<Product>();

            for (int i = 0; i < 100; i++)
            {
                Product product = new Product()
                {
                    ProductNo = "P010100005NK" + i.ToString(),
                    ProductName = "华司外六角凹⊕",
                    ProductDesc = "1/4-20*18MM白镍.￠13边10H4.6",
                    Dw = "PC"
                };
                products.Add(product);
            }
            IEnumerable<Product> result = products;
            productRepository.Upsert(ref result);

            var pagelists = productRepository.GetPagedList();

            var ngEl = AngularElement.Create("sale-order-container", "saleOrderContainer1");
            var saleOrder = AngularElement.Create("sale-order", "saleOrder1");
            var saleOrderList = AngularElement.Create("sale-order-list", "saleOrderList1");
            var saleOrderDetail = AngularElement.Create("sale-order-detail", "saleOrderDetail1");
            saleOrder.AppendChilds(saleOrderList, saleOrderDetail);
            ngEl.AppendChild(saleOrder);
            System.IO.File.WriteAllText("d:/projects/saleOrder.html", ngEl.ToString(), System.Text.Encoding.UTF8);
            //test report 
            // var reportManager = Injector.Resolve<ReportManager>();
            //reportManager.CreateOrUpdateRootReport();
            return new GridSearchResponseDto(100, new List<object>() { result, pagelists });
        }

        [Action("TestObject", HttpMethods.GET)]
        [Description("测试复杂对象")]
        [CheckPrivilege(typeof(IAmAdmin), "User:TestObject")]
        [ComponentMethod("TestObject", "测试复杂对象", true, true)]
        public GridSearchResponseDto TestObject(string name, TestInput testInputDto)
        {
            return new GridSearchResponseDto(testInputDto.param2, new List<object>() { name });
        }

        [Action("customEdit", HttpMethods.POST)]
        [Description("编辑用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:Edit")]
        [ComponentMethod("Edit", "编辑", true, true)]
        public ActionResponseDto Edit(UserInputDto dto)
        {
            var user = _userManager.Get(dto.Id) ?? new User();
            Mapper.Map(dto, user);
            // 设置用户密码
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.SetPassword(dto.Password);
            }
            else if (user.Id == Guid.Empty)
            {
                throw new BadRequestException("Please provider a password for new user");
            }
            // 保存用户
            _userManager.Save(ref user);
            // 设置角色列表
            if (dto.RoleIds != null)
            {
                _userManager.AssignRoles(user, dto.RoleIds);
            }
            return ActionResponseDto.CreateSuccess("Saved Successfully");
        }

        [Description("删除用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:Remove")]
        [ComponentMethod("Remove", "删除", true, true)]
        public ActionResponseDto Remove(Guid id)
        {
            _userManager.Delete(id);
            return ActionResponseDto.CreateSuccess("Deleted Successfully");
        }

        [Action("", HttpMethods.GET)]
        [Description("获取所有用户类型")]
        [CheckPrivilege(typeof(IAmAdmin))]
        [ComponentMethod("GetAllUserTypes", "获取用户类型", true, true)]
        public IList<UserTypeOutputDto> GetAllUserTypes()
        {
            var userTypes = _userManager.GetAllUserTypes();
            return userTypes.Select(t => new UserTypeOutputDto()
            {
                Type = t.Type,
                Description = new T(t.Type)
            }).ToList();
        }
    }

    public class TestInput : IInputDto
    {
        public string param1 { get; set; }
        public int param2 { get; set; }
    }
}
