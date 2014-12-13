using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio
{
    public class VisioDataFactory : IVisioDataFactory
    {
        private const string ShapeNameAttribute = "Attribute";
        private const string ShapeNamePrimaryKeyAttribute = "Primary Key Attribute";
        private const string ShapeNameTable = "Entity";

        private Int32 _MasterIdAttribute;
        private Int32 _MasterIdTable;
        private Int32 _MasterIdPrimaryKeyAttribute;

        private readonly IVisioReader _VisioReader;

        public VisioDataFactory(IVisioReader visioReader)
        {
            _VisioReader = visioReader;

            _MasterIdAttribute = _VisioReader.GetMasterId(ShapeNameAttribute);
            _MasterIdPrimaryKeyAttribute = _VisioReader.GetMasterId(ShapeNamePrimaryKeyAttribute);
            _MasterIdTable = _VisioReader.GetMasterId(ShapeNameTable);
        }

        internal Field GetPrimaryKeyField(XElement element)
        {
            return new Field
            {
                Name = _VisioReader.GetName(element),
                BaseType = "String" //todo: base type in Visio
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
            return _VisioReader
                .GetElements(_MasterIdTable)
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
                var child = _VisioReader.GetElement(i);
                if (child == null) return;

                var masterId = _VisioReader.GetMasterId(child);
                if (masterId == 0) return;

                if (masterId == _MasterIdPrimaryKeyAttribute)
                {
                    primaryKeyFields.Add(GetPrimaryKeyField(child));
                } 
                else if (masterId == _MasterIdAttribute)
                {
                    fields.Add(GetField(child));
                }
            });

            if (primaryKeyFields.Count > 0)
            {
                table.PrimaryKey = new PrimaryKey
                {
                    Name = "UniqueIdx",
                    Fields = primaryKeyFields
                };
            }
            table.Fields = fields;
            
            return table;
        }
    }
}