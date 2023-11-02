using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Web.Models;
using Web.Services.IServices;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        public HomeController(ILogger<HomeController> logger, IProductService productService
            , ICartService cartService)
        {
            _logger = logger;
            this.productService = productService;
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? list = new List<ProductDTO>();
            ResponseDTO? response = await productService.GetAllAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(string Id)
        {
            ResponseDTO? response = await productService.GetByIdAsync(Id);
            if (response != null && response.IsSuccess)
            {
                ProductDTO? product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(product);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
        {
            CartDTO cart = new CartDTO
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value
                },
                CartDetails = new List<CartDetailsDTO>
                {
                    new CartDetailsDTO
                    {
                        ProductId = productDTO.Id,
                        Count = productDTO.Count
                    }
                }
            };
            ResponseDTO? response = await cartService.UpsertCart(cart);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = $"\"{productDTO.Name}\" has been added to the Shopping Cart";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDTO);
        }







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
