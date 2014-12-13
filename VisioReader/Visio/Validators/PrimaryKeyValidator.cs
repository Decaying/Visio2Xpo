using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class PrimaryKeyValidator : IValidator<PrimaryKey>
    {
        private readonly IList<string> _ValidationErrors;
        private readonly IValidator<String> _PrimaryKeyNameValidator; 

        public PrimaryKeyValidator()
        {
            _ValidationErrors = new List<String>();
            _PrimaryKeyNameValidator = new NameValidator("Primary key");
        }

        public Boolean Validate(PrimaryKey validate)
        {
            return _PrimaryKeyNameValidator.Validate(validate.Name);
        }

        public IEnumerable<String> ValidationErrors()
        {
            return _ValidationErrors.Union(_PrimaryKeyNameValidator.ValidationErrors());
        }
    }
}