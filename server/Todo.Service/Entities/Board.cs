using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TodoItem> Todos { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public ICollection<UserBoard> UserBoards { get; set; }

    }
}
