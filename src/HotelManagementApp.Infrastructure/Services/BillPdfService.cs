using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HotelManagementApp.Infrastructure.Services;

public class BillPdfService(IOrderBillProductRepository billProductRepository) : IBillDocumentService
{
    public async Task<byte[]> GenerateBillDocument(Order order, CancellationToken ct)
    {
        if (order.Status != OrderStatusEnum.Completed)
            throw new InvalidOperationException($"Bill can only be acquired for completed orders. Current status for id {order.Id} {order.Status}");

        var billProducts = await billProductRepository.GetOrderBillProductsByOrderId(order.Id, ct);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Row(row =>
                {
                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().Text("FIRMA HOTELARSKA").Bold().FontSize(16);
                        col.Item().Text("Adres: Szkolna 17, Białystok");
                        col.Item().Text("NIP: 12345678");
                    });

                    row.ConstantColumn(200).AlignRight().Column(col =>
                    {
                        col.Item().Text("Faktura VAT").FontSize(18).Bold();
                        col.Item().Text($"Zamówienie nr: {order.Id}");
                        col.Item().Text($"Data: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    });
                });

                page.Content().Element(container =>
                {
                    container.Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text("Dane klienta:").Bold();
                        col.Item().Text($"Imię i nazwisko: {order.OrderDetails.FirstName} {order.OrderDetails.LastName}");
                        col.Item().Text($"Adres: {order.OrderDetails.Address}, {order.OrderDetails.City}, {order.OrderDetails.Country}");
                        col.Item().Text($"Numer telefonu: {order.OrderDetails.PhoneNumber}");

                        col.Item().LineHorizontal(1);

                        decimal totalPrice = 0m;

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(4); // Opis
                                cols.RelativeColumn(2); // Ilość
                                cols.RelativeColumn(2); // Cena jedn.
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Opis").Bold();
                                header.Cell().Text("Ilość").Bold();
                                header.Cell().Text("Cena").Bold();
                            });

                            foreach (var item in billProducts)
                            {
                                table.Cell().Text(item.Name);
                                table.Cell().Text(item.Quantity);
                                table.Cell().Text($"{item.Price} zł");
                                totalPrice += item.Price * item.Quantity;
                            }
                        });

                        col.Item().LineHorizontal(1);

                        var vatValue = totalPrice * 0.23m;
                        col.Item().AlignRight().Column(right =>
                        {
                            right.Item().Text($"Netto: {totalPrice - vatValue:0.00} zł").FontSize(12);
                            right.Item().Text($"VAT (23%): {vatValue:0.00} zł").FontSize(12);
                            right.Item().Text($"Brutto: {totalPrice:0.00} zł").FontSize(14).Bold();
                        });

                        col.Item().PaddingTop(20).Text("Dziękujemy za skorzystanie z naszych usług!").Italic().FontSize(11);
                    });
                });

                page.Footer().AlignCenter().Text("Wygenerowano automatycznie - brak podpisu");
            });
        });

        return document.GeneratePdf();
    }
}