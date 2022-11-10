using Contracts.Model;
using Newtonsoft.Json;
using System;

namespace Contracts.ViewModels
{
    public class DictionaryViewModel
    {
        private readonly string FileName = "WordInfo.json";
        public DictionaryData DictionaryData { get; set; }
        public DictionaryViewModel()
        {
            DictionaryData = new DictionaryData();
            GetDictionaryData();
        }

        private void GetDictionaryData() 
        {
            var json = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + FileName);
            DictionaryData = JsonConvert.DeserializeObject<DictionaryData>(json);
        }
    }
}
