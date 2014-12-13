using System;
using System.Collections;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public interface IFactory<T>
    {
        IValidator<T> Validator { get; }
        Int32 MasterId { get; }
        T Create(XElement element = null);
    }
}