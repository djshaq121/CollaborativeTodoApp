using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.Entities
{
    public class BoardShare
    {
        public int Id { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }
        public string Token { get; set; }
    }
}
