﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using WebStore.Constants;
using WebStore.Data.Entitties.Identity;
using WebStore.Helpers;
using WebStore.Interfaces;
using WebStore.Models.Account;

namespace WebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<UserEntity> _userManager;

        public AccountController(IJwtTokenService jwtTokenService, UserManager<UserEntity> userManager)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user!=null)
            {
                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                if(!isPasswordValid)
                {
                    return BadRequest();
                }
                var token = await _jwtTokenService.CreateToken(user);
                return Ok(new { token });
            }

            return BadRequest();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            UserEntity user = new UserEntity()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };
            //base64
            if (model.ImageBase64.Contains(','))
                model.ImageBase64 = model.ImageBase64.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(model.ImageBase64);

            string exp = ".webp";
            string imageName = Path.GetRandomFileName() + exp;
            string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
            var saveBytes = ImageProcessingHelper.ResizeImage(bytes, 300, 300);
            System.IO.File.WriteAllBytes(dirSaveImage, saveBytes);
            user.Image = imageName;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, Roles.User);
                return Ok();
            }
            return BadRequest();
        }
    }
}
