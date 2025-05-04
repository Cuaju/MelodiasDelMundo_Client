using MelodiasDelMundo_Client.Models;
using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MelodiasDelMundo_Client.Views.Sale
{
    public partial class GUI_EditSale : Window
    {
        private SaleDataContract _sale;
        private SalesManagerClient _service;
        private ProductsManagerClient _productsService;
        private NotificationDialog _notification;

        public ObservableCollection<Models.SaleDetailDataContract> SaleDetails { get; set; }
        public List<ProductDataContract> AvailableProducts { get; set; }

        public GUI_EditSale(SaleDataContract saleToEdit)
        {
            InitializeComponent();
            _sale = saleToEdit;
            _service = new SalesManagerClient();
            _productsService = new ProductsManagerClient();
            _notification = new NotificationDialog();

            LoadProducts();
            LoadSaleData();

            dgSaleDetails.CurrentCellChanged += DgSaleDetails_CurrentCellChanged;
        }

        private void LoadProducts()
        {
            try
            {
                AvailableProducts = _productsService.GetProducts().ToList();
                Models.SaleDetailDataContract.AvailableProducts = AvailableProducts;
            }
            catch (Exception ex)
            {
                _notification.ShowErrorNotification("Error al cargar productos: " + ex.Message);
            }
        }

        private void LoadSaleData()
        {
            tbCustomerName.Text = _sale.CustomerName;
            dpSaleDate.SelectedDate = _sale.SaleDate;

            SaleDetails = new ObservableCollection<Models.SaleDetailDataContract>(
                _sale.SaleDetails.Select(d => new Models.SaleDetailDataContract
                {
                    SaleDetailId = d.SaleDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                })
            );

            dgSaleDetails.ItemsSource = SaleDetails;
        }

        private void BtAddDetail_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableProducts.Count == 0)
            {
                _notification.ShowWarningNotification("No hay productos disponibles.");
                return;
            }

            var firstProduct = AvailableProducts.First();

            var detail = new Models.SaleDetailDataContract
            {
                ProductId = firstProduct.ProductId,
                ProductName = firstProduct.ProductName,
                UnitPrice = firstProduct.SalePrice,
                Quantity = 1
            };

            SaleDetails.Add(detail);
        }

        private void BtRemoveDetail_Click(object sender, RoutedEventArgs e)
        {
            if (dgSaleDetails.SelectedItem is Models.SaleDetailDataContract selected)
            {
                SaleDetails.Remove(selected);
            }
            else
            {
                _notification.ShowWarningNotification("Selecciona un detalle para eliminar.");
            }
        }

        private void dgSaleDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Se mantiene vacío porque la lógica está en el modelo
        }

        private void DgSaleDetails_CurrentCellChanged(object sender, EventArgs e)
        {
            dgSaleDetails.CommitEdit(DataGridEditingUnit.Row, true);
            dgSaleDetails.Items.Refresh();
        }

        private void BtSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _sale.CustomerName = tbCustomerName.Text.Trim();
                _sale.SaleDate = dpSaleDate.SelectedDate ?? DateTime.Now;

                _sale.SaleDetails = SaleDetails
                    .Select(d => new ServiceReference1.SaleDetailDataContract
                    {
                        SaleDetailId = d.SaleDetailId,
                        ProductId = d.ProductId,
                        ProductName = d.ProductName,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice
                    }).ToArray();

                bool success = _service.EditSale(_sale);

                if (success)
                {
                    _notification.ShowSuccessNotification("Venta editada exitosamente.");
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var window = new GUI_SalesManagement();
                            window.Show();
                            this.Close();
                        });
                    });
                }
                else
                {
                    _notification.ShowWarningNotification("No se pudo editar la venta.");
                }
            }
            catch (Exception ex)
            {
                _notification.ShowErrorNotification("Error inesperado: " + ex.Message);
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            var window = new GUI_SalesManagement();
            window.Show();
            this.Close();
        }
    }
}
