namespace Contracts.Model
{
    public class Banks
    {
        public int Id { get; set; }
        public string Bank { get; set; }
        public string CheckingAccount { get; set; }
        public string BIC { get; set; }
        public int FK_ClientId { get; set; }
    }
}
