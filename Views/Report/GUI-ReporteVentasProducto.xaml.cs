using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace MelodiasDelMundo_Client.Views.Report
{
    /// <summary>
    /// Lógica de interacción para GUI_ReporteVentasProducto.xaml
    /// </summary>
    public partial class GUI_ReporteVentasProducto : Window
    {
        private ProductsManagerClient _service;
        private NotificationDialog _notificationDialog;
        public GUI_ReporteVentasProducto()
        {
            InitializeComponent();
            _service = new ProductsManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private async void btGenerate_Click(object sender, RoutedEventArgs e)
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
                var reportData = await service.GetSalesByProductReportAsync(startDate.Value, endDate.Value);

                string fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string outputPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                var generator = new SalesByProductReportGenerator(reportData.ToList(), startDate.Value, endDate.Value);
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

        private void btBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
