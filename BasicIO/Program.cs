using System.IO;
using cvo.buyshans.Visio2Xpo.Communication.DynamicsAX;
using cvo.buyshans.Visio2Xpo.Communication.Visio;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Factories;
using cvo.buyshans.Visio2Xpo.Data;

namespace BasicIO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IVisioReader visioReader = new VisioReader("Drawing1.vsdx"))
            {
                IFactory<Schema> visioDataFactory = new SchemaFactory(visioReader);
                var schema = visioDataFactory.Create();

                using (var writer = new FileStream("output.xpo", FileMode.Create))
                {
                    var formatter = new XpoFormatter();
                    formatter.Serialize(writer, schema);
                }
            }
        }
    }
}