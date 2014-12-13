using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class FieldValidator : IValidator<Field>
    {
        private readonly IList<string> _ValidationErrors;
        private readonly IValidator<String> _FieldNameValidator; 

        public FieldValidator()
        {
            _ValidationErrors = new List<String>();
            _FieldNameValidator = new NameValidator("Field");
        }

        public Boolean Validate(Field validate)
        {
            return _FieldNameValidator.Validate(validate.Name);
        }

        public IEnumerable<String> ValidationErrors()
        {
            return _ValidationErrors.Union(_FieldNameValidator.ValidationErrors());
        }
    }
}