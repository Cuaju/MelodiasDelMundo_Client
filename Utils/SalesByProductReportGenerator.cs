using MelodiasDelMundo_Client.ServiceReference1;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasDelMundo_Client.Utils
{
    public class SalesByProductReportGenerator
    {
        private readonly List<SalesByProductReport> _reportData;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public SalesByProductReportGenerator(List<SalesByProductReport> reportData, DateTime startDate, DateTime endDate)
        {
            _reportData = reportData;
            _startDate = startDate;
            _endDate = endDate;
        }

        public void GeneratePdf(string outputPath)
        {
            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Reporte de Ventas por Producto")
                        .FontSize(20)
                        .SemiBold()
                        .AlignCenter();

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item()
                              .Text($"Periodo: {_startDate:dd/MM/yyyy} - {_endDate:dd/MM/yyyy}")
                              .FontSize(12);

                        column.Item().Table(table =>
                        {

                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3); 
                                cols.RelativeColumn(1); 
                                cols.RelativeColumn(1); 
                                cols.RelativeColumn(1); 
                                cols.RelativeColumn(1); 
                            });

                            // header row
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Producto");
                                header.Cell().Element(CellStyle).Text("Cant. Vendida");
                                header.Cell().Element(CellStyle).Text("N° Ventas");
                                header.Cell().Element(CellStyle).Text("Precio Unitario");
                                header.Cell().Element(CellStyle).Text("Total $");
                            });

                            // data rows
                            foreach (var item in _reportData)
                            {
                                table.Cell().Element(CellData).Text(item.ProductName);
                                table.Cell().Element(CellData).Text(item.QuantitySold.ToString());
                                table.Cell().Element(CellData).Text(item.SalesCount.ToString());
                                table.Cell().Element(CellData).Text($"{item.UnitPrice:C}");
                                table.Cell().Element(CellData).Text($"{item.TotalRevenue:C}");
                            }
                        });
                    });

                    // FOOTER
                    page.Footer()
                        .AlignCenter()
                        .Text(txt => txt
                            .Span($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                        );
                });
            })
            .GeneratePdf(outputPath);
        }

        // estilos
        private IContainer CellStyle(IContainer c) =>
            c.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background("#eeeeee");

        private IContainer CellData(IContainer c) =>
            c.Padding(5);
    }

}
