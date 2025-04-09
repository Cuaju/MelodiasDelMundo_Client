using MaterialDesignThemes.Wpf;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_ProductManagement : Window
    {
        private ProductsManagerClient _service;
        private List<ProductDataContract> _products;
        private NotificationDialog _notificationDialog;

        public GUI_ProductManagement()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            LoadProducts();
            _notificationDialog = new NotificationDialog();
        }

        private void LoadProducts()
        {
            try
            {
                _products = _service.GetProducts().ToList();
                dgProducts.ItemsSource = _products;
            }
            catch (FaultException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de servicio: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de comunicación con el servidor: " + ex.Message);
            }
            catch (TimeoutException ex)
            {
                _notificationDialog.ShowErrorNotification("Tiempo de espera agotado al intentar conectar con el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }

        private void BtEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductDataContract selectedProduct = dgProducts.SelectedItem as ProductDataContract;

                if (selectedProduct != null)
                {
                    GUI_EditProduct editProductWindow = new GUI_EditProduct(selectedProduct);
                    editProductWindow.Show();
                    this.Close();
                }
                else
                {
                    _notificationDialog.ShowWarningNotification("Por favor seleccione un producto para editar.");
                }
            }
            catch (FaultException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de servicio: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de comunicación con el servidor: " + ex.Message);
            }
            catch (TimeoutException ex)
            {
                _notificationDialog.ShowErrorNotification("Tiempo de espera agotado al intentar conectar con el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductDataContract selectedProduct = dgProducts.SelectedItem as ProductDataContract;

                if (selectedProduct != null)
                {
                    GUI_DeleteProduct deleteProductWindow = new GUI_DeleteProduct(selectedProduct);
                    deleteProductWindow.Show();
                    this.Close();
                }
                else
                {
                    _notificationDialog.ShowWarningNotification("Por favor seleccione un producto para eliminar.");
                }
            }
            catch (FaultException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de servicio: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                _notificationDialog.ShowErrorNotification("Error de comunicación con el servidor: " + ex.Message);
            }
            catch (TimeoutException ex)
            {
                _notificationDialog.ShowErrorNotification("Tiempo de espera agotado al intentar conectar con el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            MMenu menuWindow = new MMenu();
            menuWindow.Show();
            this.Close();
        }

        private void BtRegister_Click(object sender, RoutedEventArgs e)
        {
            GUI_RegisterProduct registerProductWindow = new GUI_RegisterProduct();
            registerProductWindow.Show();
            this.Close();
        }

        private void BtSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = tbSearch.Text.Trim();
                string code = tbCode.Text.Trim();
                string category = ((ComboBoxItem)cbCategory.SelectedItem).Content.ToString();
                string brand = ((ComboBoxItem)cbBrand.SelectedItem).Content.ToString();

                if (category == "Seleccionar") category = "";
                if (brand == "Seleccionar") brand = "";

                if (string.IsNullOrWhiteSpace(name) &&
                    string.IsNullOrWhiteSpace(code) &&
                    string.IsNullOrWhiteSpace(category) &&
                    string.IsNullOrWhiteSpace(brand))
                {
                    _notificationDialog.ShowWarningNotification("Por favor especifique al menos un criterio de búsqueda.");
                    return;
                }

                var filteredProducts = _products.Where(p =>
                    (string.IsNullOrWhiteSpace(name) || p.ProductName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (string.IsNullOrWhiteSpace(code) || p.ProductCode.IndexOf(code, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (string.IsNullOrWhiteSpace(category) || p.Category == category) &&
                    (string.IsNullOrWhiteSpace(brand) || p.Brand == brand)).ToList();

                if (filteredProducts.Count == 0)
                {
                    _notificationDialog.ShowWarningNotification("No existe un producto con esas especificaciones.");
                    return;
                }

                dgProducts.ItemsSource = filteredProducts;
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }
    }
}