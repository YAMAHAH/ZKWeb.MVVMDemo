namespace InfrastructurePlugins.BaseModule.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// 使用分隔符串联数组元素生成字符串
        /// </summary>
        /// <param name="sep">分隔符</param>
        /// <param name="objects">数组元素</param>
        /// <returns></returns>
        public static string GenerateKey(string sep, params object[] objects)
        {
            return string.Join(sep, objects);
        }
    }
}
