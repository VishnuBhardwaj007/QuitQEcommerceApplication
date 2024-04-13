namespace QuitQ_Ecom.DTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; }
        public string IfscCode { get; set; }
    }
}
