using System.Collections.Generic;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio
{
    public interface IVisioDataFactory
    {
        Schema ReadSchema();
        IEnumerable<Table> ReadTables();
    }
}