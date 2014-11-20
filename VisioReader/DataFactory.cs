using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    class DataFactory
    {
        public static IEnumerable<PrimaryKey> CreatePrimaryKeys(IEnumerable<XElement> primaryKeysData)
        {
            return primaryKeysData.Select(element => new PrimaryKey());
        }
        public static IEnumerable<Entity> CreateEntities(IEnumerable<XElement> entitiesData)
        {
            return entitiesData.Select(element => new Entity());
        }
        public static IEnumerable<Field> CreateFields(IEnumerable<XElement> fieldsData)
        {
            return fieldsData.Select(element => new Field());
        }
    }
}
