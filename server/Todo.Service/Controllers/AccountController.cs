using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.AccountDtos;
using Todo.Service.Entities;
using Todo.Service.Services;

namespace Todo.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("Username or password is incorrect");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded)
                return BadRequest("Username or password is incorrect");

            return new UserDto(user.UserName, tokenService.CreateToken(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await userManager.FindByNameAsync(registerDto.UserName) != null)
                return BadRequest("Username is taken");

            var user = new AppUser()
            {
                UserName = registerDto.UserName,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return new UserDto(user.UserName, tokenService.CreateToken(user));
        }
    }
}
