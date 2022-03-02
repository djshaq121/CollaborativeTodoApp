using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;

namespace Todo.Service
{
    public static class Extensions
    {
        public static TodoDto AsDto(this TodoItem todo)
        {
            return new TodoDto(todo.Id, todo.Title, todo.Desciption, todo.CreatedDate);
        }

        public static BoardDto AsDto(this Board board)
        {
            return new BoardDto(board.Id, board.Name, board.CreatedDate);
        }
    }
}
