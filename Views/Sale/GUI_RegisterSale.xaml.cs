using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MelodiasDelMundo_Client.Views.Sale
{
    public partial class GUI_RegisterSale : Window
    {
        private SalesManagerClient _salesService;
        private ProductsManagerClient _productsService;
        private NotificationDialog _notificationDialog;
        private List<SaleDetailDataContract> _saleDetails;
        private List<ProductDataContract> _products;

        public GUI_RegisterSale()
        {
            InitializeComponent();
            _salesService = new SalesManagerClient();
            _productsService = new ProductsManagerClient();
            _notificationDialog = new NotificationDialog();
            _saleDetails = new List<SaleDetailDataContract>();

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                _products = _productsService.GetProducts().ToList();
                cbProducts.ItemsSource = _products;
                cbProducts.DisplayMemberPath = "ProductName";
                cbProducts.SelectedValuePath = "ProductId";
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al cargar productos: " + ex.Message);
            }
        }

        private void BtAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (cbProducts.SelectedItem == null || string.IsNullOrWhiteSpace(tbQuantity.Text))
            {
                _notificationDialog.ShowWarningNotification("Selecciona un producto y especifica la cantidad.");
                return;
            }

            if (!int.TryParse(tbQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                _notificationDialog.ShowWarningNotification("Cantidad inválida.");
                return;
            }

            var selectedProduct = cbProducts.SelectedItem as ProductDataContract;
            if (selectedProduct == null) return;

            var existing = _saleDetails.FirstOrDefault(d => d.ProductId == selectedProduct.ProductId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _saleDetails.Add(new SaleDetailDataContract
                {
                    ProductId = selectedProduct.ProductId,
                    ProductName = selectedProduct.ProductName,
                    Quantity = quantity,
                    UnitPrice = selectedProduct.SalePrice
                });
            }

            dgSaleDetails.ItemsSource = null;
            dgSaleDetails.ItemsSource = _saleDetails;
            tbQuantity.Text = "";
            cbProducts.SelectedIndex = -1;
        }

        private async void BtRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCustomerName.Text))
            {
                _notificationDialog.ShowWarningNotification("Ingresa el nombre del cliente.");
                return;
            }

            if (_saleDetails.Count == 0)
            {
                _notificationDialog.ShowWarningNotification("Agrega al menos un producto a la venta.");
                return;
            }

            var sale = new SaleDataContract
            {
                CustomerName = tbCustomerName.Text.Trim(),
                SaleDate = DateTime.Now,
                IsCancelled = false,
                SaleDetails = _saleDetails.ToArray() // ✅ Conversión corregida
            };

            try
            {
                bool success = _salesService.RegisterSale(sale);
                if (success)
                {
                    _notificationDialog.ShowSuccessNotification("Venta registrada exitosamente.");
                    await Task.Delay(2000); // Espera 2 segundos

                    var management = new GUI_SalesManagement();
                    management.Show();
                    this.Close();


                }
                else
                {
                    _notificationDialog.ShowErrorNotification("Error al registrar la venta.");
                }

            }
            catch (FaultException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de servicio: " + ex.Message);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            GUI_SalesManagement salesManagementWindow = new GUI_SalesManagement();
            salesManagementWindow.Show();
            this.Close();
        }

    }
}
