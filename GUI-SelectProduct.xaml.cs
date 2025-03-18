using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MelodiasDelMundo_Client.ServiceReference1;

namespace MelodiasDelMundo_Client
{
    public partial class GUI_SeleccionarProducto : Window
    {
        private ProductsManagerClient _service;
        private List<ProductDataContract> _productos;

        public GUI_SeleccionarProducto()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            CargarProductos();
        }

        private void CargarProductos()
        {
            _productos = _service.GetProducts().ToList();
            dgProductos.ItemsSource = _productos;
        }


        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            ProductDataContract productoSeleccionado = dgProductos.SelectedItem as ProductDataContract;

            if (productoSeleccionado != null)
            {
                GUI_EditarProducto ventanaEdicion = new GUI_EditarProducto(productoSeleccionado);
                ventanaEdicion.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un producto para editar.");
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
