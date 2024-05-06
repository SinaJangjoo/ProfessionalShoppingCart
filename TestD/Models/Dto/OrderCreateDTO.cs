using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestD.Models.Dto
{
    public class OrderCreateDTO
    {
        public int OrderId { get; set; }
        [Required]
        public string PickupName { get; set; }
        [Required]
        public string PickupPhoneNumber { get; set; }
        [Required]
        public string PickupEmail { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public string ApplicationUserId { get; set; }
        public double OredrTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
    }
}
