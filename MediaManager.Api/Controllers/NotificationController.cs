using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaManager.Api.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MediaManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> hubcontext;

        public NotificationController(IHubContext<NotificationHub> hubcontext)
        {
            this.hubcontext = hubcontext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string msg)
        {
            await hubcontext.Clients.All.SendAsync("notification", msg);
            return Ok($"notification with content '{msg}' has been sent successfully!");
        }

    }
}