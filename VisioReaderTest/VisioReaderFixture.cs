using System;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Communication.Visio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cvo.buyshans.Visio2Xpo.VisioReaderTest
{
    [TestClass]
    public class VisioReaderFixture
    {
        private const string PrimaryKeyAttribute = "Primary Key Attribute";
        private const string Attribute = "Attribute";
        private const string Entity = "Entity";

        private const String FileDrawing1 = "Drawing1.vsdx";
        private const String FileDrawing2 = "Drawing2.vsdx";
        private const String FileDrawing3 = "Drawing3.vsdx";
        private const String FileDrawing4 = "Drawing4.vsdx";

        #region "MasterId"
        [TestMethod]
        public void Drawing1PrimaryKeyIsMasterId31()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                const Int32 expected = 31;
                var actual = reader.GetMasterId(PrimaryKeyAttribute);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing2AttributeIsMasterId33()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                const Int32 expected = 33;
                var actual = reader.GetMasterId(Attribute);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing3EntityIsMasterId30()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                const Int32 expected = 30;
                var actual = reader.GetMasterId(Entity);

                Assert.AreEqual(expected, actual);
            }
        }
        #endregion "MasterId"

        #region "HasElements"
        [TestMethod]
        public void Drawing1HasPrimaryKey()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                var masterId = reader.GetMasterId(PrimaryKeyAttribute);

                var actual = reader.GetElements(masterId).Any();

                Assert.IsTrue(actual);
            }
        }
        
        [TestMethod]
        public void Drawing2HasAttribute()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                var masterId = reader.GetMasterId(Attribute);

                var actual = reader.GetElements(masterId).Any();

                Assert.IsTrue(actual);
            }
        }

        [TestMethod]
        public void Drawing3HasEntity()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                var masterId = reader.GetMasterId(Entity);

                var actual = reader.GetElements(masterId).Any();

                Assert.IsTrue(actual);
            }
        }
        #endregion "HasElements"

        #region "HasNoElements"

        [TestMethod]
        public void Drawing1HasNoAttributes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                var masterId = reader.GetMasterId(Attribute);

                var actual = reader.GetElements(masterId).Any();

                Assert.IsFalse(actual);
            }
        }

        [TestMethod]
        public void Drawing1HasNoEntities()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                var masterId = reader.GetMasterId(Entity);

                var actual = reader.GetElements(masterId).Any();

                Assert.IsFalse(actual);
            }
        }
        #endregion "HasNoElements"

        #region "Element Name"

        [TestMethod]
        public void Drawing1HasPrimaryKeyName()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                var masterId = reader.GetMasterId(PrimaryKeyAttribute);
                var element = reader.GetElements(masterId).First();

                const String expected = "Primary Key 1";
                var actual = reader.GetName(element);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing2HasAttributeName()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                var masterId = reader.GetMasterId(Attribute);
                var element = reader.GetElements(masterId).First();

                const String expected = "field";
                var actual = reader.GetName(element);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing3HasEntityName()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                var masterId = reader.GetMasterId(Entity);
                var element = reader.GetElements(masterId).First();

                const String expected = "Entity";
                var actual = reader.GetName(element);

                Assert.AreEqual(expected, actual);
            }
        }

        #endregion "Element Name"

        #region "Child Elements"

        [TestMethod]
        public void Drawing4Has2Entities()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entities = reader.GetElements(masterId);

                const Int32 expected = 2;
                var actual = entities.Count();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing4Entity1Has4ChildShapes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entity = reader.GetElements(masterId).First();

                const Int32 expected = 4;
                var actual = reader.GetRelatedElementIDs(entity).Count();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing4Entity2Has5ChildShapes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entity = reader.GetElements(masterId).Skip(1).First();

                const Int32 expected = 5;
                var actual = reader.GetRelatedElementIDs(entity).Count();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Drawing4Entity1Has3NamedChildShapes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entity = reader.GetElements(masterId).First();

                const String expected1 = "PrimaryKey1";
                const String expected2 = "Field1";
                const String expected3 = "Field2";
                
                var childEntities = reader.GetRelatedElementIDs(entity).ToList();

                var entity1 = reader.GetElement(childEntities.First());
                var entity2 = reader.GetElement(childEntities.Skip(2).First());
                var entity3 = reader.GetElement(childEntities.Skip(3).First());

                var actual1 = reader.GetName(entity1);
                var actual2 = reader.GetName(entity2);
                var actual3 = reader.GetName(entity3);

                Assert.AreEqual(expected1, actual1);
                Assert.AreEqual(expected2, actual2);
                Assert.AreEqual(expected3, actual3);
            }
        }
        [TestMethod]
        public void Drawing4Entity2Has4NamedChildShapes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entity = reader.GetElements(masterId).Skip(1).First();

                const String expected1 = "PK2";
                const String expected2 = "Field2";
                const String expected3 = "Field3";
                const String expected4 = "Field4";

                var childEntities = reader.GetRelatedElementIDs(entity).ToList();

                var entity1 = reader.GetElement(childEntities.First());
                var entity2 = reader.GetElement(childEntities.Skip(2).First());
                var entity3 = reader.GetElement(childEntities.Skip(3).First());
                var entity4 = reader.GetElement(childEntities.Skip(4).First());

                var actual1 = reader.GetName(entity1);
                var actual2 = reader.GetName(entity2);
                var actual3 = reader.GetName(entity3);
                var actual4 = reader.GetName(entity4);

                Assert.AreEqual(expected1, actual1);
                Assert.AreEqual(expected2, actual2);
                Assert.AreEqual(expected3, actual3);
                Assert.AreEqual(expected4, actual4);
            }
        }

        [TestMethod]
        public void Drawing4Entity1Has1PrimaryKeyField()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing4))
            {
                var masterId = reader.GetMasterId(Entity);
                var entity = reader.GetElements(masterId).First();
                var childEntities = reader.GetRelatedElementIDs(entity).ToList();

                var childEntity = reader.GetElement(childEntities.First());

                const Int32 expected = 31;
                var actual = reader.GetMasterId(childEntity);

                Assert.AreEqual(expected, actual);
            }
        }
        #endregion "Child Elements"
    }
}