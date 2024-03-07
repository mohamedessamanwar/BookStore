using BookStore.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.ViewsModels.Product
{
    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public Double Price { get; set; }
 
    }
}
