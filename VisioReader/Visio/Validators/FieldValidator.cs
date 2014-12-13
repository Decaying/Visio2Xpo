using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class FieldValidator : IValidator<Field>
    {
        private readonly IValidator<String> _FieldNameValidator;
        private readonly IList<String> _ValidationErrors;

        public FieldValidator()
        {
            _ValidationErrors = new List<String>();
            _FieldNameValidator = new NameValidator("Field");
        }

        public Boolean Validate(Field validate)
        {
            if (validate == null) throw new ArgumentNullException("validate");

            return _FieldNameValidator.Validate(validate.Name);
        }

        public IEnumerable<String> ValidationErrors
        {
            get
            {
                return _ValidationErrors.Union(_FieldNameValidator.ValidationErrors);
            }
        }
    }
}