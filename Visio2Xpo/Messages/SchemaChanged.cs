using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.Messages
{
    public class SchemaChanged
    {
        public Schema Schema { get; private set; }
        public SchemaChanged(Schema schema)
        {
            Schema = schema;
        }
    }
}