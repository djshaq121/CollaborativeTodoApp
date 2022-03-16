using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.TodoDtos
{
    public record CreateTodoDto(int BoardId, string Task, bool IsCompleted);

    public record UpdateTodoDto(int Id, int BoardId, string Task, bool IsCompleted);

    public record UserBoardsDto(IEnumerable<BoardDto> OwnerBoards, IEnumerable<BoardDto> SharedBoards);

    public record BoardDto(int Id, string Title, DateTimeOffset CreatedDate, ICollection<TodoDto> Todos);

    public record TodoDto(int Id, string Task, bool IsCompleted, DateTimeOffset CreatedDate);
}
