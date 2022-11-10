using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class Payments
    {
        public int Id { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public double PaymentSum { get; set; }
        public int FK_ContractId { get; set; }
    }
}
