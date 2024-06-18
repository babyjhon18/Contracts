using Contracts.Model;
using System;

namespace Contracts.JSONViewModels
{
    public class JSONContractModel
    {
        public int id { get; set; }
        public string ReadyComment { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string ClientName { get; set; }
        public object Amount { get; set; }
        public int PaymentPercent { get; set; }
        public string TermsOfPaymentName { get; set; }
        public bool SignatureMark { get; set; }
        public bool ReadyMark { get; set; }
        public int DeadLine { get; set; }
        public string DeadLineDayType { get; set; }
        public DateTime DeadLineDaySetted { get; set; }
        public bool OurDelivery { get; set; }
        public string Comment { get; set; }
        public string HTMLSpecification { get; set; }
        public string SignDate { get; set; }
        public string ManufacturingLeadNotes { get; set; }
        public string ManufacturingLeadNoteColor { get; set; }
        public bool NotForWorkPlan { get; set; }
        public int PaymentDelay { get; set; }
        public string PaymentDelayDayType { get; set; }
        public DateTime LastActDate { get; set; }
        public string ContractAcceptedBy { get; set; }
        public bool SawContract { get; set; }
        public DateTime DayToPlan { get; set; }
    }
}
