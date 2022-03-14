
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Principal;
using System.Threading.Tasks;
using Todo.Service.Controllers;
using Todo.Service.Entities;
using Todo.Service.UnitOfWorkRepository;
using Xunit;

namespace TodoAppTests
{
   
    public class BoardControllerTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly BoardController boardController;

        public BoardControllerTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            boardController = new BoardController(mockUnitOfWork.Object);
        }


        //https://stackoverflow.com/questions/49330154/mock-user-identity-in-asp-net-core-for-unit-testing
       [Fact]
        public async Task CreateBoard()
        {

            // Arrange

            var boardName = "My first Board";
            var user = new AppUser
            {
                UserName = "shaq",
            };

            var board = new Board
            {
                Name = boardName
            };

            Mock<ControllerContext> controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(x => 
                x.HttpContext.User.IsInRole(It.Is<string>(s => s.Equals("admin")))
                ).Returns(true);
            boardController.ControllerContext = controllerContextMock.Object;

            mockUnitOfWork.Setup(x => x.UserRepository.GetUserByUsernameAsync("boardName"))
                .ReturnsAsync(user);

            mockUnitOfWork.Setup(x => x.BoardRepository.CreateBoardAsync(board, user));

            mockUnitOfWork.Setup(x => x.SaveAsync());
               
            // Act
            var actionResult = await boardController.CreateBoard(boardName);

            // Assert

            var result = (actionResult.Result as OkObjectResult).Value as Board;
            Assert.NotNull(result);

            Assert.Equal(boardName, result.Name);
        }
    }
}
