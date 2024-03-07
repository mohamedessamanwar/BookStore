using BookStore.BusnessLogic.ViewsModels.Caategory;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryViewDataView> CreateCategory(CategoryCreateDataView categoryCreateDataView);
        Task<List<CategoryViewDataView>> ViewCategory();

        Task<List<SelectListItem>> GetSelectListAsync(); 
    }
}
