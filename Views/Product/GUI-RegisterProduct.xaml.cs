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

namespace MelodiasDelMundo_Client.Views.Product
{
    public partial class GUI_RegisterProduct : Window
    {
        private ProductsManagerClient _service;
        private string rutaImagen = "";
        private NotificationDialog _notificationDialog;

        public GUI_RegisterProduct()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
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

                bool nombreExiste = _service.ExistsProductByName(txtNombre.Text, 0);

                if (nombreExiste)
                {
                    _notificationDialog.ShowErrorNotification("El nombre del producto ya está registrado en la base de datos.");
                    return;
                }

                var producto = new ProductDataContract
                {
                    ProductName = txtNombre.Text,
                    Description = txtDescripcion.Text,
                    PurchasePrice = decimal.Parse(txtPrecioCompra.Text, NumberStyles.Currency),
                    SalePrice = decimal.Parse(txtPrecioVenta.Text, NumberStyles.Currency),
                    Category = ((ComboBoxItem)cmbCategoria.SelectedItem).Content.ToString(),
                    Brand = ((ComboBoxItem)cmbMarca.SelectedItem).Content.ToString(),
                    Model = txtModelo.Text,
                    Stock = int.Parse(txtCantidad.Text),
                    Photo = rutaImagen,
                    Status = true,
                    HasSales = false
                };

                bool resultado = _service.RegisterProduct(producto);

                if (resultado)
                {
                    _notificationDialog.ShowSuccessNotification("El producto se ha registrado correctamente.");
                    LimpiarCampos();
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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SeleccionarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                rutaImagen = openFileDialog.FileName;
                imgVistaPrevia.Source = new BitmapImage(new Uri(rutaImagen));
            }
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

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtCantidad.Text = "";
            txtModelo.Text = "";
            cmbCategoria.SelectedIndex = 0;
            cmbMarca.SelectedIndex = 0;
            imgVistaPrevia.Source = null;
            rutaImagen = "";
        }
    }
}