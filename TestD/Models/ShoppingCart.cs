﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TestD.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }  // One ShoppingCart can have multiple CartItem

        [NotMapped]  
        public double CartTotal { get; set; }
        [NotMapped]
        public string PaymentId { get; set; }
        [NotMapped]
        public string ClientSecret { get; set; }
    }
}
