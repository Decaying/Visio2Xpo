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
        private const string XmlBaseUrl = "http://schemas.microsoft.com/visio/2010/relationships/";
        private const string XmlDocument = XmlBaseUrl + "document";
        private const string XmlPages = XmlBaseUrl + "pages";
        private const string XmlPage = XmlBaseUrl + "page";
        private readonly FileMode _FileMode;
        private readonly String _FileName;
        private IEnumerable<XElement> _Shapes;

        public IEnumerable<XElement> Shapes
        {
            get
            {
                if (_Shapes != null) return _Shapes;

                IEnumerable<XElement> elements = GetPageXml().Descendants().Where(e => e.Name.LocalName == "Shape");
                _Shapes = elements.DefaultIfEmpty(null);
                return _Shapes;
            }
        }


        private static PackagePart GetDocument(Package package)
        {
            PackageRelationship packageRel = package.GetRelationshipsByType(XmlDocument).FirstOrDefault();
            if (packageRel == null) return null;

            Uri docUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), packageRel.TargetUri);

            return package.GetPart(docUri);
        }

        private static PackagePart GetPages(PackagePart document)
        {
            return GetPackagePart(document, XmlPages);
        }

        private static PackagePart GetPage(PackagePart pages)
        {
            return GetPackagePart(pages, XmlPage);
        }

        private static PackagePart GetPackagePart(PackagePart packagePart, String relation)
        {
            PackageRelationship packageRel = packagePart.GetRelationshipsByType(relation).FirstOrDefault();
            if (packageRel == null) return null;

            Uri partUri = PackUriHelper.ResolvePartUri(
                packagePart.Uri, packageRel.TargetUri);
            return packagePart.Package.GetPart(partUri);
        }

        private static XDocument GetXmlFromPage(PackagePart page)
        {
            Stream partStream = page.GetStream();
            return XDocument.Load(partStream);
        }

        private XDocument GetPageXml()
        {
            using (Package package = Package.Open(_FileName, _FileMode))
            {
                PackagePart doc = GetDocument(package);
                PackagePart pages = GetPages(doc);
                PackagePart page = GetPage(pages);
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

        public IEnumerable<XElement> GetElementsByName(String shapeName)
        {
            return Shapes.Where(e => e.Attributes().ToList().Any(a => a.Name == "Name" && a.Value == shapeName));
        }

        #endregion IVisioReader

        #region IDisposable

        public void Dispose()
        {
        }

        #endregion IDisposable
    }
}