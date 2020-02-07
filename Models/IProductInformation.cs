using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoolApplicationMain.Models
{
    public interface IProductInformation
    {
        List<Product> GetProductsInformation();
    }
}
