using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        Task MultipleCreateCategories(List<string> categoryNames);
        Task DeleteCategory(int id);
        bool IsExist(int id);
    }
}
