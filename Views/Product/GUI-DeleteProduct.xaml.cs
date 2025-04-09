using System;
using System.ServiceModel;
using System.Windows;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_DeleteProduct : Window
    {
        private ProductsManagerClient _service;
        private ProductDataContract _product;
        private NotificationDialog _notificationDialog;

        public GUI_DeleteProduct(ProductDataContract product)
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _product = product;
            _notificationDialog = new NotificationDialog();
            LoadProductData();
        }

        private void LoadProductData()
        {
            tbName.Text = _product.ProductName;
            tbCode.Text = _product.ProductCode;
            tbDescription.Text = _product.Description;
            tbPurchasePrice.Text = _product.PurchasePrice.ToString("C");
            tbSalePrice.Text = _product.SalePrice.ToString("C");
            tbCategory.Text = _product.Category;
            tbBrand.Text = _product.Brand;
            tbModel.Text = _product.Model;
            tbStock.Text = _product.Stock.ToString();

            if (!string.IsNullOrEmpty(_product.Photo))
            {
                imgPreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_product.Photo));
            }
        }

        private void BtDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (_product.HasSales)
                    {
                        _notificationDialog.ShowErrorNotification("Este producto no puede ser eliminado porque ya ha sido registrado en ventas.");
                        return;
                    }

                    bool success = _service.DeleteProduct(_product.ProductId);
                    if (success)
                    {
                        _notificationDialog.ShowSuccessNotification("El producto se ha eliminado correctamente.");
                        GUI_ProductManagement selectProductWindow = new GUI_ProductManagement();
                        selectProductWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        _notificationDialog.ShowErrorNotification("No se pudo eliminar el producto.");
                    }
                }
                catch (FaultException ex)
                {
                    _notificationDialog.ShowErrorNotification("Error: " + ex.Message);
                }
                catch (CommunicationException ex)
                {
                    _notificationDialog.ShowErrorNotification("Error de comunicación con el servidor: " + ex.Message);
                }
                catch (TimeoutException ex)
                {
                    _notificationDialog.ShowErrorNotification("Tiempo de espera agotado: " + ex.Message);
                }
                catch (Exception ex)
                {
                    _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
                }
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            GUI_ProductManagement vSelectProduct = new GUI_ProductManagement();
            vSelectProduct.Show();
            this.Close();
        }
    }
}
