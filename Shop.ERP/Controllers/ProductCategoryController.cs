using Microsoft.AspNetCore.Mvc;
using Shop.ERP.Models;

namespace Shop.ERP.Controllers
{
    public class ProductCategoryController : Controller
    {
        public IActionResult Index()
        {
            PRODUCT_CATEGORY category = new PRODUCT_CATEGORY();
            category.ID = 1;
            category.CATEGORY_NAME = "Food Products";
            return View(category);
        }
    }
}
