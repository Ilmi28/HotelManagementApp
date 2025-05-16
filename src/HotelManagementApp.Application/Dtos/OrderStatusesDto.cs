namespace HotelManagementApp.Application.Dtos;

public class OrderStatusesDto
{
    public required int OrderId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
}