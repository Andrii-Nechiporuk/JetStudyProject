using AutoMapper;
using Azure;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<CategoryDto> GetCategories()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return categoryDtos;
        }

        public async Task MultipleCreateCategories(List<string> categoryNames)
        {
            var categoryDtos = categoryNames.Select(x => new CategoryDto { Title = x.Trim() }).ToList();
            var existingCategories = GetCategories();
            var categoriesToCreate = categoryDtos.Where(c => !existingCategories.Any(ec => ec.Title == c.Title)).ToList();
            if (categoriesToCreate.Count > 0)
            {
                var categories = _mapper.Map<List<Category>>(categoriesToCreate);
                foreach (var category in categories)
                {
                    await _unitOfWork.CategoryRepository.Insert(category);
                }
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new HttpException("All of the categories already exist in the database", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteCategory(int categoryId)
        {
            if (IsExist(categoryId))
            {
                _unitOfWork.CategoryRepository.Delete(categoryId);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new HttpException("Category not found", HttpStatusCode.NotFound);
            }
        }

        public bool IsExist(int id)
        {
            if (id <= 0) return false;

            var tag = _unitOfWork.CategoryRepository.GetById(id);

            if (tag == null) return false;
            return true;
        }
    }
}
