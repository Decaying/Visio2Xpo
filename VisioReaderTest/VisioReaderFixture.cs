using System;
using System.Linq;
using cvo.buyshans.Visio2Xpo.Communication;
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

        [TestMethod]
        public void Drawing1HasPrimaryKey()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                Assert.AreEqual(1, reader.GetElementsByName(PrimaryKeyAttribute).Count());
            }
        }

        [TestMethod]
        public void Drawing1HasNoAttributes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                Assert.AreEqual(0, reader.GetElementsByName(Attribute).Count());
            }
        }

        [TestMethod]
        public void Drawing1HasNoEntities()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing1))
            {
                Assert.AreEqual(0, reader.GetElementsByName(Entity).Count());
            }
        }

        [TestMethod]
        public void Drawing2HasField()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                Assert.AreEqual(1, reader.GetElementsByName(Attribute).Count());
            }
        }

        [TestMethod]
        public void Drawing2HasNoPrimaryKeys()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                Assert.AreEqual(0, reader.GetElementsByName(PrimaryKeyAttribute).Count());
            }
        }

        [TestMethod]
        public void Drawing2HasNoEntities()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing2))
            {
                Assert.AreEqual(0, reader.GetElementsByName(Entity).Count());
            }
        }

        [TestMethod]
        public void Drawing3HasEntity()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                Assert.AreEqual(1, reader.GetElementsByName(Entity).Count());
            }
        }

        [TestMethod]
        public void Drawing3HasNoPrimaryKeys()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                Assert.AreEqual(0, reader.GetElementsByName(PrimaryKeyAttribute).Count());
            }
        }

        [TestMethod]
        public void Drawing3HasNoAttributes()
        {
            using (IVisioReader reader = new VisioReader(FileDrawing3))
            {
                Assert.AreEqual(0, reader.GetElementsByName(Attribute).Count());
            }
        }
    }
}