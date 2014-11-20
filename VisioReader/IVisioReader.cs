using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    public interface IVisioReader : IDisposable
    {
        IEnumerable<XElement> GetEntities();
        IEnumerable<XElement> GetPrimaryKeys();
        IEnumerable<XElement> GetFields();
    }
}