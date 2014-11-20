using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Xml.Linq;

namespace cvo.buyshans.Visio2Xpo.Communication
{
    public class VisioReader : IVisioReader
    {
        private readonly String _FileName;
        private readonly FileMode _FileMode;
        private IEnumerable<XElement> _Shapes;
        private const string ShapeNameField = "Attribute";
        private const string ShapeNamePrimaryKeyAttribute = "Primary Key Attribute";
        private const string ShapeNameEntity = "Entity";

        public IEnumerable<XElement> Shapes {
            get
            {
                if (_Shapes != null) return _Shapes;

                var elements = GetPageXml().Descendants().Where(e => e.Name.LocalName == "Shape");
                _Shapes = elements.DefaultIfEmpty(null);
                return _Shapes;
            }
        }


        private static PackagePart GetDocument(Package package)
        {
            var packageRel = package.GetRelationshipsByType("http://schemas.microsoft.com/visio/2010/relationships/document").FirstOrDefault();
            if (packageRel == null) return null;

            var docUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), packageRel.TargetUri);

            return package.GetPart(docUri);
        }

        private static PackagePart GetPages(PackagePart document)
        {
            return GetPackagePart(document, "http://schemas.microsoft.com/visio/2010/relationships/pages");
        }

        private static PackagePart GetPage(PackagePart pages)
        {
            return GetPackagePart(pages, "http://schemas.microsoft.com/visio/2010/relationships/page");
        }

        private static PackagePart GetPackagePart(PackagePart packagePart, String relation)
        {
            var packageRel = packagePart.GetRelationshipsByType(relation).FirstOrDefault();
            if (packageRel == null) return null;

            var partUri = PackUriHelper.ResolvePartUri(
                packagePart.Uri, packageRel.TargetUri);
            return packagePart.Package.GetPart(partUri);
        }

        private static XDocument GetXmlFromPage(PackagePart page)
        {
            var partStream = page.GetStream();
            return XDocument.Load(partStream);
        }
        private XDocument GetPageXml()
        {
            using (var package = Package.Open(_FileName, _FileMode))
            {
                var doc = GetDocument(package);
                var pages = GetPages(doc);
                var page = GetPage(pages);
                return GetXmlFromPage(page);
            }
        }

        #region Constructors
        public VisioReader(string fileName, FileMode fileMode = FileMode.Open)
        {
            _FileName = fileName;
            _FileMode = fileMode;
        }

        #endregion Constructors

        #region IVisioReader
        public IEnumerable<XElement> GetEntities()
        {
            return Shapes.Where(e => e.Attributes().ToList().Any(a => a.Name == "Name" && a.Value == ShapeNameEntity));
        }

        public IEnumerable<XElement> GetPrimaryKeys()
        {
            return Shapes.Where(e =>
                e.Attributes().ToList().Any(a => a.Name == "Name" && a.Value == ShapeNamePrimaryKeyAttribute)
                );
        }

        public IEnumerable<XElement> GetFields()
        {
            return Shapes.Where(e =>
                e.Attributes().ToList().Any(a => a.Name == "Name" && a.Value == ShapeNameField)
                );
        }

        #endregion IVisioReader

        public void Dispose()
        {
        }
    }
}
