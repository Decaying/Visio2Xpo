using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.Communication.DynamicsAX
{
    [Export(typeof(IFormatter))]
    public class XpoFormatter : IFormatter
    {
        public object Deserialize(Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            if (serializationStream == null) throw new ArgumentNullException("serializationStream");
            if (graph == null) throw new ArgumentNullException("graph");
            if (!serializationStream.CanWrite) throw new ArgumentException("Stream is not writable");

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

                sw.Flush();
                sw.Close();
            }
        }

        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        private static void SerializeTables(TextWriter sw, IEnumerable<Table> tables)
        {
            tables.ToList().ForEach(t =>
            {
                sw.WriteLine("");
                sw.WriteLine("***Element: DBT");
                sw.WriteLine("");
                sw.WriteLine("; Microsoft Dynamics AX Table : {0} unloaded", t.Name);
                sw.WriteLine("; --------------------------------------------------------------------------------");
                sw.WriteLine("  TABLEVERSION 1");
                sw.WriteLine("");
                sw.WriteLine("  TABLE #{0}", t.Name);
                sw.WriteLine("    EnforceFKRelation 1");
                sw.WriteLine("    PROPERTIES");
                sw.WriteLine("      Name                #{0}", t.Name);
                sw.WriteLine("      CreateRecIdIndex    #Yes");

                if (t.PrimaryKey != null)
                {
                    sw.WriteLine("      PrimaryIndex        #{0}", t.PrimaryKey.Name);
                    sw.WriteLine("      ClusterIndex        #{0}", t.PrimaryKey.Name);
                }
                else
                {
                    sw.WriteLine("      PrimaryIndex        #SurrogateKey");
                    sw.WriteLine("      ClusterIndex        #SurrogateKey");
                }

                sw.WriteLine("      Origin              #{0}", Guid.NewGuid().ToString("B"));
                sw.WriteLine("    ENDPROPERTIES");
                sw.WriteLine();
                sw.WriteLine("    FIELDS");

                if (t.PrimaryKey != null)
                {
                    SerializeFields(sw, t, t.PrimaryKey.Fields);
                }
                if (t.Fields != null)
                {
                    SerializeFields(sw, t, t.Fields);
                }

                sw.WriteLine("    ENDFIELDS");
                sw.WriteLine("    GROUPS");

                sw.WriteLine("    ENDGROUPS");
                sw.WriteLine("    INDICES");

                if (t.PrimaryKey != null)
                {
                    SerializePrimaryKeyIndex(sw, t.PrimaryKey);
                }

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

        private static void SerializeFields(TextWriter sw, Table table, IEnumerable<Field> fields)
        {
            if (fields == null) return;

            fields.ToList().ForEach(f =>
            {
                sw.WriteLine("      FIELD #{0}", f.Name);
                sw.WriteLine("        {0}", f.BaseType.ToUpper());
                sw.WriteLine("        PROPERTIES");
                sw.WriteLine("          Name                #{0}", f.Name);
                sw.WriteLine("          Table               #{0}", table.Name);
                sw.WriteLine("          Origin              #{0}", Guid.NewGuid().ToString("B"));
                sw.WriteLine("        ENDPROPERTIES");
                sw.WriteLine();
            });
        }

        private static void SerializePrimaryKeyIndex(TextWriter sw, PrimaryKey primaryKey)
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
    }
}