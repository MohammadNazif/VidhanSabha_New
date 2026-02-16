using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace VishanSabha.Models
{
    public class ReportGenerator<T>
    {
        public byte[] ExportToPdf(List<T> data, string[] headers, string title, Func<T, List<string>> mapFunc)
        {
            if (data == null || data.Count == 0)
                throw new Exception("No data available to generate PDF.");

            if (headers == null || headers.Length == 0)
                throw new Exception("Headers must be provided.");

            if (mapFunc == null)
                throw new Exception("Mapping function must be provided.");

            IContainer CellStyle(IContainer container) =>
                container.Padding(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(new PageSize(2000, PageSizes.A4.Height)); 

                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(14));

                      page.Header()
                       .Element(headerContainer =>
                                     headerContainer
                               .Padding(10)
                               .Text(title)
                               .AlignCenter()
                               .SemiBold()
                               .FontSize(40)
                               .FontColor(Colors.Orange.Darken2)
                               );


                    page.Content().Border(1).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < headers.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        columns.ConstantColumn(60); // S.N.
                                        break;                                 
                                    case 1:
                                        columns.ConstantColumn(170);  // Panna Pramukh (e.g. name)
                                        break;
                                    
                                    default:
                                        columns.RelativeColumn(1); // All others
                                        break;
                                }
                            }
                        });

                        // Header row
                        table.Header(header =>
                        {
                            foreach (var h in headers)
                            {
                                header.Cell().Border(1)
                                    .Padding(10)
                                    .Text(h)
                                    .FontSize(14)
                                    .Bold();

                            }
                        });

                        // Data rows
                        foreach (var item in data)
                        {
                            var cells = mapFunc(item);

                            if (cells == null || cells.Count != headers.Length)
                                throw new Exception("Mapping function returned invalid number of cells.");

                            for (int i = 0; i < headers.Length; i++)
                            {
                                table.Cell().Border(1).Padding(10).Element(CellStyle).Text(cells[i] ?? "");
                            }
                        }
                    });
                });
            });

            try
            {
                var stream = new MemoryStream();
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("PDF generation failed: " + ex.Message, ex);
            }
        }

        public byte[] ExportToExcel(List<T> data, string[] headers, string title, Func<T, List<string>> mapFunc)
        {
            if (data == null || data.Count == 0)
                throw new Exception("No data available to generate Excel.");

            if (headers == null || headers.Length == 0)
                throw new Exception("Headers must be provided.");

            if (mapFunc == null)
                throw new Exception("Mapping function must be provided.");

            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(title ?? "Sheet1");

            // Add Title in first row, merge cells
            worksheet.Cells[1, 1, 1, headers.Length].Merge = true;
            worksheet.Cells[1, 1].Value = title;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Add Headers in second row
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[2, i + 1].Value = headers[i];
                worksheet.Cells[2, i + 1].Style.Font.Bold = true;
                worksheet.Cells[2, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[2, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[2, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Column(i + 1).AutoFit();
            }

            // Add Data rows starting from row 3
            int rowIndex = 3;
            foreach (var item in data)
            {
                var cells = mapFunc(item);

                if (cells == null || cells.Count != headers.Length)
                    throw new Exception("Mapping function returned invalid number of cells.");

                for (int col = 0; col < headers.Length; col++)
                {
                    worksheet.Cells[rowIndex, col + 1].Value = cells[col];
                    worksheet.Cells[rowIndex, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                rowIndex++;
            }

            // AutoFit columns after all data is added
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Column(i + 1).AutoFit();
            }

            return package.GetAsByteArray();
        }




    }
}