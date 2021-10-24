using System;

namespace Persistence.Models
{
    public class AccountReadModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}