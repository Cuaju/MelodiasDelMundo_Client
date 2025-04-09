using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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
    public partial class GUI_EditProduct : Window
    {
        private ProductsManagerClient _service;
        private ProductDataContract _product;
        private string _imagePath;
        private NotificationDialog _notificationDialog;

        public GUI_EditProduct(ProductDataContract product)
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _product = product;
            _imagePath = "";
            LoadData();
            _notificationDialog = new NotificationDialog();
        }

        private void LoadData()
        {
            tbName.Text = _product.ProductName;
            tbCode.Text = _product.ProductCode;
            tbDescription.Text = _product.Description;
            tbPurchasePrice.Text = _product.PurchasePrice.ToString("C");
            tbSalePrice.Text = _product.SalePrice.ToString("C");
            tbStock.Text = _product.Stock.ToString();

            for (int i = 0; i < cbCategory.Items.Count; i++)
            {
                if (((ComboBoxItem)cbCategory.Items[i]).Content.ToString() == _product.Category)
                {
                    cbCategory.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cbBrand.Items.Count; i++)
            {
                if (((ComboBoxItem)cbBrand.Items[i]).Content.ToString() == _product.Brand)
                {
                    cbBrand.SelectedIndex = i;
                    break;
                }
            }

            tbModel.Text = _product.Model;
            _imagePath = _product.Photo;

            if (!string.IsNullOrEmpty(_imagePath))
            {
                imgPreview.Source = new BitmapImage(new Uri(_imagePath));
            }
        }

        private void BtSaveChanges_Click(object sender, RoutedEventArgs e)
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

                bool nameExists = _service.ExistsProductByName(tbName.Text, _product.ProductId);

                if (nameExists)
                {
                    _notificationDialog.ShowErrorNotification("El nombre del producto ya está registrado en la base de datos.");
                    return;
                }

                _product.ProductName = tbName.Text;
                _product.Description = tbDescription.Text;
                _product.PurchasePrice = decimal.Parse(tbPurchasePrice.Text, System.Globalization.NumberStyles.Currency);
                _product.SalePrice = decimal.Parse(tbSalePrice.Text, System.Globalization.NumberStyles.Currency);
                _product.Stock = int.Parse(tbStock.Text);
                _product.Category = ((ComboBoxItem)cbCategory.SelectedItem).Content.ToString();
                _product.Brand = ((ComboBoxItem)cbBrand.SelectedItem).Content.ToString();
                _product.Model = tbModel.Text;
                _product.Photo = _imagePath;
                
                bool resultado = _service.EditProduct(_product);

                if (resultado)
                {
                    _notificationDialog.ShowSuccessNotification("La información del pruducto se modificó correctamente.");
                    GUI_ProductManagement selectProductWindow = new GUI_ProductManagement();
                    selectProductWindow.Show();
                    this.Close();
                }
                else
                {
                    _notificationDialog.ShowErrorNotification("No fue posible modificar el producto porque ya existe en la base de datos.");
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
            GUI_ProductManagement vSelectProduct = new GUI_ProductManagement();
            vSelectProduct.Show();
            this.Close();
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

        private void BtSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(_imagePath));
            }
        }
    }
}