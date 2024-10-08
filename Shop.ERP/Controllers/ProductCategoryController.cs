namespace Shop.ERP.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryService productCategoryService;
        public ProductCategoryController(ProductCategoryService _productCategoryService)
        {
            productCategoryService = _productCategoryService;
        }
        public IActionResult Index()
        {
            var objList = productCategoryService.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {
            return View("AddUpdate", new PRODUCT_CATEGORY());
        }
        public IActionResult Edit(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = productCategoryService.GetById(id);
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
        public IActionResult AddUpdate(PRODUCT_CATEGORY obj)
        {
            EQResult eQResult = new EQResult();
            if (ModelState.IsValid)
            {
                eQResult = productCategoryService.Save(obj);
                TempData["msg"] = eQResult.messages;
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResult eQResult = productCategoryService.Delete(id);
                TempData["msg"] = eQResult.messages;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["msg"] = NotifyService.Error("Invalid ID, Parameter is required");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
