using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestD.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }
		[Required]
		public string PickupName { get; set; }
		[Required]
		public string PickupPhoneNumber { get; set; }
		[Required]
		public string PickupEmail { get; set; }
        [Required]
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }
        public double OredrTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
    }
}
