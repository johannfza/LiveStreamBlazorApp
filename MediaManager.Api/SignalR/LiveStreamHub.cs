using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaModels;

namespace MediaManager.Api.SignalR
{
    public class LiveStreamHub : Hub
    {

        public static readonly string TAG = "LiveStreamHub: ";
        private static readonly Dictionary<string, LiveStream> liveStreamLookup = new Dictionary<string, LiveStream>();

        public async Task<List<LiveStream>> Register()
        {
            return liveStreamLookup.Values.ToList();
        }

        public async Task LiveStreamStarted(LiveStream liveStream)
        {
            var currentId = Context.ConnectionId;
            liveStreamLookup.Add(currentId, liveStream);
            Console.WriteLine(TAG + $"{currentId} Hub has been notified. Starting buffered countdown before notification is sent!");

            System.Threading.Thread.Sleep(20000); // problem

            Console.WriteLine(TAG + $"{currentId} has started a livestream");

            await Clients.All.SendAsync(LiveStreamClientState.LIVE,liveStream);

        }

        public async Task LiveStreamEnded(LiveStream liveStream)
        {
            var currentId = Context.ConnectionId;
            liveStreamLookup.Remove(currentId);
            Console.WriteLine(TAG + $"{currentId} has ended a livestream");
            await Clients.All.SendAsync(LiveStreamClientState.ENDED, liveStream);

        }



        public override Task OnConnectedAsync()
        {
            Console.WriteLine(TAG + $"Client with {Context.ConnectionId} Connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{TAG} Disonnected {exception?.Message} {Context.ConnectionId}");
            string id = Context.ConnectionId;

            if (!liveStreamLookup.TryGetValue(id, out LiveStream livestream))
            {
                livestream = new LiveStream();
                livestream.Title = "UNKNOWN";
            }

            liveStreamLookup.Remove(id);
            //await Clients.AllExcept(Context.ConnectionId).SendAsync(LiveStreamClientState.ENDED, livestream);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
