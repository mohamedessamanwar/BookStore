﻿namespace BookStore.BusnessLogic.ViewsModels.ShoppingCart
{
    public class CartView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public Double Price { get; set; }

        public Double LastPrice { get; set; }

        public string imageUrl { get; set; }

        public int count { get; set; }
    }
}
