namespace Shop.ERP.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private readonly UnitsService unitsService;
        public SalesInvoiceController(UnitsService _unitsService)
        {
            unitsService = _unitsService;
        }
        public IActionResult Index()
        {
            var objList = unitsService.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {
            return View("AddUpdate", new SALES_MD_VM());
        }
    }
}
