using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using MediaModels;

namespace LiveStreamBlazorApp.Models
{
    public class LiveStreamNotificationClient
    {
        public static readonly string TAG = "LiveSteamNotificationClient: ";

        public string HUBURL = StreamConnectionManager.LIVESTREAMNOTIFICATIONSERVER.GetURL();
        private HubConnection hubConnection;
        private bool started = false;

        public bool isLive = false;

        public List<LiveStream> currentstreams;

        public EventHandler<LiveStreamEventArgs> LiveStreamEvent;


        public LiveStreamNotificationClient()
        {

        }

        public async Task StartAsync()
        {
            if(!started)
            {
                hubConnection = new HubConnectionBuilder().WithUrl(HUBURL).Build();

                hubConnection.On<LiveStream>(LiveStreamClientState.LIVE, (liveStream) =>
                {
                    OnLiveStreamStart(liveStream);
                });

                hubConnection.On<LiveStream>(LiveStreamClientState.ENDED, (liveStream) =>
                {
                    OnLiveStreamEnd(liveStream);
                });

                await hubConnection.StartAsync();

                started = true;

                currentstreams = await hubConnection.InvokeAsync<List<LiveStream>>(LiveStreamClientState.REGISTER);

                if (currentstreams.Count != 0)
                {
                    foreach (var i in currentstreams)
                    {
                        Console.WriteLine(TAG + i.Title);
                    }
                }

                Console.WriteLine($"{TAG} Starting" );
            }
        }

        public async Task LiveStreamStartedAsync(LiveStream liveStream)
        {
            if (!started)
            {
                throw new InvalidOperationException($"{TAG} Client not started");
            }
            await hubConnection.SendAsync(LiveStreamClientState.LIVE, liveStream);
        }

        public async Task LiveStreamEndedAsync(LiveStream liveStream)
        {
            if (!started)
            {
                throw new InvalidOperationException($"{TAG} Client not started");
            }
            await hubConnection.SendAsync(LiveStreamClientState.ENDED, liveStream);
        }

        public async Task StopAsync()
        {
            if (started)
            {
                await hubConnection.StopAsync();
                await hubConnection.DisposeAsync();
                hubConnection = null;
                started = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            Console.WriteLine($"{TAG} Disposing");
            await StopAsync();
        }

        private void OnLiveStreamEnd(LiveStream liveStream) // on message recieved 
        {
            LiveStreamEvent?.Invoke(this, new LiveStreamEventArgs(liveStream, false)); 
        }

        private void OnLiveStreamStart(LiveStream liveStream) // on message recieved 
        {
            LiveStreamEvent?.Invoke(this, new LiveStreamEventArgs(liveStream, true));
        }


    }

    public class LiveStreamEventArgs : EventArgs
    {

        public bool isLive { get; set; }
        public LiveStream LiveStream { get; set; }

        public LiveStreamEventArgs(LiveStream liveStream, bool islive)
        {
            isLive = islive;
            LiveStream = liveStream;
        }
    }
}
