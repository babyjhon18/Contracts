using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.ViewModels
{
    public class ContractsViewModel 
    {
        DataBaseContext context;
        public ContractsViewModel(DataBaseContext db)
        {
            context = db;
        }

        public Object GetContracts(bool isReadyForAssemble)
        {
            List<Object> ContractsList = new List<Object>();
            Object ContractsData = new Object();
            if (isReadyForAssemble)
            {
                foreach (var contract in context.Contracts.Where(c => c.SignatureMark == isReadyForAssemble))
                {
                    var Contract = new
                    {
                        Contract = contract,
                        Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                        Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                    };
                    ContractsList.Add(Contract);
                }
            }
            else
            {
                foreach (var contract in context.Contracts)
                {
                    var Contract = new
                    {
                        Contract = contract,
                        Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                        Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                    };
                    ContractsList.Add(Contract);
                }
            }
            ContractsData = new
            {
                contracts = ContractsList,
                TermsOfPayment = context.TermsOfPayment,
                Clients = context.Clients
            };
            return ContractsData;
        }

        public KeyValuePair<bool, int> CreateContract(Object dataItem, int contractID)
        {
            try
            {
                JSONContractModel jsonContract = JsonConvert.DeserializeObject<JSONContractModel>(dataItem.ToString());
                var client = context.Clients.Where(c => c.name == jsonContract.ClientName).FirstOrDefault();
                if (client == null)
                {
                    context.Clients.Add(new Clients() { name = jsonContract.ClientName });
                    context.SaveChanges();
                }
                Model.Contracts contract = new Model.Contracts()
                {
                    id = jsonContract.id,
                    Amount = jsonContract.Amount,
                    Percent = jsonContract.PaymentPercent,
                    ClientId = context.Clients.Where(c =>
                        c.name == jsonContract.ClientName).FirstOrDefault().id,
                    ContractDate = Convert.ToDateTime(jsonContract.ContractDate),
                    ContractNumber = jsonContract.ContractNumber,
                    DeadlineCondition = jsonContract.DeadlineCondition,
                    Description = jsonContract.Description,
                    Notes = jsonContract.Notes,
                    OurDelivery = jsonContract.OurDelivery,
                    ReadyMark = jsonContract.ReadyMark,
                    SignatureMark = jsonContract.SignatureMark,
                    TermsOfPaymentId = context.TermsOfPayment.Where(top =>
                        top.name == jsonContract.TermsOfPaymentName).FirstOrDefault().id,
                    Comment = jsonContract.Comment,
                    HTMLSpecification = jsonContract.HTMLSpecification
                };
                if (contractID != 0 && contract.id != 0)
                {
                    return UpdateContract(contract);
                }
                else if ((contractID != 0 && contract.id == 0) || (contractID == 0 && contract.id != 0))
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                else if (contractID == 0 && contract.id == 0)
                {
                    context.Contracts.Add(contract);
                    context.SaveChanges();
                    int createdContractId = context.Contracts.Max(cid => cid.id);
                    return new KeyValuePair<bool, int>(true, createdContractId);
                }
                else return new KeyValuePair<bool, int>(false, 0);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public KeyValuePair<bool, int> UpdateContract(Model.Contracts contracts)
        {
            try
            {
                context.Entry(contracts).State = EntityState.Modified;
                context.SaveChanges();
                return new KeyValuePair<bool, int>(true, contracts.id);
            }
            catch
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }
    }
}
