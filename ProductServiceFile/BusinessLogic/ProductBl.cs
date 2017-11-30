using System;
using System.Collections.Generic;

namespace ProductServiceFile.BusinessLogic
{
    public class ProductBl
    {
        private Dictionary<int, string> _products;

        public ProductBl()
        {
            _products = new Dictionary<int, string>();
        }

        public IEnumerable<string> Get()
        {
            return _products.Values;
        }

        public string Get(int id)
        {
            return _products[id] ?? null;
        }
    }
}
