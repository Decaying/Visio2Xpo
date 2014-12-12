using System;
using System.Collections.Generic;

namespace cvo.buyshans.Visio2Xpo.Data
{
    [Serializable]
    public class PrimaryKey
    {
        public String Name { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }
}