namespace Contracts.JSONViewModels
{
    public class JSONContractModel
    {
        public int id { get; set; }
        public string Notes { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string ClientName { get; set; }
        public double Amount { get; set; }
        public int PaymentPercent { get; set; }
        public string TermsOfPaymentName { get; set; }
        public bool SignatureMark { get; set; }
        public bool ReadyMark { get; set; }
        public string DeadlineCondition { get; set; }
        public bool OurDelivery { get; set; }
        public string Comment { get; set; }
        public string HTMLSpecification { get; set; }
    }
}
