﻿namespace Library.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public double OrderTotal { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}