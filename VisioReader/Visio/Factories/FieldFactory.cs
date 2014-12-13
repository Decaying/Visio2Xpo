using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public class FieldFactory : IFactory<Field>
    {
        private const string ShapeNameAttribute = "Attribute";

        private readonly IVisioReader _VisioReader;
        private readonly IValidator<Field> _Validator;

        public FieldFactory(IVisioReader visioReader, IValidator<Field> validator)
        {
            if (visioReader == null) throw new ArgumentNullException("visioReader");
            if (validator == null) throw new ArgumentNullException("validator");

            _VisioReader = visioReader;
            _Validator = validator;
        }

        public IValidator<Field> Validator
        {
            get
            {
                return _Validator;
            }
        }

        public int MasterId
        {
            get
            {
                return _VisioReader.GetMasterId(ShapeNameAttribute);
            }
        }

        public Field Create(XElement element = null)
        {
            if (element == null) throw new ArgumentNullException("element");

            var field = new Field
            {
                Name = _VisioReader.GetName(element),
                BaseType = "String" //todo: base type in Visio
            };

            return Validator.Validate(field) ? field : null;
        }

        public IEnumerable<IFactory> ChildFactories
        {
            get
            {
                return null;
            }
        }

        public Boolean HasErrors()
        {
            return Validator.ValidationErrors.Any();
        }

        public IEnumerable<String> GetErrors()
        {
            return new List<String>(Validator.ValidationErrors);
        }
    }
}