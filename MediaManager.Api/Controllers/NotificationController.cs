using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MediaManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string title)
        {
            await hubContext.Clients.All.SendAsync("notification", $"{DateTime.Now}: {title}");
            return Ok("nofification has been succsfully sent");
        }

    }
}