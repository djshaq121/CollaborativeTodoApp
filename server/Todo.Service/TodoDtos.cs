using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.TodoDtos
{
    public record CreateTodoDto(int BoardId, string Title, string Description);

    public record UpdateTodoDto(int BoardId, int TodoId, string Title, string Description);

    public record BoardDto(int Id, string Title, DateTimeOffset CreatedDate);

    public record TodoDto(int Id, string Title, string Description, DateTimeOffset CreatedDate);
}
