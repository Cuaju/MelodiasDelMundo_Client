using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace MelodiasDelMundo_Client.Views.Sale
{
    public partial class GUI_SalesManagement : Window
    {
        private SalesManagerClient _service;
        private List<SaleDataContract> _sales;
        private NotificationDialog _notificationDialog;

        public GUI_SalesManagement()
        {
            InitializeComponent();
            _service = new SalesManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private void BtSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(tbSaleId.Text, out int saleId);
                string customerName = tbCustomerName.Text.Trim();
                DateTime? date = dpSaleDate.SelectedDate;

                if (saleId > 0)
                {
                    var sale = _service.GetSaleById(saleId);
                    dgSales.ItemsSource = sale != null ? new List<SaleDataContract> { sale } : new List<SaleDataContract>();
                }
                else
                {
                    // Llama al nuevo método GetSales con filtros (pueden ir vacíos)
                    var results = _service.GetSales(customerName, date);
                    dgSales.ItemsSource = results;
                }
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error: " + ex.Message);
            }
        }


        private void BtRegister_Click(object sender, RoutedEventArgs e)
        {
            GUI_RegisterSale registerWindow = new GUI_RegisterSale();
            registerWindow.Show();
            this.Close();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            if (dgSales.SelectedItem is SaleDataContract selectedSale)
            {
                try
                {
                    if (selectedSale.IsCancelled)
                    {
                        _notificationDialog.ShowWarningNotification("La venta ya está cancelada.");
                        return;
                    }

                    bool result = _service.CancelSale(selectedSale.SaleId);
                    if (result)
                    {
                        _notificationDialog.ShowSuccessNotification("Venta cancelada correctamente.");
                        BtSearch_Click(null, null); // refrescar
                    }
                    else
                    {
                        _notificationDialog.ShowErrorNotification("No se pudo cancelar la venta.");
                    }
                }
                catch (Exception ex)
                {
                    _notificationDialog.ShowErrorNotification("Error al cancelar: " + ex.Message);
                }
            }
            else
            {
                _notificationDialog.ShowWarningNotification("Selecciona una venta para cancelar.");
            }
        }

        private void BtEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSales.SelectedItem is SaleDataContract selectedSale)
            {
                GUI_EditSale editWindow = new GUI_EditSale(selectedSale);
                editWindow.Show();
                this.Close();
            }
            else
            {
                _notificationDialog.ShowWarningNotification("Selecciona una venta para editar.");
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            MMenu menu = new MMenu();
            menu.Show();
            this.Close();
        }
    }
}
