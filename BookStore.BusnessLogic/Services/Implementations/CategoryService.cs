using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Caategory;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.BusnessLogic.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryViewDataView> CreateCategory(CategoryCreateDataView categoryCreateDataView)
        {
            //mapping 
            Category category = new Category
            {
                Name = categoryCreateDataView.Name,
                DisplayOrder = categoryCreateDataView.DisplayOrder,
            };
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            _unitOfWork.Complete();
            return new CategoryViewDataView
            {
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
                CreatedAt = category.CreatedAt

            };
        }
        public async Task<List<CategoryViewDataView>> ViewCategory(string name, int skip, int pageSize)
        {
            List<Category> categories = (List<Category>)await _unitOfWork.GetRepository<Category>().GetAllAsync();
            // if (name != null)
            //{
            categories = categories.Where(p => p.Name.Contains(name)).ToList();
            //
            return categories.Skip(skip).Take(pageSize).Select(e => new CategoryViewDataView { Id = e.Id, Name = e.Name }).ToList();
        }
        public async Task<List<SelectListItem>> GetSelectListAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var selectList = categories.Select(c => new { c.Id, c.Name });
            return selectList.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).OrderBy(e => e.Text).ToList();
        }
    }
}
