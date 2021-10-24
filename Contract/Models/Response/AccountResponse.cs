using System;

namespace Contract.Models.Response
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}