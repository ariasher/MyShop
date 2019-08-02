using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;
using System.Runtime.Caching;
using MyShop.Core.Exceptions;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        /// <summary>
        /// Initializes the ProductRepository object to use caching.
        /// </summary>
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;

            if (products == null)
                products = new List<Product>();
        }

        /// <summary>
        /// Save products.
        /// </summary>
        public void Commit()
        {
            cache["products"] = products;
        }

        /// <summary>
        /// Add new product.
        /// </summary>
        /// <param name="product">Takes a Product object.</param>
        public void Insert(Product product)
        {
            products.Add(product);
        }

        /// <summary>
        /// Updates the product information. Throws ProductNotFoundException when the product ID is invalid.
        /// </summary>
        /// <param name="product">Product object with product information that needs to be updates.</param>
        public void Update(Product product)
        {
            Product productToUpdate = products.Find(prod => prod.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                string message = String.Format("The product {0} is invalid.", product.Name);
                throw new ProductNotFoundException(message);
            }
        }

        /// <summary>
        /// Finds the requested product with mentioned ID. 
        /// </summary>
        /// <param name="id">ID of the product to be found.</param>
        /// <returns>Returns the requested product or throws ProductNotFoundException when the product is not found.</returns>
        public Product Find(string id)
        {
            Product productToFind = products.Find(prod => prod.Id == id);

            if (productToFind != null)
            {
                return productToFind;
            }
            else
            {
                string message = String.Format("The requested product with id {0} is not found.", id);
                throw new ProductNotFoundException(message);
            }
        }
        
        /// <summary>
        /// Get all the products.
        /// </summary>
        /// <returns>Returns a list of products.</returns>
        public IQueryable<Product> GetProducts()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            Product productToDelete = products.Find(prod => prod.Id == id);
            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                string message = String.Format("The product with id {0} is invalid.", id);
                throw new ProductNotFoundException(message);
            }
        }

    }
}
