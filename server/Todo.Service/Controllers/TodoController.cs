using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.Repositories;
using Todo.Service.TodoDtos;

namespace Todo.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly BoardRepository boardRepository;
        private readonly TodoRepository todoRepository;

        public TodoController(BoardRepository boardRepository, TodoRepository todoRepository)
        {
            this.boardRepository = boardRepository;
            this.todoRepository = todoRepository;
        }

        [HttpGet("{boardId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(int boardId)
        {
            var board = await boardRepository.GetBoardByIdAsync(boardId);
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
            var board =  await boardRepository.GetBoardByIdAsync(createTodoDto.BoardId);
            if (board == null)
                return BadRequest("Board does not exist");

            var createdTodo = new TodoItem
            {
                Title = createTodoDto.Title,
                Desciption = createTodoDto.Description,
                BoardId = board.Id,
                CreatedDate = DateTimeOffset.Now
            };

            if (board.Todos == null)
                board.Todos = new List<TodoItem>();

            board.Todos.Add(createdTodo);

            await todoRepository.CreateTodoItem(createdTodo);

            await todoRepository.SaveChangeAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTodoItem(UpdateTodoDto updateTodoDto)
        {
            var todo = await todoRepository.GetTodoItem(updateTodoDto.TodoId);
            if (todo == null)
                return NoContent();


            todo.Title = updateTodoDto.Title;
            todo.Desciption = updateTodoDto.Description;

            await todoRepository.SaveChangeAsync();

            return Ok();
        }

    }
}
