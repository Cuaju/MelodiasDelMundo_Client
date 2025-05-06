using MelodiasDelMundo_Client.ServiceReference1;
using MelodiasDelMundo_Client.Utils;
using MelodiasDelMundo_Client.Views.MainMenu;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
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

namespace MelodiasDelMundo_Client.Views.Sale
{
    /// <summary>
    /// Lógica de interacción para ReportSales.xaml
    /// </summary>
    public partial class ReportSales : Window
    {
        private SalesManagerClient _service;
        private NotificationDialog _notificationDialog;

        public ReportSales()
        {
            InitializeComponent();
            _service = new SalesManagerClient();
            _notificationDialog = new NotificationDialog();
        }

        private async void BtGenerate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? start = dpStartDate.SelectedDate;
            DateTime? end = dpEndDate.SelectedDate;

            if (start == null || end == null || end < start)
            {
                _notificationDialog.ShowWarningNotification("Selecciona un rango de fechas válido.");
                return;
            }

            try
            {
                var report = await _service.GetEarningsReportAsync(start.Value, end.Value);

                string fileName = $"ReporteGanancias_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string outputPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                var generator = new EarningsReportPdfGenerator(report, start.Value, end.Value);
                generator.GeneratePdf(outputPath);

                _notificationDialog.ShowSuccessNotification($"Reporte generado correctamente en:\n{outputPath}");

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = outputPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                _notificationDialog.ShowErrorNotification("Error al generar el reporte: " + ex.Message);
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            new MMenu().Show();
            this.Close();
        }
    }

    public class EarningsReportPdfGenerator
    {
        private EarningsReport _report;
        private DateTime _start;
        private DateTime _end;

        public EarningsReportPdfGenerator(EarningsReport report, DateTime start, DateTime end)
        {
            _report = report;
            _start = start;
            _end = end;
        }

        public void GeneratePdf(string path)
        {
            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("📊 Reporte de Ganancias").FontSize(20).Bold().AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Periodo: {_start:dd/MM/yyyy} - {_end:dd/MM/yyyy}")
                        .FontSize(12).Italic().AlignCenter();  
                        col.Spacing(10);  




                        col.Item().LineHorizontal(1).LineColor("#888888");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1);
                                c.RelativeColumn(2);
                            });

                            void Row(string label, string value)
                            {
                                table.Cell().Element(CellLabelStyle).Text(label);
                                table.Cell().Element(CellValueStyle).Text(value);
                            }

                            Row("Ventas Totales", _report.TotalSales.ToString());
                            Row("Productos Vendidos", _report.TotalItemsSold.ToString());
                            Row("Ingresos Brutos", $"{_report.GrossEarnings:C}");
                            Row("Ganancia Neta", $"{_report.NetEarnings:C}");
                        });
                    });

                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            })
            .GeneratePdf(path);
        }

        private IContainer CellLabelStyle(IContainer container) =>
            container.Padding(5).Background("#f0f0f0").DefaultTextStyle(x => x.Bold());

        private IContainer CellValueStyle(IContainer container) =>
            container.Padding(5).Background("#ffffff");
    }

}
