using Application.Commands.Categories.CreateCategory;
using Application.Commands.Categories.UpdateCategory;
using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commons.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Map từ CreateCategoryCommand -> Entity Category
            CreateMap<CreateCategoryCommand, Category>();

            // Map từ UpdateCategoryCommand -> Entity Category
            CreateMap<UpdateCategoryCommand, Category>();

            // Map từ Entity Category -> CategoryDto (Bao gồm tự động map danh sách SubCategories)
            CreateMap<Category, CategoryDto>();
        }
    }
}
