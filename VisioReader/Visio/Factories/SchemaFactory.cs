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

        public SchemaFactory(IVisioReader visioReader)
        {
            if (visioReader == null) throw new ArgumentNullException("visioReader");

            _VisioReader = visioReader;

            _SchemaValidator = new SchemaValidator();

            _TableFactory = new TableFactory(visioReader, new TableValidator());
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
                Tables = ReadTables().ToList()
            };

            return Validator.Validate(schema) ? schema : null;
        }

        public IEnumerable<IFactory> ChildFactories
        {
            get
            {
                return new List<IFactory> {_TableFactory};
            }
        }

        public Boolean HasErrors()
        {
            var hasErrors = Validator.ValidationErrors.Any();

            ChildFactories.ToList().ForEach(f =>
                hasErrors = !hasErrors && f.HasErrors()
            );

            return hasErrors;
        }

        public IEnumerable<String> GetErrors()
        {
            var errors = new List<String>(Validator.ValidationErrors);

            ChildFactories.ToList().ForEach(f =>
                errors.AddRange(f.GetErrors())
            );

            return errors;
        }

        public IEnumerable<Table> ReadTables()
        {
            return _VisioReader
                .GetElements(_TableFactory.MasterId)
                .ToList()
                .Select(element => _TableFactory.Create(element));
        }
    }
}