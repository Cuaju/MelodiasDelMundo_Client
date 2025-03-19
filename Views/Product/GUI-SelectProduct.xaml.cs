using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class GUI_SelectProduct : Window
    {
        private ProductsManagerClient _service;
        private List<ProductDataContract> _productos;
        private NotificationDialog _notificationDialog;

        public GUI_SelectProduct()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            CargarProductos();
            _notificationDialog = new NotificationDialog();
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
                GUI_EditProduct ventanaEdicion = new GUI_EditProduct(productoSeleccionado);
                ventanaEdicion.Show();
                this.Close();
            }
            else
            {
                _notificationDialog.ShowWarningNotification("Por favor seleccione un producto para editar.");
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ProductDataContract selectedProduct = dgProductos.SelectedItem as ProductDataContract;

            if (selectedProduct != null)
            {
                GUI_DeleteProduct deleteWindow = new GUI_DeleteProduct(selectedProduct);
                deleteWindow.Show();
                this.Close();
            }
            else
            {
                _notificationDialog.ShowWarningNotification("Por favor seleccione un producto para eliminar.");
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}