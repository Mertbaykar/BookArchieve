using Book.Archieve.API.Controllers;
using Book.Archieve.Application.UnitOfWork;
using Book.Archieve.Domain.DTO.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.Archieve.Controllers
{

    public class UserController : BasicController
    {

        private readonly IUserUnitOfWork userUnitOfWork;

        public UserController(IUserUnitOfWork userUnitOfWork)
        {
            this.userUnitOfWork = userUnitOfWork;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            var result = userUnitOfWork.Register(registerRequest);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = userUnitOfWork.Login(loginRequest);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateShareSetting(int shareId)
        {
            userUnitOfWork.UpdateShareSetting(shareId, UserId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddFriend(int id)
        {
            userUnitOfWork.AddFriend(id, UserId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteFriend(int id)
        {
            userUnitOfWork.DeleteFriend(id, UserId);
            return Ok();
        }
    }
}
