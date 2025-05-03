using HotelManagementApp.Core.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.Add;

public class AddRoomCommand : IRequest
{
    [Required]
    [MaxLength(50)]
    public required string RoomName { get; set; }
    [Required]
    public required int HotelId { get; set; }
    [Required]
    public required RoomTypeEnum RoomType { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public required decimal Price { get; set; }
    [Required]
    [MinLength(50)]
    [MaxLength(500)]
    public required string Description { get; set; }
}
