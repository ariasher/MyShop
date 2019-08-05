using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Exceptions;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        ProductCategoryRepository context;

        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.GetProductCategories().ToList();

            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategory category = new ProductCategory();
            return View(category);
            //return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            context.Insert(productCategory);
            context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory category;
            try
            {
                category = context.Find(Id);
                return View(category);
            }
            catch (ProductCategoryNotFoundException exception)
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToEdit;
            try
            {
                productCategoryToEdit = context.Find(Id);

                if (!ModelState.IsValid)
                    return View(productCategory);

                context.Update(productCategoryToEdit);
                context.Commit();

                return RedirectToAction("Index");

            }
            catch (ProductCategoryNotFoundException exception)
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory category;
            try
            {
                category = context.Find(Id);
                return View(category);
            }
            catch (ProductCategoryNotFoundException exception)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete;

            try
            {
                productCategoryToDelete = context.Find(Id);
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
            catch (ProductCategoryNotFoundException exception)
            {
                return HttpNotFound();
            }
        }
    }
}