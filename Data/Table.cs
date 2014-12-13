using System;
using System.Collections.Generic;

namespace cvo.buyshans.Visio2Xpo.Data
{
    [Serializable]
    public class Table
    {
        public String Name { get; set; }
        public IEnumerable<Field> Fields { get; set; }
        public PrimaryKey PrimaryKey { get; set; }
    }
}