using Microsoft.Extensions.Configuration;
using ProductServiceFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace ProductServiceFile.BusinessLogic
{
    public class ProductBl
    {
        private IConfiguration _configuration;
        private static Dictionary<int, Product> _products;

        public ProductBl(IConfiguration configuration)
        {
            _configuration = configuration;
            _products = new Dictionary<int, Product>();
        }

        public async Task<List<Product>> GetProducts()
        {
            if(_products.Count == 0)
            {
                ReloadProducts();
            }

            if (_products.Count > 0)
            {
                return _products.Values.ToList();
            }
            return new List<Product>();
        }

        public async Task<Product> GetProductById(int id)
        {
            if(_products.Count == 0)
            {
                ReloadProducts();
            }
            if (_products.ContainsKey(id))
            {
                return _products[id];
            }
            return null;
        }

        private void ReloadProducts()
        {
            try
            {
                string url = _configuration["FtpUrl"];
                string user = _configuration["FtpUser"];
                string pw = _configuration["FtpPw"];

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@url);

                request.KeepAlive = true;
                request.UseBinary = true;

                request.Credentials = new NetworkCredential(@user, pw);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                _products = new Dictionary<int, Product>();

                using (Stream stream = request.GetResponse().GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    bool firstLine = true;
                    while (reader.Peek() != -1)
                    {
                        var line = reader.ReadLine();
                        if (firstLine)
                        {
                            firstLine = false;
                            continue;
                        }
                        var values = line.Split(';');

                        var p = new Product();
                        p.Id = int.Parse(values[0]);
                        p.Name = values[1];

                        if (!_products.ContainsKey(p.Id))
                        {
                            _products.Add(p.Id, p);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //TODO Add Logging
                throw;
            }
        }
    }
}
