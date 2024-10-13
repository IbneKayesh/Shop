namespace Shop.ERP.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductsService productsService;
        private readonly ProductCategoryService productCategoryService;
        private readonly UnitsService unitsService;
        public ProductController(AppDbContext dbContext, ProductsService _productsService,
            ProductCategoryService _productCategoryService,
            UnitsService _unitsService)
        {
            productsService = _productsService;
            productCategoryService = _productCategoryService;
            unitsService = _unitsService;
        }

        public IActionResult Index()
        {
            var objList = productsService.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {
            Dropdown_CreateEdit();
            return View("AddUpdate", new PRODUCTS());
        }


        public IActionResult Edit(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                Dropdown_CreateEdit();

                var entity = productsService.GetById(id);
                if (entity != null)
                {
                    return View("AddUpdate", entity);
                }
                else
                {
                    TempData["msg"] = NotifyService.NotFound();
                }
            }
            else
            {
                TempData["msg"] = NotifyService.Error("Invalid ID, Parameter is required");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddUpdate(PRODUCTS obj)
        {
            EQResult eQResult = new EQResult();
            if (ModelState.IsValid)
            {
                eQResult = productsService.Save(obj);
                TempData["msg"] = eQResult.messages;
                return RedirectToAction(nameof(Index));
            }

            Dropdown_CreateEdit();
            return View(obj);
        }

        public IActionResult Delete(string id)
        {
            //if (!string.IsNullOrWhiteSpace(id))
            //{
            //    var entity = db.PRODUCTS.Find(id);
            //    if (entity != null)
            //    {
            //        db.PRODUCTS.Remove(entity);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        //not found
            //    }
            //}
            //else
            //{
            //    //invalid request
            //}
            return RedirectToAction(nameof(Index));
        }

        private void Dropdown_CreateEdit()
        {
            ViewBag.CATEGORY_ID = productCategoryService.GetCategoryListItems();
            ViewBag.UNIT_ID = unitsService.GetUnitListItems();
        }



        [Route("api/Products/GetByCategoryID")]
        [HttpGet]
        public IActionResult GetAllByCategoryID(string id)
        {
            var objList = productsService.GetByCategoryId(id);
            return Json(objList);
        }

    }
}