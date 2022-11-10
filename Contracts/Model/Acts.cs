using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class Acts
    {
        public int id { get; set; }
        public int ActNumber { get; set; }
        public DateTime ActDate { get; set; }
        public int FK_ContractId { get; set; }
    }
}
