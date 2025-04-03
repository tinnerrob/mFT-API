namespace mFT_API.Models
{
	public class User
	{
        public int UserID { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PasswordSalt { get; set; }
        public required string PasswordHash { get; set; }
        public required int GroupID { get; set; }
        public required bool Active { get; set; }
       // public required DateTime SubscriptionDate { get; set; }
    }
}

