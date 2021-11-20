using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantLogic.Models;
using RestaurantLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    //[Authorize]

    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        //[AllowAnonymous]
        public ActionResult RegisterUser([FromBody]RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        //[AllowAnonymous]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateUserDto dto)
        {
            var id = _accountService.Create(dto);

            return Created($"/api/account/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var result = _accountService.GetAll();

            return Ok(result);
        }

        [HttpGet("{userId}")]
        public ActionResult<UserDto> GetById([FromRoute] int userId)
        {
            UserDto user = _accountService.GetById(userId);

            return Ok(user);
        }

        [HttpPost("update/{userId}")]
        public ActionResult Update([FromBody] UpdateUserDto dto, [FromRoute] int userId)
        {
            _accountService.Update(userId, dto);

            return Ok();
        }

        [HttpDelete("delete/{userId}")]
        public ActionResult Delete([FromRoute] int userId)
        {
            _accountService.Delete(userId);

            return Ok();
        }
    }
}
