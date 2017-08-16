using FluentValidation;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Application.Validators
{
    [ExportMany, SingletonReuse]
    public class GridSearchRequestValidation : AbstractValidator<GridSearchRequestDto>
    {
        public GridSearchRequestValidation()
        {
            RuleFor(c => c.PageSize).GreaterThan(0).WithMessage(new T("{0} must be greater than zero", "Page size"));
        }
    }
}
