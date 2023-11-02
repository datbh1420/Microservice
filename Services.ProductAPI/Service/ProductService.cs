using BackEnd.ProductAPI.Data;
using BackEnd.ProductAPI.Models;
using BackEnd.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.ProductAPI.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string Id);
        Task<Product?> CreateAsync(ProductDTO productDto);
        Task<Product?> UpdateAsync(ProductDTO productDto);
        Task<Product?> DeleteByIdAsync(string Id);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductService(AppDbContext context, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Product?> CreateAsync(ProductDTO productDto)
        {
            try
            {
                Product? productExist = await context.products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
                if (productExist is null)
                {
                    Product product = (Product)productDto;
                    await context.products.AddAsync(product);
                    await context.SaveChangesAsync();

                    if (productDto.Image is not null)
                    {
                        string filePath = @"Images\" + $"{productDto.Image.FileName}{Path.GetExtension(productDto.Image.FileName)}";
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            await productDto.Image.CopyToAsync(fileStream);
                        }
                        product.ImageLocalPath = filePath;
                        product.ImagePath = Path.Combine(webHostEnvironment.ContentRootPath, filePath);
                    }
                    else
                    {
                        product.ImagePath = "https://placehold.co/600x400";
                    }
                    context.products.Update(product);
                    context.SaveChanges();
                    return product;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Product?> DeleteByIdAsync(string Id)
        {
            try
            {
                Product? product = await context.products.FirstOrDefaultAsync(x => x.Id == Id);
                if (product is not null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    context.products.Remove(product);
                    await context.SaveChangesAsync();
                    return product;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                IEnumerable<Product> list = await context.products.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Product?> GetByIdAsync(string Id)
        {
            try
            {
                Product? entity = await context.products.FirstOrDefaultAsync(x => x.Id == Id);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Product?> UpdateAsync(ProductDTO productDto)
        {
            try
            {
                Product? productExist = await context.products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
                if (productExist is not null)
                {
                    Product product = (Product)productDto;

                    if (productDto.Image is not null)
                    {
                        //DeleteOldImange
                        if (!string.IsNullOrEmpty(product.ImageLocalPath))
                        {
                            var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                            FileInfo file = new FileInfo(oldFilePathDirectory);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }

                        //UpdateNewImage
                        string filePath = @"Images\" + $"{productDto.Image.FileName}";
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            await productDto.Image.CopyToAsync(fileStream);
                        }
                        product.ImageLocalPath = filePath;
                        product.ImagePath = Path.Combine(webHostEnvironment.ContentRootPath, filePath);
                    }
                    else
                    {
                        product.ImagePath = "https://placehold.co/600x400";
                    }
                    context.products.Update(product);
                    context.SaveChanges();
                    return product;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
