using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;
using Todo.Service.UnitOfWorkRepository;

namespace Todo.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public BoardController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is empty");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var board = new Board
            {
                Name = name,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await unitOfWork.BoardRepository.CreateBoardAsync(board, user);

            await unitOfWork.SaveAsync();
            
            return Ok(board.AsDto());
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        //{
        //    var board = await boardRepository.GetBoardByIdAsync(id);

        //    if (board == null)
        //        NoContent();

        //    return Ok(board);
        //}

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<BoardDto>>> GetAllBoardsForUser()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var boards = await unitOfWork.BoardRepository.GetBoardsByUserAsync(user.Id);

            return Ok(boards.Select(x => x.AsDto()));
        }


    }
}
