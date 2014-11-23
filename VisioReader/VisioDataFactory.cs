using System.Collections.Generic;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    internal class VisioDataFactory
    {
        private const string ShapeNameField = "Attribute";
        private const string ShapeNamePrimaryKeyAttribute = "Primary Key Attribute";
        private const string ShapeNameEntity = "Entity";

        private readonly IVisioReader _VisioReader;

        public VisioDataFactory(IVisioReader visioReader)
        {
            _VisioReader = visioReader;
        }

        public IEnumerable<PrimaryKey> GetPrimaryKeys()
        {
            return _VisioReader.GetElementsByName(ShapeNamePrimaryKeyAttribute).Select(element => new PrimaryKey());
        }

        public IEnumerable<Entity> GetEntities()
        {
            return _VisioReader.GetElementsByName(ShapeNameEntity).Select(element => new Entity());
        }

        public IEnumerable<Field> GetFields()
        {
            return _VisioReader.GetElementsByName(ShapeNameField).Select(element => new Field());
        }
    }
}