namespace Contract.Models.Request
{
    public class SignUpRequest
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
        public string AccountName { get; set; }
    }
}