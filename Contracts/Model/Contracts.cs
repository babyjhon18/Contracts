using System;

namespace Contracts.Model
{
    public class Contracts
    {
        public int id { get; set; }
        public string ReadyComment { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractDate { get; set; }
        public int ClientId { get; set; }
        public double Amount { get; set; }
        public int Percent { get; set; }
        public int TermsOfPaymentId { get; set; }
        public bool SignatureMark { get; set; }
        public bool ReadyMark { get; set; }
        public int DeadLine { get; set; }
        public string DeadLineDayType { get; set; }
        public DateTime DeadLineDaySetted { get; set; }
        public bool OurDelivery { get; set; }
        public string Comment { get; set; }
        public string HTMLSpecification { get; set; }
        public DateTime SignDate { get; set; }
        public string ManufacturingLeadNotes { get; set; }
        public string ManufacturingLeadNoteColor { get; set; }
    }
}
