using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Model
{
    public class DictionaryData
    {
        public List<IndelResponsiblePersons> IndelResponsiblePersons { get; set; }
        public DictionaryData()
        {
            IndelResponsiblePersons = new List<IndelResponsiblePersons>();
        }
    }

    public class IndelResponsiblePersons
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string JobTitle { get; set; }
    }
}
