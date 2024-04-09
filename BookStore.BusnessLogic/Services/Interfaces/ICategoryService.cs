using BookStore.BusnessLogic.ViewsModels.Caategory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.BusnessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryViewDataView> CreateCategory(CategoryCreateDataView categoryCreateDataView);
        Task<List<CategoryViewDataView>> ViewCategory(string name, int skip, int pageSize);
        Task<List<SelectListItem>> GetSelectListAsync();
    }
}
