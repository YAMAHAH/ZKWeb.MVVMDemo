using System.Collections.Generic;

namespace InfrastructurePlugins.BaseModule.Domain.TreeStruct
{
    public class ManyTreeNodeInfo
    {
        public IList<object> NodeIds { get; set; }
        public IList<object> RootIds { get; set; }
    }
}
