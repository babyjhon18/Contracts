using Contracts.JSONViewModels;
using Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Morpher.WebService.V2;
using Newtonsoft.Json;
using Spire.Doc.Fields;
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
                //Реестр
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
                //План работ
                case 1:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == false))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        if(Contract.Contract.NotForWorkPlan == false) ContractsList.Add(Contract);
                    }
                    break;
                //Неоплаченные
                case 2:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        FullPayments payments = CountPayment(Contract.Acts, Contract.Payments);
                        if (CheckIfNotFullPayment(Contract.Contract, Contract.Acts.ToList(), Contract.Payments.ToList()) && 
                            Contract.Contract.NotForWorkPlan == false && 
                            payments.PaymentsSum < payments.ActsSum && 
                            (payments.ActsSum > 0 || payments.ActsSum == Contract.Contract.Amount)) ContractsList.Add(Contract);
                        if (Contract.Contract.NotForWorkPlan == true &&
                            payments.ActsSum == Contract.Contract.Amount &&
                            payments.PaymentsSum < payments.ActsSum &&
                            payments.PaymentsSum < Contract.Contract.Amount &&
                            payments.PaymentsSum != 0
                            ) ContractsList.Add(Contract);
                    }
                    break;
                //Архив
                case 3:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };
                        if (CheckIfFullPayment(Contract.Contract, Contract.Acts.ToList(), Contract.Payments.ToList())) ContractsList.Add(Contract);
                    }
                    break;
                //Готовые
                case 4:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true))
                    {
                        var Contract = new
                        {
                            Contract = contract,
                            Payments = context.Payments.Where(p => p.FK_ContractId == contract.id),
                            Acts = context.Acts.Where(p => p.FK_ContractId == contract.id),
                        };

                        if (Contract.Contract.NotForWorkPlan == false && Contract.Contract.ReadyMark == true)
                        {
                            var fullPayments = CountPayment(Contract.Acts.ToList(), Contract.Payments.ToList());
                            if (fullPayments.ActsSum != Contract.Contract.Amount && 
                                fullPayments.PaymentsSum <= Contract.Contract.Amount
                                )
                                ContractsList.Add(Contract);
                        }
                        FullPayments payments = CountPayment(Contract.Acts, Contract.Payments);
                        if (Contract.Contract.NotForWorkPlan == true &&
                            Contract.Payments.Count() > 0 &&
                            Contract.Contract.TermsOfPaymentId != 3 &&
                            Contract.Contract.Amount != payments.ActsSum && 
                            Contract.Contract.Amount >= payments.PaymentsSum) { 
                            ContractsList.Add(Contract); }
                        else if (Contract.Contract.NotForWorkPlan == true &&
                            Contract.Contract.TermsOfPaymentId == 3 &&
                            ((Contract.Acts.Count() == 0 &&
                            Contract.Payments.Count() == 0) ||
                            Contract.Contract.Amount != payments.ActsSum)) { ContractsList.Add(Contract); }
                    }
                    break;
                //ПНР
                case 5:
                    foreach (var contract in context.Contracts.Where(c => c.SignatureMark == true && c.ReadyMark == false && c.NotForWorkPlan == true))
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
            }
            ContractsData = new
            {
                contracts = ContractsList,
                TermsOfPayment = context.TermsOfPayment,
                Clients = context.Clients,
                BlackListClients = context.BlackListCompanies,
                TechnicalConditions = context.TechnicalConditions,
            };
            return ContractsData;
        }

        private FullPayments CountPayment(IEnumerable<Acts> Acts, IEnumerable<Payments> Payments)
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
            return false;
        }

        private bool CheckIfNotFullPayment(Model.Contracts Contract, List<Acts> Acts, List<Payments> Payments)
        {
            var FullPayments = CountPayment(Acts, Payments);
            if (FullPayments.PaymentsSum < Contract.Amount)
                return true;
            else
                return false;
        }

        private bool CheckIfFullPayment(Model.Contracts Contract, List<Acts> Acts, List<Payments> Payments)
        {
            if (Acts != null && Payments != null)
            {
                if (Acts.Count() != 0 && Payments.Count() != 0)
                {
                    var FullPayments = CountPayment(Acts, Payments) as FullPayments;
                    if (FullPayments.ActsSum <= FullPayments.PaymentsSum && FullPayments.ActsSum == Contract.Amount && FullPayments.PaymentsSum >= Contract.Amount)
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
                //var client = context.Clients.Where(c => c.FullName == jsonContract.ClientName).FirstOrDefault();
                var existedContract = context.Contracts.Where(ct => ct.id == jsonContract.id).FirstOrDefault();
                //if (client == null)
                //{
                //    context.Clients.Add(new Clients() { name = jsonContract.ClientName });
                //    context.SaveChanges();
                //}
                Model.Contracts contract = new Model.Contracts()
                {
                    Amount = Convert.ToDouble(jsonContract.Amount),
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
                    TermsOfPaymentId = context.TermsOfPayment.Where(top =>
                        top.name == jsonContract.TermsOfPaymentName).FirstOrDefault().id,
                    Comment = jsonContract.Comment,
                    HTMLSpecification = jsonContract.HTMLSpecification,
                    ManufacturingLeadNotes = jsonContract.ManufacturingLeadNotes,
                    ManufacturingLeadNoteColor = jsonContract.ManufacturingLeadNoteColor,
                    NotForWorkPlan = jsonContract.NotForWorkPlan,
                    PaymentDelay= jsonContract.PaymentDelay,
                    PaymentDelayDayType= jsonContract.PaymentDelayDayType,
                    ContractAcceptedBy = jsonContract.ContractAcceptedBy,
                    SignDate = Convert.ToDateTime(jsonContract.SignDate),
                    ReadyMark = jsonContract.ReadyMark,
                    SignatureMark = jsonContract.SignatureMark,
                    SawContract = jsonContract.SawContract,
                };
                if (contractID != 0 && jsonContract.id != 0)
                {
                    existedContract.Amount = Convert.ToDouble(jsonContract.Amount);
                    existedContract.Percent = jsonContract.PaymentPercent;
                    existedContract.ClientId = context.Clients.Where(c =>
                        c.FullName == jsonContract.ClientName).FirstOrDefault().id;
                    existedContract.ContractDate = Convert.ToDateTime(jsonContract.ContractDate);
                    existedContract.ContractNumber = jsonContract.ContractNumber;
                    existedContract.DeadLine = jsonContract.DeadLine;
                    existedContract.DeadLineDayType = jsonContract.DeadLineDayType;
                    existedContract.DeadLineDaySetted = jsonContract.DeadLineDaySetted;
                    existedContract.Description = jsonContract.Description;
                    existedContract.ReadyComment = jsonContract.ReadyComment;
                    existedContract.OurDelivery = jsonContract.OurDelivery;
                    existedContract.TermsOfPaymentId = context.TermsOfPayment.Where(top =>
                        top.name == jsonContract.TermsOfPaymentName).FirstOrDefault().id;
                    existedContract.Comment = jsonContract.Comment;
                    existedContract.HTMLSpecification = jsonContract.HTMLSpecification;
                    existedContract.ManufacturingLeadNotes = jsonContract.ManufacturingLeadNotes;
                    existedContract.ManufacturingLeadNoteColor = jsonContract.ManufacturingLeadNoteColor;
                    existedContract.NotForWorkPlan = jsonContract.NotForWorkPlan;
                    existedContract.PaymentDelay = jsonContract.PaymentDelay;
                    existedContract.PaymentDelayDayType = jsonContract.PaymentDelayDayType;
                    existedContract.ContractAcceptedBy = jsonContract.ContractAcceptedBy;
                    if (jsonContract.SignDate != "" && jsonContract.SignDate != null)
                    {
                        existedContract.SignDate = Convert.ToDateTime(jsonContract.SignDate);
                        existedContract.ReadyMark = jsonContract.ReadyMark;
                        existedContract.SignatureMark = jsonContract.SignatureMark;
                        existedContract.SawContract = jsonContract.SawContract;
                        if (jsonContract.SignatureMark && existedContract.DayToPlan == DateTime.MinValue)
                        {
                            existedContract.DayToPlan = DateTime.Now;
                        }
                    }
                    return UpdateContract(existedContract);
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
            catch(Exception ex)
            {
                return new KeyValuePair<bool, int>(false, 0);
            }
        }

        public bool DeleteContract(int contractId)
        {
            try
            {
                context.Entry(context.Contracts.Where(c => c.id == contractId).FirstOrDefault()).State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
