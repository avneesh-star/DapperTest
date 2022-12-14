using DapperTest.Models;
using DapperTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("dr")]
        public async Task<IActionResult> GetList([FromQuery] int Page, [FromQuery] int PageSize)
        {
            try
            {
                Page = 1;
                PageSize = 25;
                var res = await _userService.GetDrRecordsList(Page, PageSize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("dl")]
        public async Task<IActionResult> GetDlList([FromQuery] int Page, [FromQuery] int PageSize)
        {
            try
            {
                Page = 1;
                PageSize = 25;
                var res = await _userService.GetDlRecordsList(Page, PageSize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
