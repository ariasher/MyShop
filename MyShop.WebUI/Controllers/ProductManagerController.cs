using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        InMemoryRepository<Product> context;
        InMemoryRepository<ProductCategory> productCategories;
        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCategories = new InMemoryRepository<ProductCategory>();
        }


        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            ProductManagerViewModel viewModel = new ProductManagerViewModel()
            {
                Product = product,
                ProductCategories = productCategories.Collection()
            };

            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }

            context.Insert(product);
            context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            Product product;
            try
            {
                product = context.Find(Id);
                ProductManagerViewModel viewModel = new ProductManagerViewModel()
                {
                    Product = product,
                    ProductCategories = productCategories.Collection()
                };
                return View(viewModel);
            }
            catch
            {
                return HttpNotFound();
            }
            
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit;
            try
            {
                productToEdit = context.Find(Id);
                
                if(!ModelState.IsValid)
                    return View(product);

                context.Update(productToEdit);
                context.Commit();

                return RedirectToAction("Index");

            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(string Id)
        {
            Product product;
            try
            {
                product = context.Find(Id);
                return View(product);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete;

            try
            {
                productToDelete = context.Find(Id);
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}