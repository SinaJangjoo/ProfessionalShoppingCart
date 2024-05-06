using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using System.Net;
using TestD.Data;
using TestD.Models;
using TestD.Services.IServices;

namespace TestD.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private ApiResponse _response;
        private readonly AppDbContext _db;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public MenuItemController(AppDbContext db, IBackgroundJobClient backgroundJobClient)
        {
            _db = db;
            _backgroundJobClient = backgroundJobClient;
            _response = new ApiResponse();
        }


        [HttpPost("create")]
        public ActionResult Create(string MenuItemName)
        {
            _backgroundJobClient.Enqueue<IMenuItemService>(u => u.CreateMenuItem(MenuItemName));
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok();
        }


        [HttpPost("schedule")]
        public ActionResult Schedule(string MenuItemName)
        {
            var jobId = _backgroundJobClient.Schedule(() =>
            Console.WriteLine("The name is " + MenuItemName),
                TimeSpan.FromSeconds(5));

            _backgroundJobClient.ContinueJobWith(jobId, () =>
            Console.WriteLine($"The job {jobId} has finished!"));
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok();
        }
    }
}
