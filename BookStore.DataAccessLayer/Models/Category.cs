using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DataAccessLayer.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public int  DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ModifiedAt { get; set; } 


    }
}
