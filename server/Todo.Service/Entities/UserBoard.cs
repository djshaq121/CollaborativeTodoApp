using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.Entities
{
    public class UserBoard
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int BoardPermissionId { get; set; }

        public BoardPermission BoardPermission { get; set; }
    }
}
