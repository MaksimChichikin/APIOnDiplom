using Microsoft.AspNetCore.Mvc;
using MyProApiDiplom.CommonAppData.DTO;
using MyProApiDiplom.CommonAppData.User;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MyProApiDiplom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsermyClass _userClass;

        public UsersController(UsermyClass userClass)
        {
            _userClass = userClass;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userClass.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userClass.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var updatedUser = await _userClass.UpdateUserAsync(id, userDto);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserCreateDTO userCreateDto)
        {
            var createdUser = await _userClass.CreateUserAsync(userCreateDto);

            return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userClass.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDTO notificationDto)
        {
            var success = await _userClass.SendNotificationAsync(notificationDto.Email, notificationDto.MessageBody);
            if (!success)
            {
                return NotFound("User not found");
            }

            return Ok("Notification sent successfully");
        }
    }
}
