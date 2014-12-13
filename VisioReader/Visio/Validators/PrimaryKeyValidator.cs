using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class PrimaryKeyValidator : IValidator<PrimaryKey>
    {
        private readonly IValidator<String> _PrimaryKeyNameValidator;
        private readonly IList<String> _ValidationErrors;

        public PrimaryKeyValidator()
        {
            _ValidationErrors = new List<String>();
            _PrimaryKeyNameValidator = new NameValidator("Primary key");
        }

        public Boolean Validate(PrimaryKey validate)
        {
            if (validate == null) throw new ArgumentNullException("validate");

            return _PrimaryKeyNameValidator.Validate(validate.Name);
        }

        public IEnumerable<String> ValidationErrors()
        {
            return _ValidationErrors.Union(_PrimaryKeyNameValidator.ValidationErrors());
        }
    }
}