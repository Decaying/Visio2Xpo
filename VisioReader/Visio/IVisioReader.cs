using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio
{
    public interface IVisioReader : IDisposable
    {
        IEnumerable<XElement> GetElements(Int32 masterId);
        IEnumerable<Int32> GetRelatedElementIDs(XElement element);
        String GetName(XElement element);
        Int32 GetMasterId(String masterName);
        Int32 GetMasterId(XElement element);
        XElement GetElement(Int32 id);
    }
}