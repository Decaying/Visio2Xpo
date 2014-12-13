using System;
using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public class TableValidator : IValidator<Table>
    {
        private readonly IValidator<String> _TableNameValidator;
        private readonly IList<String> _ValidationErrors;

        public TableValidator()
        {
            _ValidationErrors = new List<String>();
            _TableNameValidator = new NameValidator("Table");
        }

        public Boolean Validate(Table validate)
        {
            if (validate == null) throw new ArgumentNullException("validate");

            return _TableNameValidator.Validate(validate.Name);
        }

        public IEnumerable<String> ValidationErrors
        {
            get
            {
                return _ValidationErrors.Union(_TableNameValidator.ValidationErrors);
            }
        }
    }
}