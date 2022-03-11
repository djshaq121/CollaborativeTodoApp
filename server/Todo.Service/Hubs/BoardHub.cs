using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;
using Todo.Service.UnitOfWorkRepository;

namespace Todo.Service.Hubs
{
    public class BoardHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;

        public BoardHub(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var requestValue = httpContext.Request.Query["boardId"].ToString();
            var boardId = Int32.Parse(requestValue);

            var username = Context.User.GetUsername();
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null)
               throw new HubException("User not found");

            var board = await unitOfWork.BoardRepository.GetBoardByIdAsync(boardId);

            if (!(user.UserBoards.Any(ub => ub.BoardId == boardId)))
                 throw new UnauthorizedAccessException();

            await Groups.AddToGroupAsync(Context.ConnectionId, boardId.ToString());

            // Cycle found in board - return Dto's and not entity
            await Clients.Group(boardId.ToString()).SendAsync("ReceiveBoardUpdatedBoard", board.AsDto());

        }

        public async Task SendTodo(CreateTodoDto createTodoDto)
        {
            var username = Context.User.GetUsername();
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new HubException("User not found");

            var todo = new TodoItem
            {
                Task = createTodoDto.Task,
                CreatedDate = DateTimeOffset.UtcNow,
                IsCompleted = createTodoDto.IsCompleted
            };

            var board = await unitOfWork.BoardRepository.GetBoardByIdAsync(createTodoDto.BoardId);
            if (board == null || !(user.UserBoards.Any(ub => ub.BoardId == createTodoDto.BoardId)))
                throw new UnauthorizedAccessException();

            if (board.Todos == null)
                board.Todos = new List<TodoItem>();

            board.Todos.Add(todo);

            if (await unitOfWork.SaveAsync())
            {
                await Clients.Group(board.Id.ToString()).SendAsync("newTodoItem", todo.AsDto());
            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

        }
    }
}
