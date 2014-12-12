using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio
{
    public class VisioDataFactory : IVisioDataFactory
    {
        private const string ShapeNameField = "Attribute";
        private const string ShapeNamePrimaryKeyAttribute = "Primary Key Attribute";
        private const string ShapeNameEntity = "Entity";
        private const string ShapeNameTable = "Entity";

        private readonly IVisioReader _VisioReader;

        public VisioDataFactory(IVisioReader visioReader)
        {
            _VisioReader = visioReader;
        }

        internal Field GetPrimaryKeyField(XElement element)
        {
            return new Field
            {
                Name = _VisioReader.GetName(element)
            };
        }

        internal Field GetField(XElement element)
        {
            return new Field
            {
                Name = _VisioReader.GetName(element),
                BaseType = "String"
            };
        }

        public Schema ReadSchema()
        {
            var schema = new Schema
            {
                Tables = ReadTables()
            };

            return schema;
        }

        public IEnumerable<Table> ReadTables()
        {
            var masterId = _VisioReader
                .GetMasterId(ShapeNameTable);

            return _VisioReader
                .GetElements(masterId)
                .Select(CreateTable);
        }

        internal Table CreateTable(XElement element)
        {
            var table = new Table
            {
                Name = _VisioReader.GetName(element)
            };

            table.Name = table.Name.Replace(' ', '_');

            var relatedIDs = _VisioReader.GetRelatedElementIDs(element);

            var primaryKeyFields = new List<Field>();
            var fields = new List<Field>();

            relatedIDs.ToList().ForEach(i =>
            {
                var pk = _VisioReader.GetElement(i);
                if (pk != null)
                {
                    if (table.PrimaryKey == null)
                    {
                        table.PrimaryKey = new PrimaryKey
                        {
                            Name = "UniqueIdx"
                        };
                    }

                    primaryKeyFields.Add(GetPrimaryKeyField(pk));
                }
                var field = _VisioReader.GetElement(i);
                if (field != null)
                {
                    fields.Add(GetField(field));
                }
            });

            table.PrimaryKey.Fields = primaryKeyFields;
            table.Fields = fields;
            
            return table;
        }
    }
}