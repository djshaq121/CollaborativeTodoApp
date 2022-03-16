using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.Services;
using Todo.Service.TodoDtos;
using Todo.Service.UnitOfWorkRepository;

namespace Todo.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IShareableService shareableService;

        public BoardController(IUnitOfWork unitOfWork, IShareableService shareableService)
        {
            this.unitOfWork = unitOfWork;
            this.shareableService = shareableService;
        }

        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is empty");

            var username = User.GetUsername();
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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<BoardDto>>> GetAllBoardsForUser()
        {
            var username = User.GetUsername();
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var boards = await unitOfWork.BoardRepository.GetBoardsByUserAsync(x => x.UserBoards.Any(ub => ub.UserId == user.Id));

            var ownedBoards = boards.Where(x => x.UserBoards.Any(ub => ub.BoardPermissionId == 1));
              
            var sharedBoard = boards.Where(x => x.UserBoards.Any(ub => ub.BoardPermissionId != 1));

            var ownedBoardsDto = ownedBoards.Select(b => b.AsDto());
            var sharedBoardDto = sharedBoard.Select(b => b.AsDto());

            var userBoardsDto = new UserBoardsDto(ownedBoardsDto, sharedBoardDto);

            return Ok(userBoardsDto);
        }

        [HttpPost("sharing/generate/{boardId}")]
        public async Task<ActionResult<string>> ShareBoard(int boardId)
        {
            var username = User.GetUsername();

            var userTask = unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var boardTask =  unitOfWork.BoardRepository.GetBoardByIdAsync(boardId);

            await Task.WhenAll(userTask, boardTask);
            var boardShare = await shareableService.MakeBoardSharable(userTask.Result, boardTask.Result);
            if (boardShare == null)
                return BadRequest();

            return Ok(boardShare.Token);
        }

        [HttpPost("sharing")]
        public async Task<ActionResult> Invite([FromQuery] string token)
        {
            var username = User.GetUsername();
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (user == null)
                return BadRequest("User not found");

            if (string.IsNullOrEmpty(token))
                return BadRequest("No token provided");

            var result = await shareableService.AddUserToBoard(user, token);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to share board");

            return NoContent();
        }


    }
}
