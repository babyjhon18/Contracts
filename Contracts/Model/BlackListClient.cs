namespace Contracts.Model
{
    public class BlackListClient
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int FK_CompanyID { get; set; }
    }
}
