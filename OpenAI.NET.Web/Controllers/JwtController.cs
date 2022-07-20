using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenAI.NET.Models.Jwt.Add;
using OpenAI.NET.Models.Jwt.Auth;
using OpenAI.NET.Models.Jwt.Remove;
using OpenAI.NET.Models.Response;
using OpenAI.NET.Models.Web;
using OpenAI.NET.Web.Controllers.Interfaces;
using OpenAI.NET.Web.Cryptography;
using OpenAI.NET.Web.EntityFrameworkCore.Entities;
using OpenAI.NET.Web.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers
{
    /// <summary>
    /// Jwt controller for work with OpenAI.NET.Web.
    /// </summary>
    public class JwtController : BaseController, IJwtController
    {
        private readonly IConfiguration _configuration;

        private readonly UserRepository _userRepository;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public JwtController(
            IConfiguration configuration,
            UserRepository userRepository)
        {
            _configuration = configuration;

            _userRepository = userRepository;
        }

        [HttpPost("/Jwt/Auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthAsync(
            AuthRequest request)
        {
            return await SendResponseAsync(
                request,
                InvokeAuthAsync,
                "Exception in user authorization");
        }

        [HttpPost("/Jwt/Add")]
        [Authorize(Roles = Permission.CanManageUsers)]
        public async Task<IActionResult> AddAsync(
            AddRequest request)
        {
            return await SendResponseAsync(
                request,
                InvokeAddAsync,
                "Exception in adding new user");
        }

        [HttpPost("/Jwt/Remove")]
        [Authorize(Roles = Permission.CanManageUsers)]
        public async Task<IActionResult> RemoveAsync(
            RemoveRequest request)
        {
            return await SendResponseAsync(
                request,
                InvokeRemoveAsync,
                "Exception in removing user");
        }

        /// <summary>
        /// Async method that executing in
        /// <see cref="BaseController.SendResponseAsync{T}(T, Action{T}, string)"/>.
        /// </summary>
        /// <returns>Json content with <see cref="AuthResponse"/> body</returns>
        [NonAction]
        private async Task<IActionResult> InvokeAuthAsync(
            AuthRequest request,
            Response response)
        {
            if (await GetClaimsIdentityAsync(request) is
                    (ClaimsIdentity, EntityFrameworkCore.Entities.User) identity)
            {
                DateTime date = DateTime.UtcNow;

                JwtSecurityToken jwt = new(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    identity.Item1.Claims,
                    date,
                    date.Add(identity.Item2.TokenLifeTime),
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                _configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256));

                response.Body = new AuthResponse()
                {
                    Name = identity.Item1.Name,
                    Permissions = identity.Item2.Permissions,
                    AccessToken =
                        new JwtSecurityTokenHandler().WriteToken(jwt),
                    ExpirationDate =
                        date.Add(identity.Item2.TokenLifeTime).ToString("G")
                };

                return base.Content(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }
            else
            {
                throw new Exception(
                    "There's no user with such data");
            }
        }

        /// <summary>
        /// Async method that executing in
        /// <see cref="BaseController.SendResponseAsync{T}(T, Action{T}, string)"/>.
        /// </summary>
        /// <returns>Json content with <see cref="AddResponse"/> body</returns>
        [NonAction]
        private async Task<IActionResult> InvokeAddAsync(
            AddRequest request,
            Response response)
        {
            if (await _userRepository.GetUserByNameAsync(request.Name) is null)
            {
                if (!TimeSpan.TryParse(
                    request.TokenLifeTime,
                    out TimeSpan tokenLifeTime))
                {
                    tokenLifeTime = TimeSpan.Zero;
                }

                User user = new()
                {
                    Name = request.Name,
                    PasswordHash = Sha256.GetHash(request.Password),
                    Permissions = request.Permissions,
                    TokenLifeTime = tokenLifeTime,
                };
                await _userRepository.AddAsync(user);

                response.Body = new AddResponse()
                {
                    Name = user.Name,
                    Permissions = user.Permissions,
                    TokenLifeTime = user.TokenLifeTime.ToString("G")
                };

                return base.Content(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }
            else
            {
                throw new Exception(
                    "User with this Name already exists");
            }
        }

        /// <summary>
        /// Async method that executing in
        /// <see cref="BaseController.SendResponseAsync{T}(T, Action{T}, string)"/>.
        /// </summary>
        /// <returns>Json content with <see cref="RemoveResponse"/> body</returns>
        [NonAction]
        private async Task<IActionResult> InvokeRemoveAsync(
            RemoveRequest request,
            Response response)
        {
            if (await _userRepository.GetUserByNameAsync(request.Name) is
                    User user)
            {
                if (user.Permissions.Contains(Permission.Untouchable))
                {
                    if (User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role) &&
                        x.Value.Equals(Permission.Untouchable)) is null)
                    {
                        throw new Exception(
                            "It isn't possible to delete Untouchable user");
                    }
                }

                await _userRepository.RemoveAsync(user);

                response.Body = new RemoveResponse()
                {
                    Name = user.Name
                };

                return base.Content(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }
            else
            {
                throw new Exception(
                    "There's no user with this name");
            }
        }

        /// <summary>
        /// Async getting an authorized user.
        /// </summary>
        /// <returns>Claims and user.</returns>
        [NonAction]
        private async Task<(ClaimsIdentity, User)> GetClaimsIdentityAsync(
            AuthRequest request)
        {
            List<User> users = await _userRepository.GetAllAsync();

            if (users.FirstOrDefault(x => x.Name.Equals(request.Name) &&
                    x.PasswordHash.Equals(Sha256.GetHash(request.Password))) is User user)
            {
                List<Claim> claims = new()
                {
                    new Claim(
                        JwtRegisteredClaimNames.Iss,
                        _configuration["Jwt:Issuer"]),
                    new Claim(
                        JwtRegisteredClaimNames.Aud,
                        _configuration["Jwt:Audience"]),
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        _configuration["Jwt:Subject"]),
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Name),
                    new Claim(
                        ClaimTypes.Name,
                        user.Name)
                };
                foreach (string permission in user.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission));
                }

                return (new ClaimsIdentity(claims, "AccessToken"), user);
            }

            return (null, null);
        }
    }
}