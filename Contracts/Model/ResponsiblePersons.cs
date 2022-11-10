using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class ResponsiblePersons
    {
        public int id { get; set; }
        public string name { get; set; }
        public string jobTitle { get; set; }
        public int FK_ClientId { get; set; }
    }
}
