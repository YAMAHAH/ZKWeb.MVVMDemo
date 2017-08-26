namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    public interface IDtoToModelMapProfileBase
    {
        /// <summary>
        /// 配置文件的DTO全名
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// 注册到容器
        /// </summary>
        void RegisterToContiner();
        /// <summary>
        /// 获取成员的配置值
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        IDtoToModelMapValueBase GetMemberMapValue(string memberName);
    }
}
