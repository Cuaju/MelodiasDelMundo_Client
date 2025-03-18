using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using MelodiasDelMundo_Client.ServiceReference1;
using System.ServiceModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace MelodiasDelMundo_Client
{
    public partial class GUI_EditarProducto : Window
    {
        private ProductsManagerClient _service;
        private ProductDataContract _producto;
        private string rutaImagen = "";

        public GUI_EditarProducto(ProductDataContract producto)
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _producto = producto;
            CargarDatos();
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

            // Buscar el índice del ComboBoxItem que coincida con la marca recuperada
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
                _producto.ProductName = txtNombre.Text;
                _producto.Description = txtDescripcion.Text;
                _producto.PurchasePrice = decimal.Parse(txtPrecioCompra.Text, System.Globalization.NumberStyles.Currency);
                _producto.SalePrice = decimal.Parse(txtPrecioVenta.Text, System.Globalization.NumberStyles.Currency);
                _producto.Stock = int.Parse(txtCantidad.Text);
                _producto.Category = ((ComboBoxItem)cmbCategoria.SelectedItem).Content.ToString();
                _producto.Brand = ((ComboBoxItem)cmbMarca.SelectedItem).Content.ToString();
                _producto.Model = txtModelo.Text;
                _producto.Photo = rutaImagen;

                _service.EditProduct(_producto);
                MessageBox.Show("Producto actualizado correctamente.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
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
