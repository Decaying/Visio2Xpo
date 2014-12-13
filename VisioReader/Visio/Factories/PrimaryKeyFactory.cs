using System;
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

        public PrimaryKey Create(XElement element)
        {
            var primaryKey = new PrimaryKey
            {
                Name = "UniqueIdx"
            };

            return Validator.Validate(primaryKey) ? primaryKey : null;
        }
    }
}