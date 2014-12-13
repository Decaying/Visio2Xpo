using System;
using System.Collections.Generic;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class SchemaValidator : IValidator<Schema>
    {
        private readonly IList<string> _ValidationErrors;

        public SchemaValidator()
        {
            _ValidationErrors = new List<string>();
        }

        public bool Validate(Schema validate)
        {
            if (validate == null) throw new ArgumentNullException("validate");

            return true;
        }

        public IEnumerable<string> ValidationErrors()
        {
            return _ValidationErrors;
        }
    }
}