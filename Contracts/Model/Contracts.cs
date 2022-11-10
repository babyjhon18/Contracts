using System;

namespace Contracts.Model
{
    public class Contracts
    {
        public int id { get; set; }
        public string Notes { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractDate { get; set; }
        public int ClientId { get; set; }
        public double Amount { get; set; }
        public int Percent { get; set; }
        public int TermsOfPaymentId { get; set; }
        public bool SignatureMark { get; set; }
        public bool ReadyMark { get; set; }
        public string DeadlineCondition { get; set; }
        public bool OurDelivery { get; set; }
        public string Comment { get; set; }
        public string HTMLSpecification { get; set; }
    }
}
