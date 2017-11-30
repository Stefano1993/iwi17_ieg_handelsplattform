using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductServiceLocal.Models;

namespace ProductServiceLocal.BusinessLogic
{
    public class ProductBl
    {
        private static Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public async Task<List<Product>> GetProducts()
        {
            if (_products.Count > 0)
            {
                return _products.Values.ToList();
            }
            return new List<Product>();
        }

        public async Task<Product> GetProductById(int id)
        {
            if (_products.ContainsKey(id))
            {
                return _products[id];
            }
            return null;
        }

        public async Task<int> CreateProduct(Product product)
        {
            int id = 0;
            if (_products.Keys.Count > 0)
            {
                id = _products.Keys.Max();
            }
            id++;
            _products.Add(id, new Product
            {
                Id = id,
                Name = product.Name
            });
            return id;
        }

        public async Task<int?> UpdateProductById(Product product)
        {
            if (_products.ContainsKey(product.Id))
            {
                _products[product.Id] = product;
                return product.Id;
            }
            return null;
        }

        public async Task<int?> DeleteProductById(int id)
        {
            if (_products.ContainsKey(id))
            {
                _products.Remove(id);
                return id;
            }
            return null;
        }
    }
}
