using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccessLayer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }
        public Double Price { get; set; }
        public Double LastPrice { get; set; }

        public bool active { get; set; }


    }
}
