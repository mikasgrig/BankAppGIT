using System;
using Contract.Enum;

namespace Contract.Models.Response
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime DateCreate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}