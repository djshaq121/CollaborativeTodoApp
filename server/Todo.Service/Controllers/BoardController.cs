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
    public class BoardController : ControllerBase
    {
        private readonly BoardRepository boardRepository;

        public BoardController(BoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
        }

        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard([FromQuery]string name)
        {
            var board = new Board
            {
                Name = name,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await boardRepository.CreateBoard(board);

            await boardRepository.SaveChangeAsync();
            
            return Ok(board.AsDto());
        }


    }
}
