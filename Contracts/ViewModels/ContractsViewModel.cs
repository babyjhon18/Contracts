using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public Object GetContracts(int contractsType)
        {
            List<Object> ContractsList = new List<Object>();
            Object ContractsData = new Object();
            switch (contractsType)
            {
                case 0:
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
                    break;
                case 1:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == false))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        ContractsList.Add(Contract);
                    }
                    break;
                case 2:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        if (CheckIfNotFullPayment(Contract.Contract, Contract.Acts.ToList(), Contract.Payments.ToList()))
                        {
                            ContractsList.Add(Contract);
                        }
                    }
                    break;
                case 3:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        if (CheckIfFullPayment(Contract.Contract, Contract.Acts.ToList(), Contract.Payments.ToList()))
                        {
                            ContractsList.Add(Contract);
                        }
                    }
                    break;
                case 4:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        if(CheckIfPaymentExists(Contract.Contract, Contract.Acts.ToList(), Contract.Payments.ToList()))
                            ContractsList.Add(Contract);
                    }
                    break;
            }
            ContractsData = new
            {
                contracts = ContractsList,
                TermsOfPayment = context.TermsOfPayment,
                Clients = context.Clients,
                BlackListClients = context.BlackListCompanies,
            };
            return ContractsData;
        }

        private Object CountPayment(IEnumerable<Acts> Acts, IEnumerable<Payments> Payments)
        {
            double ActsSum = 0;
            double PaymentsSum = 0;
            foreach (var act in Acts)
            {
                ActsSum += act.ActPayment;
            }
            foreach (var payment in Payments)
            {
                PaymentsSum += payment.PaymentSum;
            }
            return new FullPayments() { ActsSum = ActsSum, PaymentsSum = PaymentsSum };
        }

        private bool CheckIfPaymentExists(Model.Contracts Contract, List<Acts> Acts, List<Payments> Payments)
        {
            if (Payments.Count() != 0 && Acts.Count() != 0)
            {
                var PaidFull = CountPayment(Acts, Payments) as FullPayments;
                if(PaidFull.PaymentsSum != PaidFull.ActsSum && 
                   PaidFull.PaymentsSum == Contract.Amount)
                    return true;
                else
                    return false;
            }
            return true;
        }

        private bool CheckIfNotFullPayment(Model.Contracts Contract, List<Acts> Acts, List<Payments> Payments)
        {
            if (Acts.Count() != 0 && Payments.Count() != 0)
            {
                var FullPayments = CountPayment(Acts, Payments) as FullPayments;
                if (FullPayments.PaymentsSum != Contract.Amount)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private bool CheckIfFullPayment(Model.Contracts Contract, List<Acts> Acts, List<Payments> Payments)
        {
            if (Acts != null && Payments != null)
            {
                if (Acts.Count() != 0 && Payments.Count() != 0)
                {
                    var FullPayments = CountPayment(Acts, Payments) as FullPayments;
                    if (FullPayments.ActsSum == FullPayments.PaymentsSum && FullPayments.ActsSum == Contract.Amount && FullPayments.PaymentsSum == Contract.Amount)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public KeyValuePair<bool, int> CreateContract(Object dataItem, int contractID)
        {
            try
            {
                JSONContractModel jsonContract = JsonConvert.DeserializeObject<JSONContractModel>(dataItem.ToString());
                var client = context.Clients.Where(c => c.FullName == jsonContract.ClientName).FirstOrDefault();
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
                        c.FullName == jsonContract.ClientName).FirstOrDefault().id,
                    ContractDate = Convert.ToDateTime(jsonContract.ContractDate),
                    ContractNumber = jsonContract.ContractNumber,
                    DeadLine = jsonContract.DeadLine,
                    DeadLineDayType = jsonContract.DeadLineDayType,
                    DeadLineDaySetted = jsonContract.DeadLineDaySetted,
                    Description = jsonContract.Description,
                    ReadyComment = jsonContract.ReadyComment,
                    OurDelivery = jsonContract.OurDelivery,
                    ReadyMark = jsonContract.ReadyMark,
                    SignatureMark = jsonContract.SignatureMark,
                    TermsOfPaymentId = context.TermsOfPayment.Where(top =>
                        top.name == jsonContract.TermsOfPaymentName).FirstOrDefault().id,
                    Comment = jsonContract.Comment,
                    HTMLSpecification = jsonContract.HTMLSpecification,
                    SignDate = Convert.ToDateTime(jsonContract.SignDate),
                    ManufacturingLeadNotes = jsonContract.ManufacturingLeadNotes,
                    ManufacturingLeadNoteColor = jsonContract.ManufacturingLeadNoteColor,
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
