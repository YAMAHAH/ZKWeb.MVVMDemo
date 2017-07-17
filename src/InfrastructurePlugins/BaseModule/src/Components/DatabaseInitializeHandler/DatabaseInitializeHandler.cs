using System.Text.RegularExpressions;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.DatabaseInitializeHandler
{
    /// <summary>
    /// 添加表前缀
    /// </summary>
    [ExportMany]
    public class DatabaseInitializeHandler : IDatabaseInitializeHandler
    {
        public void ConvertTableName(ref string tableName)
        {
            //[A-Z] [a-z]+(?=[A-Z])|[A-Z] (?=[A-Z] [a-z]+(?=[A-Z]))

            Regex reg = new Regex(@"[A-Z]([a-z]+|(?=[A-Z][a-z]+))(?=[A-Z])");
            tableName = reg.Replace(tableName, m => m.Value + "_");
        }
    }
}
