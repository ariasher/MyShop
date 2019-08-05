using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.Exceptions;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;
        ProductCategoryRepository productCategories;
        public ProductManagerController()
        {
            context = new ProductRepository();
            productCategories = new ProductCategoryRepository();
        }


        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.GetProducts().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            ProductManagerViewModel viewModel = new ProductManagerViewModel()
            {
                Product = product,
                ProductCategories = productCategories.GetProductCategories()
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
                    ProductCategories = productCategories.GetProductCategories()
                };
                return View(viewModel);
            }
            catch (ProductNotFoundException exception)
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

               /* productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;*/

                context.Update(productToEdit);
                context.Commit();

                return RedirectToAction("Index");

            }
            catch (ProductNotFoundException exception)
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
            catch (ProductNotFoundException exception)
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
            catch (ProductNotFoundException exception)
            {
                return HttpNotFound();
            }
        }
    }
}