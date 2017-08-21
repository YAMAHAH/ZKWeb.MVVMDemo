namespace InfrastructurePlugins.BaseModule.src.Components.DtoToModelMap
{
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}
