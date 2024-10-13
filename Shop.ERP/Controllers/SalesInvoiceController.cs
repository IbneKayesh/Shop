using Shop.ERP.Models;
using Shop.ERP.Services;
using Shop.ERP.ViewModels;
using System.Text.Json;

namespace Shop.ERP.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private readonly SalesInvoiceService salesInvoiceService;
        public SalesInvoiceController(SalesInvoiceService _salesInvoiceService)
        {
            salesInvoiceService = _salesInvoiceService;
        }
        public IActionResult Index()
        {
            var objList = salesInvoiceService.GetAll();
            return View(objList);
        }

        public IActionResult Create(string id)
        {
            ViewData["IsEdit"] = id;
            return View("AddUpdate");
        }
        //POST: api/EditSalesInvoice
        [Route("api/EditSalesInvoice")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = salesInvoiceService.GetById(id);
                return Ok(entity);
            }
            else
            {
                TempData["msg"] = NotifyService.Error("Invalid ID, Parameter is required");
            }
            return RedirectToAction(nameof(Index));
        }

        //POST: api/SaveSalesInvoice
        [Route("api/SaveSalesInvoice")]
        [HttpPost]
        public IActionResult AddUpdate(SALES_MD_VM sales_md_vm)
        {
            if (sales_md_vm == null)
            {
                return BadRequest();
            }

            EQResult eQResult = new EQResult();
            //if (ModelState.IsValid)
            //{
            eQResult = salesInvoiceService.Save(sales_md_vm);
            if (eQResult.success)
            {
                sales_md_vm.SALES_MASTER.SALES_NO = eQResult.messages;
            }
            //else
            //{
            //    TempData["msg"] = eQResult.messages;
            //    return RedirectToAction(nameof(Index));
            //}
            //}
            return Ok(sales_md_vm);
        }
    }
}
