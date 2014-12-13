using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class SchemaValidator : IValidator<Schema>
    {
        private readonly IList<String> _ValidationErrors;

        public SchemaValidator()
        {
            _ValidationErrors = new List<String>();
        }

        public bool Validate(Schema validate)
        {
            if (validate == null) throw new ArgumentNullException("validate");

            return true;
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