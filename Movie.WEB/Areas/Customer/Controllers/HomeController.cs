using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Diagnostics;
using System.Security.Claims;

namespace Movie.WEB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Products.GetAll(includeProperties: "Genre");

            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.Products.GetOne(x => x.Id == productId, includeProperties: "Genre"),
                ProductId = productId,
                Count = 1
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCarts.GetOne(x => x.ApplicationUserId == claim.Value && x.ProductId == shoppingCart.ProductId);

            if(cartFromDb == null)
            {
                _unitOfWork.ShoppingCarts.Add(shoppingCart);
                _unitOfWork.Save();
            }

            else
            {
                _unitOfWork.ShoppingCarts.IncrementCount(cartFromDb, shoppingCart.Count);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}