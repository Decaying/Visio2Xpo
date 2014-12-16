using System.Collections;
using cvo.buyshans.Visio2Xpo.Data;
using cvo.buyshans.Visio2Xpo.UI.ViewModels;
using DevExpress.Xpf.Grid;

namespace cvo.buyshans.Visio2Xpo.UI.Selectors
{
    public class EntitySelector : IChildNodesSelector
    {
        public IEnumerable SelectChildren(object item)
        {
            var schema = item as SchemaViewModel;
            if (schema != null)
            {
                return schema.Tables;
            }
            var table = item as TableViewModel;
            if (table != null)
            {
                return table.Fields;
            }
            return null;
        }
    }
}