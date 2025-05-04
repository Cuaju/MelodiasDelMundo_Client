using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MelodiasDelMundo_Client.ServiceReference1;

namespace MelodiasDelMundo_Client.Models
{
    public class SaleDetailDataContract : INotifyPropertyChanged
    {
        public int SaleDetailId { get; set; }

        private int _productId;
        public int ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged(nameof(ProductId));
                    UpdateProductInfo(); // Actualiza nombre y precio si cambia el producto
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice != value)
                {
                    _unitPrice = value;
                    OnPropertyChanged(nameof(UnitPrice));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        public decimal Subtotal => UnitPrice * Quantity;


        public static List<ProductDataContract> AvailableProducts { get; set; }

        private void UpdateProductInfo()
        {
            var product = AvailableProducts?.FirstOrDefault(p => p.ProductId == ProductId);
            if (product != null)
            {
                ProductName = product.ProductName;
                UnitPrice = product.SalePrice;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
