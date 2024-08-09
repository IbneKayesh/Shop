using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ERP.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop.ERP.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db;
        public ProductController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            var entityList = from a in db.PRODUCTS
                             join b in db.PRODUCT_CATEGORY on a.CATEGORY_ID equals b.ID
                             select new PRODUCTS
                             {
                                 ID = a.ID,
                                 CATEGORY_ID = a.CATEGORY_ID,
                                 PRODUCT_NAME = a.PRODUCT_NAME,
                                 PRODUCT_RATE = a.PRODUCT_RATE,
                                 CATEGORY_NAME = b.CATEGORY_NAME
                             };
            var result = entityList.ToList();

            return View(result);
        }

        public IActionResult Create()
        {
            Dropdown_CreateEdit();
            PRODUCTS obj = new PRODUCTS();
            return View(obj);
        }

        [HttpPost]
        public IActionResult Create(PRODUCTS obj)
        {
            Dropdown_CreateEdit();
            if (ModelState.IsValid)
            {
                //DB Operation
                //new obj
                if (obj.ID == Guid.Empty.ToString())
                {
                    obj.ID = Guid.NewGuid().ToString();
                    db.PRODUCTS.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    var entity = db.PRODUCTS.Find(obj.ID);
                    if (entity != null)
                    {
                        entity.CATEGORY_ID = obj.CATEGORY_ID;
                        entity.PRODUCT_NAME = obj.PRODUCT_NAME;
                        entity.PRODUCT_RATE = obj.PRODUCT_RATE;
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        //Not found
                    }
                }
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(string id)
        {
            Dropdown_CreateEdit();
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = db.PRODUCTS.Find(id);
                if (entity != null)
                {
                    return View("Create", entity);
                }
                else
                {
                    //not found
                }
            }
            else
            {
                //invalid request
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = db.PRODUCTS.Find(id);
                if (entity != null)
                {
                    db.PRODUCTS.Remove(entity);
                    db.SaveChanges();
                }
                else
                {
                    //not found
                }
            }
            else
            {
                //invalid request
            }
            return RedirectToAction(nameof(Index));
        }

        private void Dropdown_CreateEdit()
        {
            ViewBag.CATEGORY_ID = new SelectList(db.PRODUCT_CATEGORY.ToList(), "ID", "CATEGORY_NAME");
        }
    }
}