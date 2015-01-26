using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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

        public async void Serialize(Stream serializationStream, object graph)
        {
            if (serializationStream == null) throw new ArgumentNullException("serializationStream");
            if (graph == null) throw new ArgumentNullException("graph");
            if (!serializationStream.CanWrite) throw new ArgumentException("Stream is not writable");

            var members = FormatterServices.GetSerializableMembers(graph.GetType(), Context);

            // Get fields data.
            var objs = FormatterServices.GetObjectData(graph, members);

            var sb = new StringBuilder();

            sb.AppendLine("Exportfile for AOT version 1.0 or later");
            sb.AppendLine("Formatversion: 1");

            objs.OfType<IEnumerable<Table>>().ToList().ForEach(o => SerializeTables(sb, o));

            sb.AppendLine("***Element: END");

            // Write class name and all fields & values to file
            using (var sw = new StreamWriter(serializationStream))
            {
                await sw.WriteAsync(sb.ToString());
            }
        }

        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        private static void SerializeTables(StringBuilder sb, IEnumerable<Table> tables)
        {
            tables.ToList().ForEach(t =>
            {
                sb.AppendLine("");
                sb.AppendLine("***Element: DBT");
                sb.AppendLine("");
                sb.AppendLine(String.Format("; Microsoft Dynamics AX Table : {0} unloaded", t.Name));
                sb.AppendLine("; --------------------------------------------------------------------------------");
                sb.AppendLine("  TABLEVERSION 1");
                sb.AppendLine("");
                sb.AppendLine(String.Format("  TABLE #{0}", t.Name));
                sb.AppendLine("    EnforceFKRelation 1");
                sb.AppendLine("    PROPERTIES");
                sb.AppendLine(String.Format("      Name                #{0}", t.Name));
                sb.AppendLine("      CreateRecIdIndex    #Yes");

                if (t.PrimaryKey != null)
                {
                    sb.AppendLine(String.Format("      PrimaryIndex        #{0}", t.PrimaryKey.Name));
                    sb.AppendLine(String.Format("      ClusterIndex        #{0}", t.PrimaryKey.Name));
                }
                else
                {
                    sb.AppendLine("      PrimaryIndex        #SurrogateKey");
                    sb.AppendLine("      ClusterIndex        #SurrogateKey");
                }

                sb.AppendLine(String.Format("      Origin              #{0}", Guid.NewGuid().ToString("B")));
                sb.AppendLine("    ENDPROPERTIES");
                sb.AppendLine();
                sb.AppendLine("    FIELDS");

                if (t.PrimaryKey != null)
                {
                    SerializeFields(sb, t, t.PrimaryKey.Fields);
                }
                if (t.Fields != null)
                {
                    SerializeFields(sb, t, t.Fields);
                }

                sb.AppendLine("    ENDFIELDS");
                sb.AppendLine("    GROUPS");

                sb.AppendLine("    ENDGROUPS");
                sb.AppendLine("    INDICES");

                if (t.PrimaryKey != null)
                {
                    SerializePrimaryKeyIndex(sb, t.PrimaryKey);
                }

                sb.AppendLine("    ENDINDICES");
                sb.AppendLine("    FULLTEXTINDICES");
                sb.AppendLine("    ENDFULLTEXTINDICES");
                sb.AppendLine("    REFERENCES");
                sb.AppendLine("    ENDREFERENCES");
                sb.AppendLine();
                sb.AppendLine("    DELETEACTIONS");
                sb.AppendLine("    ENDDELETEACTIONS");
                sb.AppendLine();
                sb.AppendLine("    METHODS");
                sb.AppendLine("    ENDMETHODS");
                sb.AppendLine("  ENDTABLE");
                sb.AppendLine();
            });
        }

        private static void SerializeFields(StringBuilder sb, Table table, IEnumerable<Field> fields)
        {
            if (fields == null) return;

            fields.ToList().ForEach(f =>
            {
                sb.AppendLine(String.Format("      FIELD #{0}", f.Name));
                sb.AppendLine(String.Format("        {0}", f.BaseType.ToUpper()));
                sb.AppendLine("        PROPERTIES");
                sb.AppendLine(String.Format("          Name                #{0}", f.Name));
                sb.AppendLine(String.Format("          Table               #{0}", table.Name));
                sb.AppendLine(String.Format("          Origin              #{0}", Guid.NewGuid().ToString("B")));
                sb.AppendLine("        ENDPROPERTIES");
                sb.AppendLine();
            });
        }

        private static void SerializePrimaryKeyIndex(StringBuilder sb, PrimaryKey primaryKey)
        {
            sb.AppendLine(String.Format("      #{0}", primaryKey.Name));
            sb.AppendLine("      PROPERTIES");
            sb.AppendLine(String.Format("        Name                #{0}", primaryKey.Name));
            sb.AppendLine("        AllowDuplicates     #No");
            sb.AppendLine("        AlternateKey        #Yes");
            sb.AppendLine(String.Format("        Origin              #{0}", Guid.NewGuid().ToString("B")));
            sb.AppendLine("      ENDPROPERTIES");
            sb.AppendLine();
            sb.AppendLine("      INDEXFIELDS");
            primaryKey.Fields.ToList().ForEach(f =>
                sb.AppendLine(String.Format("        #{0}", f.Name))
                );
            sb.AppendLine("      ENDINDEXFIELDS");
        }
    }
}