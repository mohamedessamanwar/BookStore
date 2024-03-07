using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.BusnessLogic.ViewsModels.Atts;
using BookStore.DataAccessLayer.Interfaces;

namespace BookStore.BusnessLogic.ViewsModels.Caategory
{
    public class CategoryCreateDataView
    {
        private readonly IUnitOfWork unitOfWork;

        [NameUnique]
        public string Name { get; set; }
        [DisplayName("Order")]
        [Range(1, 100, ErrorMessage = "Range between 0 and 100 ")]     
        public int DisplayOrder { get; set; }

    }
}
