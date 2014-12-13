using System;
using System.Collections.Generic;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public interface IValidator<in T>
    {
        Boolean Validate(T validate);
        IEnumerable<String> ValidationErrors();
    }
}