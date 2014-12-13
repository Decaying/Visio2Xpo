using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public class SchemaFactory : IFactory<Schema>
    {
        private readonly IValidator<Schema> _SchemaValidator;
        private readonly IFactory<Table> _TableFactory;
        private readonly IVisioReader _VisioReader;
        private readonly IValidator<Table> _TableValidator;

        public SchemaFactory(IVisioReader visioReader)
        {
            if (visioReader == null) throw new ArgumentNullException("visioReader");

            _VisioReader = visioReader;

            _TableValidator = new TableValidator();
            _SchemaValidator = new SchemaValidator();

            _TableFactory = new TableFactory(visioReader, _TableValidator);
        }

        public IValidator<Schema> Validator
        {
            get { return _SchemaValidator; }
        }

        public int MasterId
        {
            get { return 0; }
        }

        public Schema Create(XElement element = null)
        {
            var schema = new Schema
            {
                Tables = ReadTables()
            };

            return Validator.Validate(schema) ? schema : null;
        }

        public IEnumerable<Table> ReadTables()
        {
            return _VisioReader
                .GetElements(_TableFactory.MasterId)
                .Select(element => _TableFactory.Create(element));
        }
    }
}