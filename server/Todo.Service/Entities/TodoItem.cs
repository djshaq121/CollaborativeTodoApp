using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Desciption { get; set; }

        [Required]
        public DateTimeOffset CreatedDate { get; set; }

        public int BoardId { get; set; }

        public Board Board { get; set; }
    }
}
