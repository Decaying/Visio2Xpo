using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvo.buyshans.Visio2Xpo.Data
{
    [Serializable]
    public class Schema
    {
        public String Name { get; set; }

        public IEnumerable<Table> Tables { get; set; } 
    }
}
