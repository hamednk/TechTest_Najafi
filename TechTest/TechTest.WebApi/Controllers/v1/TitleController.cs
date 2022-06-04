using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechTest.Application.Features.Titles.Commands.CreateTitleToDbCommand;
using TechTest.Application.Features.Titles.Commands.SetToRedis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTest.WebApi.Controllers.v1
{
    public class TitleController : BaseApiController
    {
        // POST api/<controller>
        [HttpPost("SetFileToRedis")]
        public async Task<IActionResult> SetFileToRedis(IFormFile file)
        {
            return Ok(await Mediator.Send(new SetToRedisCommand { Stream = file.OpenReadStream() }));
        }

        [HttpPost("CreateTitleToDb")]
        public async Task<IActionResult> CreateTitleToDb()
        {
            return Ok(await Mediator.Send(new CreateTitleToDbCommand()));
        }

    }
}
