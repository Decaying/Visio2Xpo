using System;
using System.Collections.Generic;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class NameValidator : IValidator<String>
    {
        private readonly IList<String> _ValidationErrors;
        private readonly String _EntityName;

        private const Int32 MaxLength = 40;

        public NameValidator(String entityName)
        {
            _ValidationErrors = new List<String>();
            _EntityName = entityName;
        }

        public bool Validate(String validate)
        {
            var validated = true;

            if (String.IsNullOrWhiteSpace(validate))
            {
                _ValidationErrors.Add(String.Format("{0} name cannot be empty", _EntityName));
                validated = false;
            }

            if (validated && validate.Contains(" "))
            {
                _ValidationErrors.Add(String.Format("{0} name cannot contain spaces ({1})", _EntityName, validate));
                validated = false;
            }

            if (validated && validate.StartsWithNumber())
            {
                _ValidationErrors.Add(String.Format("{0} name cannot start with a number ({1})", _EntityName, validate));
                validated = false;
            }

            if (validated && validate.Length > MaxLength)
            {
                _ValidationErrors.Add(String.Format("{0} name cannot have more than {1} characters ({2})", _EntityName, MaxLength, validate));
                validated = false;
            }

            if (validated && validate.ContainsSpecialCharacters())
            {
                _ValidationErrors.Add(String.Format("{0} name can only contain letters, numbers and _ ({1})", _EntityName, validate));
                validated = false;
            }

            return validated;
        }

        public IEnumerable<String> ValidationErrors
        {
            get
            {
                return _ValidationErrors;
            }
        }
    }
}