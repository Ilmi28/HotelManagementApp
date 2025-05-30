﻿namespace HotelManagementApp.Core.Models.OrderModels;

public class ConfirmedOrder
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required Order Order { get; set; }
}
