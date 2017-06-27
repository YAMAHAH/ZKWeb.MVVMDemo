using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructurePlugins.BaseModule.Components.Global
{
    public interface IScriptPathConfig
    {
        /// <summary>
        /// 生成模块的文件夹路径
        /// </summary>
        string GenerateModuleDirectory { get; set; }

        /// <summary>
        /// 生成业务模块的文件夹路径
        /// </summary>
        string BusinessModuleDirectory { get; set; }
        /// <summary>
        /// 保存数据传输脚本的文件夹名称
        /// </summary>
        string DtosDirectoryName { get; set; }
        /// <summary>
        /// 保存应用服务脚本的文件夹名称
        /// </summary>
        string ServicesDirectoryName { get; set; }
        /// <summary>
        /// 保存翻译脚本的文件夹名称
        /// </summary>
        string TranslationsDirectoryName { get; set; }
        /// <summary>
        /// 保存权限脚本的文件夹名称
        /// </summary>
        string PrivilegesDirectoryName { get; set; }
        /// <summary>
        /// 生成模块的文件名
        /// </summary>
        string GeneratedModuleFilename { get; set; }
        /// <summary>
        /// 转换文件名称
        /// 例
        /// "ExampleService" => "example-service"
        /// "MyHTTPService" => "my-http-service"
        /// "zh-CN" => "zh-cn"
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        string NormalizeFilename(string filename);
        /// <summary>
        /// 转换类名称
        /// 例
        /// "Translation_zh-CN" => "Translation_zh_CN"
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        string NormalizeClassName(string className);
        /// <summary>
        /// 转换变量名称
        /// 例
        /// "a:b" => "a_b"
        string NormalizeVariableName(string variableName);
        /// <summary>
        /// 获取类型的类名称，支持反省
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        string NormalizeClassName(Type type);
    }
}
