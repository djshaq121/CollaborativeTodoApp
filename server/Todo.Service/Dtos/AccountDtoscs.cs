using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Service.AccountDtos
{
   public record UserDto(string Username, string Token); 

   public record LoginDto(string UserName, string Email, string Password);

   public record RegisterDto(string UserName, string Email, string Password, string ConfirmedPassword);
}
