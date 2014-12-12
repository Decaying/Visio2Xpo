using System.IO;
using cvo.buyshans.Visio2Xpo.Communication.DynamicsAX;
using cvo.buyshans.Visio2Xpo.Communication.Visio;

namespace BasicIO
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var visioReader = new VisioReader("Drawing1.vsdx"))
            {
                var visioDataFactory = new VisioDataFactory(visioReader);
                var schema = visioDataFactory.ReadSchema();

                using (var writer = new FileStream("output.xpo", FileMode.Create))
                {
                    var formatter = new XpoFormatter();
                    formatter.Serialize(writer, schema);
                }
            }
        }
    }
}
