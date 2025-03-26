using System;
namespace mFT_API.Models
{
	public class User
	{
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required int GroupID { get; set; }
	}
}

