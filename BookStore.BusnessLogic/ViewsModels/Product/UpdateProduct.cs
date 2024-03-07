using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.ViewsModels.Product
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public string Author { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        [Range(0, 100000, ErrorMessage = " range from 0 to 100000")]
        public Double Price { get; set; }
        public Double LastPrice { get; set; }

        public bool active { get; set; }
    }
}
