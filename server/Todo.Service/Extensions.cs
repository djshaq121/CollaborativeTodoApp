using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;

namespace Todo.Service
{
    public static class Extensions
    {
        public static TodoDto AsDto(this TodoItem todo)
        {
            return new TodoDto(todo.Id, todo.Task, todo.IsCompleted, todo.CreatedDate);
        }

        public static BoardDto AsDto(this Board board)
        {
            return new BoardDto(board.Id, board.Name, board.CreatedDate, board.Todos?.Select(x => x.AsDto()).ToList());
        }

        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
