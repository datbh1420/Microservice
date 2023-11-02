using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Web.Services.IServices;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            ResponseDTO? response = await productService.GetAllAsync();
            List<ProductDTO> list = new();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public IActionResult CreateProduct()
        {
            ProductDTO ProductDTO = new();
            return View(ProductDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO ProductDTO)
        {
            ResponseDTO? response = await productService.CreateAsync(ProductDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Create Successful";
                return RedirectToAction("ProductIndex");
            }
            TempData["error"] = response.Message;
            return View(ProductDTO);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            ResponseDTO? response = await productService.GetByIdAsync(Id);
            if (ModelState.IsValid)
            {
                if (response != null && response.IsSuccess)
                {
                    ProductDTO? Product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                    return View(Product);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDTO ProductDTO)
        {

            if (ModelState.IsValid)
            {
                ResponseDTO? response = await productService.DeleteAsync(ProductDTO.Id);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Delete Successfully";
                    return RedirectToAction("ProductIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(ProductDTO);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string Id)
        {
            ResponseDTO? response = await productService.GetByIdAsync(Id);
            if (ModelState.IsValid)
            {
                if (response != null && response.IsSuccess)
                {
                    ProductDTO? Product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                    return View(Product);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductDTO ProductDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await productService.UpdateAsync(ProductDTO);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Update Successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(ProductDTO);
        }
    }
}
