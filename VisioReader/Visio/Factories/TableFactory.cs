using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Factories
{
    public class TableFactory : IFactory<Table>
    {
        private const string ShapeNameTable = "Entity";

        private readonly IVisioReader _VisioReader;
        private readonly IValidator<Table> _Validator;

        private readonly IFactory<PrimaryKey> _PrimaryKeyFactory;
        private readonly IFactory<Field> _FieldFactory; 

        public TableFactory(IVisioReader visioReader, IValidator<Table> validator)
        {
            if (visioReader == null) throw new ArgumentNullException("visioReader");
            if (validator == null) throw new ArgumentNullException("validator");

            _VisioReader = visioReader;
            _Validator = validator;

            _PrimaryKeyFactory = new PrimaryKeyFactory(visioReader, new PrimaryKeyValidator());
            _FieldFactory = new FieldFactory(visioReader, new FieldValidator());
        }

        public IValidator<Table> Validator
        {
            get { return _Validator; }
        }

        public Int32 MasterId
        {
            get
            {
                return _VisioReader.GetMasterId(ShapeNameTable);
            }
        }

        public Table Create(XElement element = null)
        {
            if (element == null) throw new ArgumentNullException("element");

            var tableName = _VisioReader.GetName(element);
            
            var table = new Table
            {
                Name = tableName
            };
            
            var relatedIDs = _VisioReader.GetRelatedElementIDs(element);

            PrimaryKey primaryKey = null;
            var primaryKeyFields = new List<Field>();
            var fields = new List<Field>();

            relatedIDs.ToList().ForEach(i =>
            {
                var child = _VisioReader.GetElement(i);
                if (child == null) return;

                var masterId = _VisioReader.GetMasterId(child);
                if (masterId == 0) return;

                if (masterId == _PrimaryKeyFactory.MasterId)
                {
                    if (primaryKey == null)
                    {
                        primaryKey = _PrimaryKeyFactory.Create();
                    }

                    primaryKeyFields.Add(_FieldFactory.Create(child));
                }
                else if (masterId == _FieldFactory.MasterId)
                {
                    fields.Add(_FieldFactory.Create(child));
                }
            });
            
            if (primaryKey != null)
            {
                primaryKey.Fields = primaryKeyFields;
                table.PrimaryKey = primaryKey;
            }

            table.Fields = fields;

            return _Validator.Validate(table) ? table : null;
        }

        public IEnumerable<IFactory> ChildFactories
        {
            get
            {
                return new List<IFactory> {_PrimaryKeyFactory, _FieldFactory};
            }
        }

        public Boolean HasErrors()
        {
            var hasErrors = Validator.ValidationErrors.Any();

            ChildFactories.ToList().ForEach(f =>
            {
                hasErrors = !hasErrors && f.HasErrors();
            });

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
    }
}