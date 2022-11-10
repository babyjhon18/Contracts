using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class Clients
    {
        public int id { get; set; }
        public string name { get; set; }
        public string FullName { get; set; }
        public string LegalAddress { get; set; }
        public string PostAddress { get; set; }
        //Банковские реквизиты
        public string CheckingAccount { get; set; }
        public string Bank { get; set; }
        public string BIC { get; set; }
        //-----------------------------
        public string UNP { get; set; }
        public string OKPO { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string StorageAddress { get; set; }
    }
}
