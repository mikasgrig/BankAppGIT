using System;

namespace Contract.Models.Response
{
    public class SignUpResponse
    {
        public Guid Id { get; set; }

        public string IdToken { get; set; }
        
        public string Email { get; set; }
        
        public DateTime DateCreated { get; set; }
    }
}