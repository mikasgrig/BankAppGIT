using System;

namespace Contract.Models
{
    public class DepositWriteModel
    {
        public decimal Amount { get; set; }
        public decimal AccountAmount { get; set; }
        public Guid AccountId  { get; set; }
    }
}