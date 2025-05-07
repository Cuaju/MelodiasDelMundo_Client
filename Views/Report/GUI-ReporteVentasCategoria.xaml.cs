using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.MainMenu;
using MelodiasDelMundo_Client.Views.MainMenu.Menus;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace MelodiasDelMundo_Client.Views.Report
{
    public partial class GUI_ReporteVentasCategoria : Window
    {
        private ProductsManagerClient _service;
        private NotificationDialog _notificationDialog;

        public GUI_ReporteVentasCategoria()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private async void BtGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? startDate = dpStartDate.SelectedDate;
                DateTime? endDate = dpEndDate.SelectedDate;

                if (startDate == null || endDate == null)
                {
                    _notificationDialog.ShowWarningNotification("Por favor selecciona las fechas para saber el periodo de tiempo que tendrá el reporte.");
                    return;
                }

                var service = new ProductsManagerClient();
                var reportData = await service.GetSalesByCategoryReportAsync(startDate.Value, endDate.Value);

                string fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string outputPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                var generator = new SalesByCategoryReportGenerator(reportData.ToList(), startDate.Value, endDate.Value);
                generator.GeneratePdf(outputPath);

                _notificationDialog.ShowSuccessNotification($"El reporte se ha generado correctamente y se ha guardado en: {outputPath}");
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
            GUI_ReportesMenu reportesMenu = new GUI_ReportesMenu();
            reportesMenu.Show();
            this.Close();
        }
    }
}