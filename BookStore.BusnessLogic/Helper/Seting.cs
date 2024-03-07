using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusnessLogic.Helper
{
    public static class Seting 
    {
        public const string ImagesPath = "F:\\BookStore\\BookStore.BusnessLogic\\Helper\\assets\\images\\products";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
