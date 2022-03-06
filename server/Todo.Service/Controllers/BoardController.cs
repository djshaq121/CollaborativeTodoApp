using Microsoft.AspNetCore.Authorization;
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
        private readonly IBoardRepository boardRepository;

        public BoardController(IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is empty");

            var board = new Board
            {
                Name = name,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await boardRepository.CreateBoardAsync(board);

            await boardRepository.SaveChangeAsync();
            
            return Ok(board.AsDto());
        }

        [HttpGet]
        public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        {
            var board = await boardRepository.GetBoardByIdAsync(id);

            if (board == null)
                NoContent();

            return Ok(board);
        }


    }
}
