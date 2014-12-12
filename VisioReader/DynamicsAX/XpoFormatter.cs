using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.DynamicsAX
{
    public class XpoFormatter : IFormatter
    {
        public object Deserialize(Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            var members = FormatterServices.GetSerializableMembers(graph.GetType(), Context);

            // Get fields data.
            var objs = FormatterServices.GetObjectData(graph, members);

            // Write class name and all fields & values to file
            using (var sw = new StreamWriter(serializationStream))
            {
                sw.WriteLine("Exportfile for AOT version 1.0 or later");
                sw.WriteLine("Formatversion: 1");

                objs.OfType<IEnumerable<Table>>().ToList().ForEach(o => SerializeTables(sw, o));

                sw.WriteLine("***Element: END");
            }
        }

        private void SerializeTables(StreamWriter sw, IEnumerable<Table> tables)
        {
            tables.ToList().ForEach(t =>
            {
                sw.WriteLine("");
                sw.WriteLine("*** ELEMENT: DBT");
                sw.WriteLine("");
                sw.WriteLine("; Microsoft Dynamics AX Table : {0} unloaded", t.Name);
                sw.WriteLine("; --------------------------------------------------------------------------------");
                sw.WriteLine("  TABLEVERSION 1");
                sw.WriteLine("  TABLE #{0}", t.Name);
                sw.WriteLine("    EnforceFKRelation 1");
                sw.WriteLine();
                sw.WriteLine("    PROPERTIES");
                sw.WriteLine("      Name                #{0}", t.Name);
                sw.WriteLine("      CreateRecIdIndex    #Yes");
                sw.WriteLine("      PrimaryIndex        #SurrogateKey");
                sw.WriteLine("      ClusterIndex        #SurrogateKey");
                sw.WriteLine("      Origin              #{0}", Guid.NewGuid().ToString("B"));
                sw.WriteLine("    ENDPROPERTIES");
                sw.WriteLine();
                sw.WriteLine("    FIELDS");

                SerializeFields(sw, t.PrimaryKey.Fields);
                SerializeFields(sw, t.Fields);

                sw.WriteLine("    ENDFIELDS");
                sw.WriteLine("    GROUPS");

                sw.WriteLine("    ENDGROUPS");
                sw.WriteLine("    INDICES");

                SerializePrimaryKeyIndex(sw, t.PrimaryKey);

                sw.WriteLine("    ENDINDICES");
                sw.WriteLine("    FULLTEXTINDICES");
                sw.WriteLine("    ENDFULLTEXTINDICES");
                sw.WriteLine("    REFERENCES");
                sw.WriteLine("    ENDREFERENCES");
                sw.WriteLine();
                sw.WriteLine("    DELETEACTIONS");
                sw.WriteLine("    ENDDELETEACTIONS");
                sw.WriteLine();
                sw.WriteLine("    METHODS");
                sw.WriteLine("    ENDMETHODS");
                sw.WriteLine("  ENDTABLE");
                sw.WriteLine();
            });
        }

        private void SerializeFields(StreamWriter sw, IEnumerable<Field> fields)
        {
            if (fields == null) return;
            
            fields.ToList().ForEach(f =>
            {
                sw.WriteLine("      FIELD #{0}", f.Name);
                sw.WriteLine("        {0}", f.BaseType.ToUpper());
                sw.WriteLine("        PROPERTIES");
                sw.WriteLine("          Name                #{0}", f.Name);
                sw.WriteLine("          Table               #DummyTable");
                sw.WriteLine("          Origin              #{0}", Guid.NewGuid().ToString("B"));
                sw.WriteLine("        ENDPROPERTIES");
                sw.WriteLine();
            });
        }

        private void SerializePrimaryKeyIndex(StreamWriter sw, PrimaryKey primaryKey)
        {
            sw.WriteLine("      #{0}", primaryKey.Name);
            sw.WriteLine("      PROPERTIES");
            sw.WriteLine("        Name                #{0}", primaryKey.Name);
            sw.WriteLine("        Origin              #{0}", Guid.NewGuid().ToString("B"));
            sw.WriteLine("        PROPERTIES");
            sw.WriteLine("      ENDPROPERTIES");
            sw.WriteLine();
            sw.WriteLine("      INDEXFIELDS");
            primaryKey.Fields.ToList().ForEach(f =>
                sw.WriteLine("        #{0}", f.Name)
            );
            sw.WriteLine("      ENDINDEXFIELDS");
        }

        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
    }
}
