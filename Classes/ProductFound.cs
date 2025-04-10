using MelodiasDelMundo_Client.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasDelMundo_Client.Classes
{
    public class ProductFound
    {
    
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Stock {  get; set; }
        public string Photo {  get; set; }
        public bool Status {  get; set; }
        public bool HasSales { get; set; }

        public ProductFound() { }
        public ProductFound(int productId, string productName, string productCode, string description, decimal purchasePrice, decimal salePrice, string category,string brand, string model, int stock, string photo, bool status, bool hasSales)
        {
            ProductID = productId;
            ProductName = productName;
            ProductCode = productCode;
            Description = description;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            Category = category;
            Brand = brand;
            Model = model;
            Stock = stock;
            Photo = photo;
            Status = status;
            HasSales = hasSales;
        }

        public ProductFound(ProductDataContract productData)
        {
            ProductID = productData.ProductId;
            ProductName = productData.ProductName;
            ProductCode = productData.ProductCode;
            Description = productData.Description;
            PurchasePrice = productData.PurchasePrice;
            SalePrice = productData.SalePrice;
            Category = productData.Category;
            Brand = productData.Brand;
            Model = productData.Model;
            Stock = productData.Stock;
            Photo = productData.Photo;
            Status = productData.Status;
            HasSales = productData.HasSales;
        }

    }
}
