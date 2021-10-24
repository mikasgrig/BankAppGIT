using System;
using System.ComponentModel.DataAnnotations;

namespace Contract.Models.Request
{
    public class SendRequest
    {
        [Required]
        [Range(typeof(decimal), "0.0001", "79228162514264337593543950335")]
        public decimal Amount { get; set; }

        public Guid AccountId { get; set; }
    }
}