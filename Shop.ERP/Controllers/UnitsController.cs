using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ERP.Models;
using Shop.ERP.Services;

namespace Shop.ERP.Controllers
{
    public class UnitsController : Controller
    {
        private readonly UnitsService unitsService;
        public UnitsController(UnitsService _unitsService)
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
            return View("AddUpdate", new UNITS());
        }
        public IActionResult Edit(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = unitsService.GetById(id);
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
        public IActionResult AddUpdate(UNITS obj)
        {
            EQResult eQResult = new EQResult();
            if (ModelState.IsValid)
            {
                eQResult = unitsService.Save(obj);
                TempData["msg"] = eQResult.messages;
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResult eQResult = unitsService.Delete(id);
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
