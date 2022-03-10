using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.Entities
{
   public enum Permission
    {
        Admin,
        Write,
        Read,
    }

    public class BoardPermission
    {
        public int Id { get; set; }

        public Permission Permission { get; set; }
    }
}
