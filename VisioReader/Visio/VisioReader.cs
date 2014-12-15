using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio
{
    [Export(typeof(IVisioReader))]
    public class VisioReader : IVisioReader
    {
        private const string XmlBaseUrl = "http://schemas.microsoft.com/visio/2010/relationships/";
        private const string XmlDocument = XmlBaseUrl + "document";
        private const string XmlPages = XmlBaseUrl + "pages";
        private const string XmlPage = XmlBaseUrl + "page";
        private const string XmlMasters = XmlBaseUrl + "masters";
        private const string XmlVisioCore = "http://schemas.microsoft.com/office/visio/2012/main";

        private FileMode _FileMode;
        private String _FileName;

        private IEnumerable<XElement> _Shapes;
        private IEnumerable<XElement> _Masters;

        private IEnumerable<XElement> Shapes
        {
            get
            {
                if (_Shapes == null)
                {
                    var xShapeName = XName.Get("Shape", XmlVisioCore);
                    _Shapes = GetPageXml().Descendants(xShapeName);
                }

                return _Shapes;
            }
        }

        private IEnumerable<XElement> Masters
        {
            get
            {
                if (_Masters == null)
                {
                    var xMasterName = XName.Get("Master", XmlVisioCore);
                    _Masters = GetMastersXml().Descendants(xMasterName);
                }

                return _Masters;
            }
        }

        private XDocument GetMastersXml()
        {
            using (var package = Package.Open(_FileName, _FileMode))
            {
                var doc = GetDocument(package);
                var masters = GetMasters(doc);
                return GetXmlFromPackagePart(masters);
            }
        }

        private PackagePart GetMasters(PackagePart doc)
        {
            return GetPackagePart(doc, XmlMasters);
        }


        private static PackagePart GetDocument(Package package)
        {
            if (package == null) throw new ArgumentNullException("package");

            var packageRel = package.GetRelationshipsByType(XmlDocument).FirstOrDefault();
            if (packageRel == null) return null;

            var docUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), packageRel.TargetUri);

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
            if (packagePart == null) throw new ArgumentNullException("packagePart");

            var packageRel = packagePart.GetRelationshipsByType(relation).FirstOrDefault();
            if (packageRel == null) return null;

            var partUri = PackUriHelper.ResolvePartUri(
                packagePart.Uri, packageRel.TargetUri);
            return packagePart.Package.GetPart(partUri);
        }

        private static XDocument GetXmlFromPackagePart(PackagePart page)
        {
            if (page == null) throw new ArgumentNullException("page");

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
                return GetXmlFromPackagePart(page);
            }
        }

        private IEnumerable<Int32> GetIDsFromRelationshipsFormula(String formula)
        {
            const String pattern = "Sheet.(\\d{1,})";
            var results = Regex.Matches(formula, pattern);

            return results
                .OfType<Match>()
                .ToList()
                .Select(m => 
                    Convert.ToInt32(m.Groups[1].Value)
                    );
        }
        #region IVisioReader
        public IVisioReader Initialize(String fileName, FileMode fileMode = FileMode.Open)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException(String.Format("File {0} does not exist", fileName), fileName);

            _FileName = fileName;
            _FileMode = fileMode;

            return this;
        }

        public IEnumerable<XElement> GetElements(Int32 masterId)
        {
            return Shapes.Where(e => 
                e.Attribute("Master") != null &&
                e.Attribute("Master").Value == masterId.ToString(CultureInfo.InvariantCulture)
            );
        }

        public IEnumerable<Int32> GetRelatedElementIDs(XElement element)
        {
            if (element == null) throw new ArgumentNullException("element");

            var textXCell = XName.Get("Cell", XmlVisioCore);

            var relatedElementIDs = new List<Int32>();

            element.Descendants(textXCell)
                .Where(xe => xe.Attribute("N") != null && xe.Attribute("N").Value == "Relationships")
                .ToList()
                .ForEach(
                    xe => relatedElementIDs.AddRange(GetIDsFromRelationshipsFormula(xe.Attribute("F").Value))
                );

            return relatedElementIDs;
        }

        public String GetName(XElement element)
        {
            if (element == null) throw new ArgumentNullException("element");

            var textXName = XName.Get("Text", XmlVisioCore);
            var elementName = element.Elements(textXName).SingleOrDefault();

            return elementName != null ? elementName.Value.Trim().Trim('\n') : "";
        }

        public XElement GetElement(Int32 id)
        {
            return Shapes.SingleOrDefault(e => 
                e.Attributes("ID").Any(a => 
                    a.Value == id.ToString(CultureInfo.InvariantCulture))
                );
        }

        public Int32 GetMasterId(String masterName)
        {
            var master = Masters.SingleOrDefault(e => e.Attributes("Name").Any(a => a.Value == masterName));
            return master != null ? Convert.ToInt32(master.Attribute("ID").Value) : 0;
        }

        public int GetMasterId(XElement element)
        {
            if (element == null) throw new ArgumentNullException("element");

            return Convert.ToInt32(element.Attribute("Master").Value);
        }

        #endregion IVisioReader

        #region IDisposable
        public void Dispose()
        {
        }
        #endregion IDisposable
    }
}