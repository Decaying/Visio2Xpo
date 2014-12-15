using System.Collections;
using System.Collections.Generic;
using cvo.buyshans.Visio2Xpo.Data;
using DevExpress.Xpf.Grid;

namespace cvo.buyshans.Visio2Xpo.UI.Selectors
{
    public class EntitySelector : IChildNodesSelector
    {
        public IEnumerable SelectChildren(object item)
        {
            var schema = item as Schema;
            if (schema != null)
            {
                return schema.Tables;
            }
            return null;
        }
    }
}