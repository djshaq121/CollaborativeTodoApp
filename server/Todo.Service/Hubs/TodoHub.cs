using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.Repositories;
using Todo.Service.TodoDtos;

namespace Todo.Service.Hubs
{
    public class TodoHub : Hub
    {
        private readonly TodoRepository todoRepository;
        private readonly BoardRepository boardRepository;

        public TodoHub(TodoRepository todoRepository, BoardRepository boardRepository)
        {
            this.todoRepository = todoRepository;
            this.boardRepository = boardRepository;
        }

        public override async Task OnConnectedAsync()
        {
            // await Clients.Others.SendAsync("UserIsOnline"), );

            //var board = await boardRepository.GetBoardByIdAsync(1);
            //var name = board.Name;

            //await Groups.AddToGroupAsync(Context.ConnectionId, name);

            //var todos = board.Todos.Select(x => x.AsDto());

            //await Clients.Group(name).SendAsync("ReceiveTodos", todos);
        }

        public async Task AddTodo(CreateTodoDto createTodoDto)
        {
            var board = await boardRepository.GetBoardByIdAsync(createTodoDto.BoardId);
            if (board == null)
               throw new HubException("Board does not exist");

            var createdTodo = new TodoItem
            {
                Task = createTodoDto.Task,
                IsCompleted = createTodoDto.IsCompleted,
                BoardId = board.Id,
                CreatedDate = DateTimeOffset.Now
            };

            if (board.Todos == null)
                board.Todos = new List<TodoItem>();

            board.Todos.Add(createdTodo);

            await todoRepository.CreateTodoItem(createdTodo);
            await todoRepository.SaveChangeAsync();

            var group = board.Name;
            await Clients.Group(group).SendAsync("AddNewTodoItem", createdTodo.AsDto());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

        }
    }
}
