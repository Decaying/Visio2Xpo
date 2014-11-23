using System.Collections.Generic;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    internal interface IVisioDataFactory
    {
        IEnumerable<Field> GetFields();
        IEnumerable<Entity> GetEntities();
        IEnumerable<PrimaryKey> GetPrimaryKeys();
    }
}