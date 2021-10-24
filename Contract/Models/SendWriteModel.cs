using System;

namespace Contract.Models
{
    public class SendWriteModel
    {
        public decimal SendAmount { get; set; }
        public decimal SenderAccountAmount { get; set; }
        public decimal ReceiverAccountAmount { get; set; }
        public Guid SenderAccountId  { get; set; }
        public Guid ReceiverAccountId { get; set; }
        public string ReceiverAccountName { get; set; }
        public string SenderAccountName { get; set; }
    }
}