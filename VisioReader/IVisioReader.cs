using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    public interface IVisioReader : IDisposable
    {
        IEnumerable<XElement> GetElementsByName(String shapeName);
    }
}