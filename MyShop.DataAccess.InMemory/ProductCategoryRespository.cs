using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Exceptions;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        /// <summary>
        /// Initializes the ProductCategoryRepository object to use caching.
        /// </summary>
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;

            if (productCategories == null)
                productCategories = new List<ProductCategory>();
        }

        /// <summary>
        /// Save product categories.
        /// </summary>
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        /// <summary>
        /// Add new product category.
        /// </summary>
        /// <param name="category">Takes a Product object.</param>
        public void Insert(ProductCategory category)
        {
            productCategories.Add(category);
        }

        /// <summary>
        /// Updates the product category information. Throws ProductCategoryNotFoundException when the product category ID is invalid.
        /// </summary>
        /// <param name="category">Product category object with category information that needs to be updated.</param>
        public void Update(ProductCategory category)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(prodCategory => prodCategory.Id == prodCategory.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = category;
            }
            else
            {
                string message = String.Format("The product category {0} is invalid.", category.Category);
                throw new ProductCategoryNotFoundException(message);
            }
        }

        /// <summary>
        /// Finds the requested product category with mentioned ID. 
        /// </summary>
        /// <param name="id">ID of the product category to be found.</param>
        /// <returns>Returns the requested product category or throws ProductCategoryNotFoundException when the category is not found.</returns>
        public ProductCategory Find(string id)
        {
            ProductCategory productCategoryToFind = productCategories.Find(prodCategory => prodCategory.Id == id);

            if (productCategoryToFind != null)
            {
                return productCategoryToFind;
            }
            else
            {
                string message = String.Format("The requested product category with id {0} is not found.", id);
                throw new ProductCategoryNotFoundException(message);
            }
        }

        /// <summary>
        /// Get all the product categories.
        /// </summary>
        /// <returns>Returns a list of product categories.</returns>
        public IQueryable<ProductCategory> GetProductCategories()
        {
            return productCategories.AsQueryable();
        }

        /// <summary>
        /// Deleted the mentioned product category or throws ProductCategoryNotFoundException.
        /// </summary>
        /// <param name="id">Id of the category to be deleted.</param>
        public void Delete(string id)
        {
            ProductCategory productToDelete = productCategories.Find(category => category.Id == id);
            if (productToDelete != null)
            {
                productCategories.Remove(productToDelete);
            }
            else
            {
                string message = String.Format("The product category with id {0} is invalid.", id);
                throw new ProductCategoryNotFoundException(message);
            }
        }

    }
}
