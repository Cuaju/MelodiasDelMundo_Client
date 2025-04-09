using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_RegisterProduct : Window
    {
        private ProductsManagerClient _service;
        private string _imagePath;
        private NotificationDialog _notificationDialog;

        public GUI_RegisterProduct()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _imagePath = "";
            _notificationDialog = new NotificationDialog();
        }

        private void BtRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbName.Text) ||
                    string.IsNullOrWhiteSpace(tbPurchasePrice.Text) ||
                    string.IsNullOrWhiteSpace(tbSalePrice.Text) ||
                    string.IsNullOrWhiteSpace(tbStock.Text) ||
                    cbCategory.SelectedIndex == 0 ||
                    cbBrand.SelectedIndex == 0 ||
                    string.IsNullOrWhiteSpace(_imagePath) ||
                    string.IsNullOrWhiteSpace(tbModel.Text) ||
                    string.IsNullOrWhiteSpace(tbDescription.Text))
                {
                    _notificationDialog.ShowWarningNotification("Por favor llene todos los campos.");
                    return;
                }

                bool nameExists = _service.ExistsProductByName(tbName.Text, 0);

                if (nameExists)
                {
                    _notificationDialog.ShowErrorNotification("El nombre del producto ya está registrado en la base de datos.");
                    return;
                }

                var product = new ProductDataContract
                {
                    ProductName = tbName.Text,
                    Description = tbDescription.Text,
                    PurchasePrice = decimal.Parse(tbPurchasePrice.Text, NumberStyles.Currency),
                    SalePrice = decimal.Parse(tbSalePrice.Text, NumberStyles.Currency),
                    Category = ((ComboBoxItem)cbCategory.SelectedItem).Content.ToString(),
                    Brand = ((ComboBoxItem)cbBrand.SelectedItem).Content.ToString(),
                    Model = tbModel.Text,
                    Stock = int.Parse(tbStock.Text),
                    Photo = _imagePath,
                    Status = true,
                    HasSales = false
                };

                bool result = _service.RegisterProduct(product);

                if (result)
                {
                    _notificationDialog.ShowSuccessNotification("El producto se ha registrado correctamente.");
                    ClearFields();
                }
                else
                {
                    _notificationDialog.ShowErrorNotification("No fue posible registrar el producto porque ya existe en la base de datos.");
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

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            GUI_ProductManagement productManagementWindow = new GUI_ProductManagement();
            productManagementWindow.Show();
            this.Close();
        }

        private void BtSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(_imagePath));
            }
        }

        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            string unformattedText = textBox.Text.Replace("$", "").Replace(",", "").Trim();

            if (decimal.TryParse(unformattedText, out decimal valor))
            {
                textBox.Text = valor.ToString("C2", CultureInfo.CurrentCulture);
                textBox.CaretIndex = textBox.Text.Length;
            }
            else if (string.IsNullOrWhiteSpace(unformattedText))
            {
                textBox.Text = "";
            }
        }

        private void StockValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void ClearFields()
        {
            tbName.Text = "";
            tbDescription.Text = "";
            tbPurchasePrice.Text = "";
            tbSalePrice.Text = "";
            tbStock.Text = "";
            tbModel.Text = "";
            cbCategory.SelectedIndex = 0;
            cbBrand.SelectedIndex = 0;
            imgPreview.Source = null;
            _imagePath = "";
        }
    }
}