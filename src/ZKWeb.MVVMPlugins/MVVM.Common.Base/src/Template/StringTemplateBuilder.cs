using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Template
{
    public class StringTemplateBuilder
    {
        #region 基本属性
        /// <summary>
        /// 拼接新组时是否换行
        /// </summary>
        public bool NewLine { get; set; } = true;
        public bool NeedJoinSymbol { get; set; } = true;
        /// <summary>
        /// 组与组的连接字符
        /// </summary>
        public string JoinSymbol { get; set; } = ",";
        /// <summary>
        /// 字符串模板
        /// </summary>
        public string Template { get; set; }

        public List<Dictionary<string, string>> GroupValues { get; set; }
        public StringTemplateBuilder()
        {
            this.GroupValues = new List<Dictionary<string, string>>();
        }
        public StringTemplateBuilder(string template):this()
        {
            Template = template;
        }
        #endregion
        #region 数据操作
        public string TransformText(string template = "")
        {
            if (template != string.Empty) Template = template;
            List<string> results = new List<string>();

            foreach (var child in GroupValues)
            {
                var result = this.Template;
                foreach (var item in child)
                {
                    result = result.Replace("{{" + item.Key + "}}", item.Value);
                }
                results.Add(result);
            }
            var stringResult = string.Empty;
            foreach (var item in results)
            {
                stringResult += item;
                if (item != results.Last())
                {
                    stringResult += (NeedJoinSymbol ? JoinSymbol : string.Empty) + (NewLine ? "\n" : string.Empty);
                }
            }
            return stringResult;
        }
        public void Clear()
        {
            GroupValues.Clear();
        }
        public void AddGroupValues(Dictionary<string, string> groupValues)
        {
            GroupValues.Add(groupValues);
        }
        public void RemoveGroupValues(Dictionary<string, string> groupValues)
        {
            GroupValues.Remove(groupValues);
        }
        #endregion
    }
}
