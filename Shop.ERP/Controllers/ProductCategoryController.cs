using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //var catData = db.PRODUCT_CATEGORY.FromSqlRaw("exec sp_get_product_category").ToList();

            //var catData2 = db.ExecuteComplexStoredProcedureAsync();

            return View(CategoryList);
        }

        public IActionResult Create()
        {
            PRODUCT_CATEGORY category = new PRODUCT_CATEGORY();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(PRODUCT_CATEGORY obj)
        {
            if (ModelState.IsValid)
            {
                //DB Operation
                PRODUCT_CATEGORY category = db.PRODUCT_CATEGORY.Find(obj.ID);
                if (category == null)
                {
                    db.PRODUCT_CATEGORY.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    category.CATEGORY_NAME = obj.CATEGORY_NAME;
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
                //return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Edit(string id)
        {
            PRODUCT_CATEGORY category = db.PRODUCT_CATEGORY.Find(id);
            string sql_with_pk = $"select * from PRODUCT_CATEGORY where ID='{id}'";

            PRODUCT_CATEGORY category1 = db.PRODUCT_CATEGORY.Where(p => p.ID == id).FirstOrDefault();
            //string sql_without_pk = $"select * from PRODUCT_CATEGORY where ID='{id}'";

            return View("Create", category);
        }

    }
}