using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;
using MyShop.Core.Contracts;
using System.IO;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }

            if(file != null)
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
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
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit;
            try
            {
                productToEdit = context.Find(Id);
                
                if(!ModelState.IsValid)
                    return View(product);

                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

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