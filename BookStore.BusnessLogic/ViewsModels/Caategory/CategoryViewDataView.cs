using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.ViewsModels.Caategory
{
    public class CategoryViewDataView
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ModifiedAt { get; set; }
    }
}
