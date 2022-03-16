using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.UnitOfWorkRepository;

namespace Todo.Service.Services
{
    public class ShareableService : IShareableService
    {
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public ShareableService(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BoardShare> MakeBoardSharable(AppUser user, Board board)
        {
            // check the users permission
            if (user?.UserBoards == null || board == null)
                return null;

            var userBoard = user.UserBoards.FirstOrDefault(x => x.BoardId == board.Id && x.UserId == user.Id);
            if (userBoard == null)
                return null;

            if (userBoard?.BoardPermission.Permission != Permission.Admin)
                return null;

            var exist = await unitOfWork.BoardSharedRepository.GetBoardShare(x => x.BoardId == board.Id);
            if (exist != null)
                return exist;

            var token = tokenService.CreateShareableToken();

            // Save details to db and return token
            var boardShare = new BoardShare
            {
                BoardId = board.Id,
                Token = token
            };

            await unitOfWork.BoardSharedRepository.CreateBoardShare(boardShare);

            await unitOfWork.SaveAsync();

            return boardShare;
        }

        public async Task<bool> AddUserToBoard(AppUser user, string token)
        {
            if (user == null || string.IsNullOrEmpty(token))
                return false;

            var boardShare = await unitOfWork.BoardSharedRepository.GetBoardShare(x => x.Token == token);

            var foundBound = user.UserBoards.FirstOrDefault(x => x.BoardId == boardShare.BoardId);
            if (foundBound != null)
                return false;

            var userBoards = new UserBoard
            {
                UserId = user.Id,
                BoardId = boardShare.BoardId,
                BoardPermissionId = 2
            };

            await unitOfWork.BoardRepository.AddUserBoardAsync(userBoards);

            await unitOfWork.SaveAsync();

            return true;
        }
    }
}
