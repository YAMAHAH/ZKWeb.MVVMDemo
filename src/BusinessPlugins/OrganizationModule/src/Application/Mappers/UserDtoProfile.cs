using BusinessPlugins.OrganizationModule.Application.Dtos;
using BusinessPlugins.OrganizationModule.Application.Services;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Extensions;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Mappers
{
    /// <summary>
    /// 用户DTO和用户实体对应关系配置
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserDtoProfile : DtoToModelMapProfile<User, UserOutputDto, Guid>
    {
        public UserDtoProfile()
        {
            BelongTo(typeof(UserManageService));
            FilterKeywordWith(u => new { u.Username, u.Remark })
             .ForMember(r => r.CreateTime, opt => opt.Map(m => m.CreateTime.ToString()))
             .ForMember(r => r.UpdateTime, opt => opt.Map(m => m.UpdateTime.ToString()))
             .ForMember(r => r.Id, opt => opt.Map(m => m.Id)
                        .MapObjectDictInfo(a =>
                        {
                            a.Editable = true;
                            a.Text = "主键";
                            a.required = true;
                        })
             )
             .ForMember(r => r.Username, opt => opt.Map(u => u.Username))
             .ForMember(r => r.OwnerTenantName, (opt) => opt.Map(t => t.OwnerTenant.Name))
             .ForMember(r => r.OwnerTenantId, opt => opt.Map(r => r.OwnerTenantId))
             .ForMember(r => r.Remark, opt => opt.Map(r => r.Remark))
             .ForMember(r => r.Deleted, opt => opt.Map(r => r.Deleted))
             .ForMember(r => r.Type, opt => opt.Map(r => r.Type))
             .ForMember(r => r.Privileges, opt => opt.Map(r => r.GetPrivileges()))
             .ForMember(u => u.RoleIds, opt => opt.Map(u => u.Roles.Select(r => r.To.Id).ToList()))
             .ForMember(u => u.Roles, opt => opt.MapColumnFilterWrapper(c =>
             {
                 ////****************请求的是复杂对象列表(any,all)************************
                 ////创建基于UserToRole表达式工厂
                 //var childExprFactory = new ExpressionCreateFactory<UserToRole, UserToRoleOutputDto, Guid>();
                 ////创建表达式树生成器
                 //var lbdExprBuilder = childExprFactory.CreateBuilder();
                 ////创建子查询表达式
                 //Expression<Func<UserToRole, bool>> childQueryExpr = childExprFactory.CreateChildQueryExpression(c.Childs.ToArray());
                 ////创建基于User表达式工厂
                 //var anyExprFactory = new ExpressionCreateFactory<User, UserOutputDto, Guid>();
                 ////创建表达式生成器
                 //var anyExprBuilder = anyExprFactory.CreateBuilder();
                 ////创建any方法表达式
                 //var anyExpr = anyExprBuilder.Any(c.Column, childQueryExpr);
                 ////创建e.Roles.Any(childQueryExpr)
                 //var resultExpr = anyExprBuilder.GetLambdaExpression(anyExpr);
                 //////***************请求的是对象类型u=>u.Role.Second.Id == "myID" || u.Role.Second.Name.contains("L3")*************************************
                 //////创建对象成员
                 //////var memberExpr = anyExprBuilder.GetPropertyExpression(c.Column);

                 //return resultExpr;
                 var roleIds = c.Value.ConvertOrDefault<IList<Guid>>();
                 if (roleIds != null)
                 {
                     Expression<Func<User, bool>> expr = e => Regex.IsMatch(e.Id.ToString(), ".*") && e.Roles.Any(r => roleIds.Contains(r.To.Id));
                     return expr;
                 }
                 return u => false;
             }))

             ;
        }

    }
}
