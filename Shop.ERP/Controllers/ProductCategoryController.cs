using Microsoft.AspNetCore.Mvc;
using Shop.ERP.Models;

namespace Shop.ERP.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext db;
        public ProductCategoryController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            //List<PRODUCT_CATEGORY> CategoryList = new List<PRODUCT_CATEGORY>();

            //PRODUCT_CATEGORY category = new PRODUCT_CATEGORY();
            //category.ID = 1;
            //category.CATEGORY_NAME = "Food Products";
            //CategoryList.Add(category);

            //category = new PRODUCT_CATEGORY();
            //category.ID = 2;
            //category.CATEGORY_NAME = "Cosmetics Products";
            //CategoryList.Add(category);




            //Linq
            //select * from PRODUCT_CATEGORY
            //SELECT [p].[ID], [p].[CATEGORY_NAME] FROM[PRODUCT_CATEGORY] AS[p]
            List<PRODUCT_CATEGORY> CategoryList = db.PRODUCT_CATEGORY.ToList();

            return View(CategoryList);
        }

        public IActionResult Create()
        {
            PRODUCT_CATEGORY category = new PRODUCT_CATEGORY();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(PRODUCT_CATEGORY category)
        {
            return View(category);
        }
    }
}