using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetRoomTypes;
using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class GetRoomTypesQueryHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly GetRoomTypesQueryHandler _handler;

        public GetRoomTypesQueryHandlerTests()
        {
            _handler = new GetRoomTypesQueryHandler(_roomRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnRoomTypes_WhenRoomTypesExist()
        {
            var roomTypes = new List<HotelRoomType>
            {
                new HotelRoomType { Id = 1, Name = "Economy" },
                new HotelRoomType { Id = 2, Name = "Premium" }
            };

            _roomRepositoryMock.Setup(r => r.GetRoomTypes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(roomTypes);

            var result = await _handler.Handle(new GetRoomTypesQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == RoomTypeEnum.Economy.ToString());
            Assert.Contains(result, r => r.Name == RoomTypeEnum.Premium.ToString());
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoRoomTypesExist()
        {
            _roomRepositoryMock.Setup(r => r.GetRoomTypes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelRoomType>());

            var result = await _handler.Handle(new GetRoomTypesQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
