using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.UpdateLoyaltyReward;

public class UpdateLoyaltyRewardCommand : IRequest
{
    [Required]
    public required int LoyaltyRewardId { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]   
    public required string RewardName { get; set; }
    [Required]
    [Range(1, int.MaxValue)]  
    public required int PointsRequired { get; set; }
    [Required]
    [MinLength(20)]
    [MaxLength(500)] 
    public required string Description { get; set; }
}