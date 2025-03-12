using GoodsAndOrders.Model;
using GoodsAndOrders.Model.Entities;
using Microsoft.EntityFrameworkCore;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace GoodsAndOrders.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ProductCategoryModelApi>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.ProductCategories.GetAll().ToListAsync();

            if (categories == null || categories.Count == 0)
                return Result<List<ProductCategoryModelApi>>.Fail("Категории не найдены", 404);

            var list = categories.Select(c => new ProductCategoryModelApi
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Result<List<ProductCategoryModelApi>>.Success(list);
        }
    }
}
