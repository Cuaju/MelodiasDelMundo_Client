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

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_EditProduct : Window
    {
        private ProductsManagerClient _service;
        private ProductDataContract _producto;
        private string rutaImagen = "";
        private NotificationDialog _notificationDialog;

        public GUI_EditProduct(ProductDataContract producto)
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _producto = producto;
            CargarDatos();
            _notificationDialog = new NotificationDialog();
        }

        private void CargarDatos()
        {
            txtNombre.Text = _producto.ProductName;
            txtCodigo.Text = _producto.ProductCode;
            txtDescripcion.Text = _producto.Description;
            txtPrecioCompra.Text = _producto.PurchasePrice.ToString("C");
            txtPrecioVenta.Text = _producto.SalePrice.ToString("C");
            txtCantidad.Text = _producto.Stock.ToString();

            for (int i = 0; i < cmbCategoria.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbCategoria.Items[i]).Content.ToString() == _producto.Category)
                {
                    cmbCategoria.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbMarca.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbMarca.Items[i]).Content.ToString() == _producto.Brand)
                {
                    cmbMarca.SelectedIndex = i;
                    break;
                }
            }

            txtModelo.Text = _producto.Model;
            rutaImagen = _producto.Photo;

            if (!string.IsNullOrEmpty(rutaImagen))
            {
                imgVistaPrevia.Source = new BitmapImage(new Uri(rutaImagen));
            }
        }

        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioCompra.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioVenta.Text) ||
                    string.IsNullOrWhiteSpace(txtCantidad.Text) ||
                    cmbCategoria.SelectedIndex == 0 ||
                    cmbMarca.SelectedIndex == 0 ||
                    string.IsNullOrWhiteSpace(rutaImagen) ||
                    string.IsNullOrWhiteSpace(txtModelo.Text) ||
                    string.IsNullOrWhiteSpace(txtDescripcion.Text))
                 {
                    _notificationDialog.ShowWarningNotification("Por favor llene todos los campos.");
                    return;
                 }

                bool nombreExiste = _service.ExistsProductByName(txtNombre.Text, _producto.ProductId);

                if (nombreExiste)
                {
                    _notificationDialog.ShowErrorNotification("El nombre del producto ya está registrado en la base de datos.");
                    return;
                }

                _producto.ProductName = txtNombre.Text;
                _producto.Description = txtDescripcion.Text;
                _producto.PurchasePrice = decimal.Parse(txtPrecioCompra.Text, System.Globalization.NumberStyles.Currency);
                _producto.SalePrice = decimal.Parse(txtPrecioVenta.Text, System.Globalization.NumberStyles.Currency);
                _producto.Stock = int.Parse(txtCantidad.Text);
                _producto.Category = ((ComboBoxItem)cmbCategoria.SelectedItem).Content.ToString();
                _producto.Brand = ((ComboBoxItem)cmbMarca.SelectedItem).Content.ToString();
                _producto.Model = txtModelo.Text;
                _producto.Photo = rutaImagen;
                
                bool resultado = _service.EditProduct(_producto);

                if (resultado)
                {
                    _notificationDialog.ShowSuccessNotification("La información del pruducto se modificó correctamente.");
                    GUI_SelectProduct vSelectProduct = new GUI_SelectProduct();
                    vSelectProduct.Show();
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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            GUI_SelectProduct vSelectProduct = new GUI_SelectProduct();
            vSelectProduct.Show();
            this.Close();
        }

        private void PrecioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            string textoSinFormato = textBox.Text.Replace("$", "").Replace(",", "").Trim();

            if (decimal.TryParse(textoSinFormato, out decimal valor))
            {
                textBox.Text = valor.ToString("C2", CultureInfo.CurrentCulture);
                textBox.CaretIndex = textBox.Text.Length;
            }
            else if (string.IsNullOrWhiteSpace(textoSinFormato))
            {
                textBox.Text = "";
            }
        }

        private void StockValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void SeleccionarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                rutaImagen = openFileDialog.FileName;
                imgVistaPrevia.Source = new BitmapImage(new Uri(rutaImagen));
            }
        }
    }
}
