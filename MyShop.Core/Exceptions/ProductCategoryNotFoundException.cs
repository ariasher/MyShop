using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Exceptions
{
    public class ProductCategoryNotFoundException : Exception
    {
        public ProductCategoryNotFoundException(string message) : base(message)
        {

        }
    }
}
