using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> productContext;
        IRepository<ProductCategory> categoryContext;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> categoryContext)
        {
            this.productContext = productContext;
            this.categoryContext = categoryContext;
        }

        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<ProductCategory> categories = categoryContext.Collection().ToList();

            if(Category == null)
            {
                products = productContext
                    .Collection()
                    .ToList();
            }
            else
            {
                products = productContext
                    .Collection()
                    .Where(item => item.Category == Category)
                    .ToList();
            }

            ProductListViewModel viewModel = new ProductListViewModel
            {
                Product = products,
                ProductCategories = categories
            };

            return View(viewModel);
        }

        public ActionResult Details(string Id)
        {
            Product product = productContext.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}