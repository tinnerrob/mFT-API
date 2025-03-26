using System;
namespace mFT_API.Models
{
	public class Category
	{
        public required string CategoryName { get; set; }
        public required string Color { get; set; }
        public required string Icon { get; set; }
        public required string Type { get; set; }
        public required int GroupID { get; set; }
    }
}

