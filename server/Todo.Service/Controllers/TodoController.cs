using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.Repositories;
using Todo.Service.TodoDtos;
using Todo.Service.UnitOfWorkRepository;

namespace Todo.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TodoController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{boardId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(int boardId)
        {
            var board = await unitOfWork.BoardRepository.GetBoardByIdAsync(boardId);
            if (board == null)
                return NotFound();

            if (board.Todos == null)
                return NoContent();

            var todoItems = board.Todos.Select(todo => todo.AsDto());
            return Ok(todoItems);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(CreateTodoDto createTodoDto)
        {
            var board = await unitOfWork.BoardRepository.GetBoardByIdAsync(createTodoDto.BoardId);
            if (board == null)
                return BadRequest("Board does not exist");

            var createdTodo = new TodoItem
            {
                Task = createTodoDto.Task,
                IsCompleted = createTodoDto.IsCompleted,
                BoardId = board.Id,
                CreatedDate = DateTimeOffset.UtcNow
            };

            if (board.Todos == null)
                board.Todos = new List<TodoItem>();

            board.Todos.Add(createdTodo);

            await unitOfWork.TodoRepository.CreateTodoItemAsync(createdTodo);

            await unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTodoItem(UpdateTodoDto updateTodoDto)
        {
            var todo = await unitOfWork.TodoRepository.GetTodoItemAsync(updateTodoDto.TodoId);
            if (todo == null)
                return NoContent();

            todo.Task = updateTodoDto.Task;
            todo.IsCompleted = updateTodoDto.IsCompleted;

            await unitOfWork.SaveAsync();

            return Ok();
        }

    }
}
