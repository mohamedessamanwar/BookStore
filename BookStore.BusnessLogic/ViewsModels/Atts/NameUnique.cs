using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using BookStore.DataAccessLayer.Reposatories.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.ViewsModels.Atts
{
    public class NameUnique : ValidationAttribute
    {
        private readonly IUnitOfWork unitOfWork ;
        private readonly ApplicationDbContext applicationDbContext ;
        

        public NameUnique()
        {
            this.applicationDbContext = new ApplicationDbContext();
            this.unitOfWork = new UnitOfWork(applicationDbContext);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

             var name = value as string;
             if (value is not null )
             {
                var category = unitOfWork.GetRepository<Category>().Find(c=> c.Name == name);
                if (category != null)
                {
                    return new ValidationResult($"Name is aready exixt");
                }
             }

             return ValidationResult.Success;
        }
    }
}
