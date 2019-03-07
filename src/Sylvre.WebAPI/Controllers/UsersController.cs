﻿using System.Threading.Tasks;
using System.Collections.Generic;

using Sylvre.WebAPI.Data;
using Sylvre.WebAPI.Dtos;
using Sylvre.WebAPI.Entities;

using Sylvre.WebAPI.Services;
using Sylvre.WebAPI.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Sylvre.WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="newUser">The user to register.</param>
        /// <response code="201">Successfully registered the user.</response>
        /// <response code="400">Problems occured with registration (e.g. username/email taken, etc.)</response>
        /// <returns>The user that was created and registered.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserDto newUser)
        {
            var newUserEntity = GetUserEntityFromUserDto(newUser);

            User createdUserEntity;
            try
            {
                createdUserEntity = await _userService.CreateAsync(newUserEntity, newUser.Password);
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return CreatedAtAction("Register", new { id = createdUserEntity.Id },
                GetUserResponseDtoFromUserEntity(createdUserEntity));
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        /// <param name="id">The id of the user to retrieve.</param>
        /// <response code="200">Successfully retrieved the user by id.</response>
        /// <response code="404">User with the given id was not found.</response>
        /// <returns>The user by id.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserResponseDto>> GetUserById([FromRoute] int id)
        {
            var userEntity = await _userService.RetrieveAsync(id);

            if (userEntity == null)
            {
                return NotFound(new { Message = "User with given id not found" });
            }

            return Ok(GetUserResponseDtoFromUserEntity(userEntity));
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <response code="200">Successfully retrieved a list of all users.</response>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            IEnumerable<User> userEntities = await _userService.RetrieveAllAsync();

            var response = new List<UserResponseDto>();
            foreach (User userEntity in userEntities)
            {
                response.Add(GetUserResponseDtoFromUserEntity(userEntity));
            }

            return Ok(response);
        }

        /// <summary>
        /// Update a user by id with new details.
        /// </summary>
        /// <param name="id">The id of the user to update.</param>
        /// <param name="updatedUser">The updated details to update the user with.</param>
        /// <response code="204">Successfully updated the user with the new details.</response>
        /// <response code="400">Problems occured with updating (e.g. username/email taken, etc.)</response>
        /// <response code="404">User to update was not found by their id.</response>
        /// <returns>204 No Content response.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody] UserDto updatedUser)
        {
            var userToUpdateEntity = await _userService.RetrieveAsync(id);
            if (userToUpdateEntity == null)
            {
                return NotFound(new { Message = "User with given id not found" });
            }

            var updatedUserEntity = GetUserEntityFromUserDto(updatedUser);
            try
            {
                await _userService.UpdateAsync(updatedUserEntity, userToUpdateEntity,
                    updatedUser.Password);
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <response code="204">Successfully deleted the user.</response>
        /// <response code="404">User to delete was not found by their id.</response>
        /// <returns>204 No Content response.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserResponseDto>> DeleteUser([FromRoute] int id)
        {
            var userToDeleteEntity = await _userService.RetrieveAsync(id);
            if (userToDeleteEntity == null)
            {
                return NotFound(new { Message = "User with given id not found" });
            }

            await _userService.DeleteAsync(userToDeleteEntity);

            return NoContent();
        }

        private User GetUserEntityFromUserDto(UserDto userDto)
        {
            return new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                FullName = userDto.FullName
            };
        }
        private UserResponseDto GetUserResponseDtoFromUserEntity(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName
            };
        }
    }
}