namespace InfrastructurePlugins.BaseModule.Components.CORSExposeHeaders.Interfaces
{
    /// <summary>
    /// 允许跨站请求返回的头
    /// </summary>
    public interface ICORSExposeHeader
    {
        string ExposeHeader { get; }
    }
}
