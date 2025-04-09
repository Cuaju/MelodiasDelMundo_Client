using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_ValidateInventory : Window
    {
        private ProductsManagerClient _service;
        private List<ProductDataContract> _products;
        private NotificationDialog _notificationDialog;

        public GUI_ValidateInventory()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private void BtValidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedCategory = (cbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrWhiteSpace(selectedCategory) || selectedCategory == "Seleccionar")
                {
                    _notificationDialog.ShowWarningNotification("Por favor selecciona una categoría de producto para validar.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbPhysicalStock.Text) || !int.TryParse(tbPhysicalStock.Text, out int physicalStock))
                {
                    _notificationDialog.ShowWarningNotification("Por favor ingrese una cantidad válida de stock físico.");
                    return;
                }

                _products = _service.GetProducts().Where(p => p.Category == selectedCategory).ToList();
                dgProducts.ItemsSource = _products;

                if (_products.Count == 0)
                {
                    _notificationDialog.ShowWarningNotification("No se encontraron productos en la categoría seleccionada.");
                    return;
                }

                int totalSystemStock = _products.Sum(p => p.Stock);

                if (physicalStock == totalSystemStock)
                {
                    _notificationDialog.ShowSuccessNotification("El stock físico concuerda con el stock registrado en el sistema. No hubo discrepancias.");
                }
                else
                {
                    var result = MessageBox.Show("El stock físico NO concuerda con el stock registrado en el sistema. ¿Deseas solucionar ahora?",
                                                 "Discrepancia detectada",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        GUI_ProductManagement manageWindow = new GUI_ProductManagement();
                        manageWindow.Show();
                        this.Close();
                    }
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
            this.Close();
        }

        private void StockValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
