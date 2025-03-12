using GoodsAndOrders.Model;
using GoodsAndOrders.Model.Entities;
using Microsoft.EntityFrameworkCore;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Utils;
using GoodsAndOrders.Common;

namespace GoodsAndOrders.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetProductsAsync(int page, int pageSize, string? search, decimal? minPrice, decimal? maxPrice, Guid? categoryId)
        {
            if (page < 1 || pageSize < 1)
                return Result<object>.Fail("Некорректные параметры пагинации", 400);

            var query = _unitOfWork.Products.GetAll().Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            int totalItems = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new
            {
                items = products.Select(p => new ProductModelApi
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Code = p.Code,
                    CategoryName = p.Category.Name
                }),
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems
            };

            return Result<object>.Success(data);
        }

        public async Task<Result<UpdateProductModelApi>> CreateProductAsync(UpdateProductModelApi? createProductModelApi)
        {
            var validation = ValidateProduct(createProductModelApi);
            if (!validation.IsSuccess)
                return validation;

            var category = _unitOfWork.ProductCategories.GetAll().FirstOrDefault(c => c.Id == createProductModelApi.CategoryId);
            if (category == null)
                return Result<UpdateProductModelApi>.Fail("Ошибка валидации, категория не найдена", 404);

            var existingCodes = await _unitOfWork.Products.GetAll().Select(p => p.Code).ToListAsync();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createProductModelApi.Name,
                Price = createProductModelApi.Price,
                Code = ProductCodeGenerator.GenerateUniqueCode(existingCodes),
                CategoryId = createProductModelApi.CategoryId
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateProductModelApi>.Success(createProductModelApi);
        }

        public async Task<Result<UpdateProductModelApi>> UpdateProductAsync(Guid id, UpdateProductModelApi? updateProductModelApi)
        {
            var validation = ValidateProduct(updateProductModelApi);
            if (!validation.IsSuccess)
                return validation;

            var product = await _unitOfWork.Products.FindAsync(id);
            if (product == null)
                return Result<UpdateProductModelApi>.Fail("Товар не найден", 404);

            var category = _unitOfWork.ProductCategories.GetAll().FirstOrDefault(c => c.Id == updateProductModelApi.CategoryId);
            if (category == null)
                return Result<UpdateProductModelApi>.Fail("Ошибка валидации, проверьте категорию", 404);

            product.Name = updateProductModelApi.Name;
            product.Price = updateProductModelApi.Price;
            product.CategoryId = updateProductModelApi.CategoryId;

            await _unitOfWork.SaveChangesAsync();
            return Result<UpdateProductModelApi>.Success(updateProductModelApi);
        }

        public async Task<Result<UpdateProductModelApi>> DeleteProductAsync(Guid id)
        {
            var product = await _unitOfWork.Products.FindAsync(id);

            if (product == null)
                return Result<UpdateProductModelApi>.Fail("Товар не найден", 404);

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();


            var deletedProduct = new UpdateProductModelApi
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };

            return Result<UpdateProductModelApi>.Success(deletedProduct);
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <param name="productModelApi"></param>
        /// <returns></returns>
        private Result<UpdateProductModelApi> ValidateProduct(UpdateProductModelApi? productModelApi)
        {
            if (productModelApi == null)
                return Result<UpdateProductModelApi>.Fail("Некорректный запрос, данные отсутствуют", 400);

            if (string.IsNullOrWhiteSpace(productModelApi.Name) || productModelApi.Name.Length > 20)
                return Result<UpdateProductModelApi>.Fail("Ошибка валидации, проверьте наименование", 422);

            if (productModelApi.Price <= 0 || productModelApi.Price >= 1000000)
                return Result<UpdateProductModelApi>.Fail("Ошибка валидации, проверьте цену", 422);

            if (productModelApi.CategoryId == Guid.Empty)
                return Result<UpdateProductModelApi>.Fail("Ошибка валидации, категория обязательна", 422);

            return Result<UpdateProductModelApi>.Success(productModelApi);
        }
    }
}
