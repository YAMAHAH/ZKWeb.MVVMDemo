using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Template
{
    public abstract class TemplateElement
    {
        #region 数据存储
        private Dictionary<string, string> xElementProperties = new Dictionary<string, string>();
        public Dictionary<string, string> ElementProperties
        {
            get
            {
                return xElementProperties;
            }
        }
        private Dictionary<string, string> xElementEvents = new Dictionary<string, string>();
        public Dictionary<string, string> ElementEvents
        {
            get
            {
                return xElementEvents;
            }
        }
        #endregion

        #region 基本属性
        /// <summary>
        /// 元素标签
        /// </summary>
        public string ElementTag { get; set; }
        /// <summary>
        /// 元素名称，最好唯一值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父亲元素
        /// </summary>
        public TemplateElement Parent { get; set; }
        /// <summary>
        /// 元素所在的层数
        /// </summary>
        public int Level { get; set; }
        #endregion

        #region 子元素操作
        private List<TemplateElement> xChildElements;
        public List<TemplateElement> ChildElements
        {
            get
            {
                if (xChildElements == null) xChildElements = new List<TemplateElement>();
                return xChildElements;
            }
        }
        public void AppendChild(TemplateElement element)
        {
            if (element != null)
            {
                ChildElements.Add(element);
                element.Parent = this;
                //修正level
                NormalizationLevel(element);
            }
        }
        public void AppendChilds(params TemplateElement[] elements)
        {
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    ChildElements.Add(element);
                    element.Parent = this;
                    //修正level
                    NormalizationLevel(element);
                }
            }
        }

        private void NormalizationLevel(TemplateElement element)
        {
            element.Level = element.Parent.Level + 1;
            foreach (var child in element.ChildElements)
            {
                NormalizationLevel(child);
            }
        }
        public void RemoveElement(TemplateElement element)
        {
            if (xChildElements.Contains(element))
            {
                xChildElements.Remove(element);
            }
        }

        public void RemoveElements(params TemplateElement[] elements)
        {
            if (elements != null)
            {
                foreach (var el in elements)
                {
                    if (xChildElements.Contains(el)) xChildElements.Remove(el);
                }
            }
        }

        public void Clear()
        {
            ChildElements.Clear();
        }
        #endregion

        #region 元素属性操作
        public void AddProperty(string key, string value)
        {
            var pKey = NormalizationPropertyKey(key);
            ElementProperties[pKey] = value;
        }
        public void AddProperties(List<KeyValuePair<string, string>> properties)
        {
            foreach (var prop in properties)
            {
                var pKey = NormalizationPropertyKey(prop.Key);
                ElementProperties[pKey] = prop.Value;
            }
        }
        public void RemoveProperty(string key)
        {
            var pKey = NormalizationEventKey(key);
            if (ElementProperties.ContainsKey(pKey))
            {
                ElementProperties.Remove(pKey);
            }
        }
        public void RemoveProperties(List<string> keys)
        {
            foreach (var key in keys)
            {
                var pKey =NormalizationPropertyKey(key);
                if (ElementProperties.ContainsKey(pKey)) { ElementProperties.Remove(pKey); }
            }
        }
        public void ClearProperies()
        {
            ElementProperties.Clear();
        }

        public virtual string NormalizationPropertyKey(string key)
        {
            return key;
        }
        #endregion

        #region 元素事件操作
        public void AddEvent(string key, string value)
        {
            ElementEvents[NormalizationEventKey(key)] = value;
        }
        public void AddEvents(List<KeyValuePair<string, string>> events)
        {
            foreach (var ev in events)
            {
                ElementEvents[NormalizationEventKey(ev.Key)] = ev.Value;
            }
        }
        public void RemoveEvent(string key)
        {
            var evKey = NormalizationEventKey(key);
            if (ElementEvents.ContainsKey(evKey))
            {
                ElementEvents.Remove(evKey);
            }
        }
        public void RemoveEvents(List<string> keys)
        {
            foreach (var key in keys)
            {
                var evKey = NormalizationEventKey(key);
                if (ElementEvents.ContainsKey(evKey)) { ElementEvents.Remove(evKey); }
            }
        }

        public void ClearEvents()
        {
            ElementEvents.Clear();
        }

        public virtual string NormalizationEventKey(string key)
        {
            return key;
        }
        #endregion

        #region 元素文本序列化处理
        public override string ToString()
        {
            //元素属性处理
            var propString = ElementPropertyHandler();
            //元素事件处理
            var eventString = ElementEventHandler();
            //子元素处理
            var childString = ChildElementHandler();
            var indent = " ".Repeat(this.Level * 4);
            //获取本身的字符串及子结点生成的字符串
            return indent + "<" + ElementTag + propString + eventString + ">\n" +
                       childString +
                   indent + "</" + ElementTag + ">\n";
        }

        private string ElementPropertyHandler()
        {
            var result = string.Empty;
            foreach (var p in ElementProperties)
            {
                result += " " + p.Key + "=" + '"' + p.Value + '"';
            }
            return result;
        }

        private string ElementEventHandler()
        {
            var result = string.Empty;
            foreach (var e in ElementEvents)
            {
                result += " " + e.Key + "=" + '"' + e.Value + '"';
            }
            return result;
        }

        private string ChildElementHandler()
        {
            return GetChildElementTemplateString(this);
        }

        private string GetChildElementTemplateString(TemplateElement element)
        {
            var result = string.Empty;
            foreach (var child in element.ChildElements)
            {
                result += child.ToString();
            }
            return result;
        }
        #endregion
    }
}
