using System;

namespace Contracts.JSONViewModels
{
    public class JSONNotificationModel
    {
        public int ClientId { get; set; }
        public int ContractId { get; set; }
        public DateTime ReadyDate { get; set; }
    }
}
