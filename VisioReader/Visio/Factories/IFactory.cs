using System;
using System.Collections.Generic;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public interface IFactory
    {
        IEnumerable<IFactory> ChildFactories { get; }
        Boolean HasErrors();
        IEnumerable<String> GetErrors();
    }

    public interface IFactory<T> : IFactory
    {
        IValidator<T> Validator { get; }
        Int32 MasterId { get; }
        T Create(XElement element = null);
    }
}