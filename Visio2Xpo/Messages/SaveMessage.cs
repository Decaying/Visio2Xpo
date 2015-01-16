using System.Diagnostics;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.Messages
{
    public class SaveMessage
    {
        public Schema Schema { get; private set; }

        public SaveMessage(Schema schema)
        {
            Schema = schema;
        }
    }
}