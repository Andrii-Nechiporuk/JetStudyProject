using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetStudyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Returns the list of existing categories
        /// </summary>
        [HttpGet]
        public List<CategoryDto> GetPreviewCategories()
        {
            return _categoryService.GetCategories();
        }

        /// <summary>
        /// Creates categories from a list of categories separated by commas
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MultipleCreateCategories([FromBody] List<string> categoryNames)
        {
            await _categoryService.MultipleCreateCategories(categoryNames);
            return Ok();
        }

        /// <summary>
        /// Deletes category from the database
        /// </summary>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
