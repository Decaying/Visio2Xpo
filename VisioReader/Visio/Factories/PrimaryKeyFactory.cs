using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public class PrimaryKeyFactory : IFactory<PrimaryKey>
    {
        private const string ShapeNamePrimaryKeyAttribute = "Primary Key Attribute";

        private readonly IVisioReader _VisioReader;
        private readonly IValidator<PrimaryKey> _Validator;

        public PrimaryKeyFactory(IVisioReader visioReader, IValidator<PrimaryKey> validator)
        {
            if (visioReader == null) throw new ArgumentNullException("visioReader");
            if (validator == null) throw new ArgumentNullException("validator");

            _VisioReader = visioReader;
            _Validator = validator;
        }

        public IValidator<PrimaryKey> Validator
        {
            get { return _Validator; }
        }

        public Int32 MasterId
        {
            get
            {
                return _VisioReader.GetMasterId(ShapeNamePrimaryKeyAttribute);
            }
        }

        public PrimaryKey Create(XElement element = null)
        {
            var primaryKey = new PrimaryKey
            {
                Name = "UniqueIdx"
            };

            return Validator.Validate(primaryKey) ? primaryKey : null;
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