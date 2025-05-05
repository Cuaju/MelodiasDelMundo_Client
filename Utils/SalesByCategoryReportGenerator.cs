using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using MelodiasDelMundo_Client.ServiceReference1;

public class SalesByCategoryReportGenerator
{
    private List<SalesByCategoryReport> _reportData;
    private DateTime _startDate;
    private DateTime _endDate;

    public SalesByCategoryReportGenerator(List<SalesByCategoryReport> reportData, DateTime startDate, DateTime endDate)
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
                page.Header().Text($"Reporte de Ventas por Categoría")
                    .FontSize(20)
                    .SemiBold().AlignCenter();

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text($"Periodo: {_startDate:dd/MM/yyyy} - {_endDate:dd/MM/yyyy}")
                        .FontSize(12);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); 
                            columns.RelativeColumn(1); 
                            columns.RelativeColumn(1); 
                            columns.RelativeColumn(1); 
                            columns.RelativeColumn(1); 
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(c => CellStyle(c)).Text("Categoría");
                            header.Cell().Element(c => CellStyle(c)).Text("Cant. Vendida");
                            header.Cell().Element(c => CellStyle(c)).Text("N° Ventas");
                            header.Cell().Element(c => CellStyle(c)).Text("Total $");
                            header.Cell().Element(c => CellStyle(c)).Text("% Total");
                            header.Cell().Element(c => CellStyle(c)).Text("Producto + Vendido");
                        });

                        foreach (var item in _reportData)
                        {
                            table.Cell().Element(c => CellData(c)).Text(item.Category);
                            table.Cell().Element(c => CellData(c)).Text(item.QuantitySold.ToString());
                            table.Cell().Element(c => CellData(c)).Text(item.SalesCount.ToString());
                            table.Cell().Element(c => CellData(c)).Text($"{item.TotalRevenue:C}");
                            table.Cell().Element(c => CellData(c)).Text($"{item.Percentage:F2}%");
                            table.Cell().Element(c => CellData(c)).Text(item.TopProduct);
                        }
                    });
                });

                page.Footer()
                    .AlignCenter()
                    .Text(txt =>
                    {
                        txt.Span($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
            });
        })
        .GeneratePdf(outputPath);
    }

    private IContainer CellStyle(IContainer cont)
    {
        return cont.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background("#eeeeee");
    }

    private IContainer CellData(IContainer cont)
    {
        return cont.Padding(5);
    }
}