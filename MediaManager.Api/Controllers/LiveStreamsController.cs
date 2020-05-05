using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaManager.Api.Data;
using MediaModels;
using System.Diagnostics;
using MediaManager.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MediaManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveStreamsController : ControllerBase
    {
        private readonly ILiveStreamRepository liveStreamRepository;
        private readonly IHubContext<MediaDbHub> hubContext;
        private readonly IHubContext<NotificationHub> notificationHubContext;

        public LiveStreamsController(ILiveStreamRepository liveStreamRepository, IHubContext<MediaDbHub> hubcontext, IHubContext<NotificationHub> notificationHubContext)
        {
            this.liveStreamRepository = liveStreamRepository;
            this.hubContext = hubcontext;
            this.notificationHubContext = notificationHubContext;
        }


        // GET: api/LiveStreams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiveStream>>> GetLiveStream()
        {
            try
            {
                return Ok(await liveStreamRepository.GetLiveStreams());

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }
        }

        // GET: api/LiveStreams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiveStream>> GetLiveStream(int id)
        {

            try
            {
                var result = await liveStreamRepository.GetLiveStream(id);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }

        }

        // PUT: api/LiveStreams/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLiveStream(int id, LiveStream liveStream)
        {
            if (id != liveStream.Id)
            {
                return BadRequest();
            }

            //liveStreamRepository.Entry(liveStream).State = EntityState.Modified;

            try
            {
                await liveStreamRepository.UpdateLiveStream(liveStream);
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!LiveStreamExists(id))
                //{
                //    return NotFound();
                //}
                //else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LiveStreams
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        //On a Successful Post return:
        //Return Http Status Code 201 
        //Return the newly created resource 
        //Include location header in the response (Uri of newly created recsource) 
        [HttpPost]
        public async Task<ActionResult<LiveStream>> CreateLiveStream(LiveStream liveStream)
        {
            try
            {
                if (liveStream == null)
                {
                    return BadRequest();
                }
                var createdLiveStream = await liveStreamRepository.AddLiveStream(liveStream);
                await hubContext.Clients.All.SendAsync("dbupdate", "A new live stream has been added!");
                await notificationHubContext.Clients.All.SendAsync("notification", $"LIVE: {createdLiveStream.Title}");
                await notificationHubContext.Clients.All.SendAsync("newlivestreamnotification", createdLiveStream);
                return CreatedAtAction(nameof(GetLiveStream), new { id = createdLiveStream.Id}, createdLiveStream );
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");

            }
        }

        // DELETE: api/LiveStreams/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LiveStream>> DeleteLiveStream(int id)
        {
            var liveStream = await liveStreamRepository.GetLiveStream(id);
            //if (liveStream == null)
            //{
            //    return NotFound();
            //}

            //_context.LiveStream.Remove(liveStream);
            //await _context.SaveChangesAsync();
            liveStreamRepository.DeleteLiveStream(id);
            return Ok(liveStream);
        }

        private async Task<ActionResult<bool>> LiveStreamExists(int id)
        {
            return await liveStreamRepository.LiveStreamExist(id);
        }
    }
}
