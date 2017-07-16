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
            //AbcDef => Abc_Def
            //foreach (var item in Regex.Matches(tableName, "[A-Z]"))
            //{
            //    tableName = tableName.Replace(item.ToString(), item.ToString() + '_');
            //}
        }
    }
}
