using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Caategory;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork  _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  async Task<CategoryViewDataView> CreateCategory(CategoryCreateDataView categoryCreateDataView)
        {
            //mapping 
            Category category= new Category
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
        public async Task<List<CategoryViewDataView>> ViewCategory()
        { 
            List<Category> categories   = (List<Category>) await _unitOfWork.GetRepository<Category>().GetAllAsync();
            return categories.Select(e=> new CategoryViewDataView { CreatedAt=e.CreatedAt,Id=e.Id,DisplayOrder=e.DisplayOrder,Name=e.Name} ).ToList();      
        }
        public async Task<List<SelectListItem>> GetSelectListAsync() {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var selectList  = categories.Select(c => new { c.Id, c.Name });
            return selectList.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).OrderBy(e => e.Text).ToList(); 
        }
    }
}
