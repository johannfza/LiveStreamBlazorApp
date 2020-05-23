using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MediaModels;

namespace LiveStreamBlazorApp.Models
{
    public class ChatClient
    {
        public static readonly string TAG = "ChatClient: ";

        public string HUBURL = StreamConnectionManager.CHATSERVER.GetURL();
        private HubConnection hubConnection;
        public readonly string Username;
        private bool started = false;

        public EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public ChatClient(string username)
        {
            //navigationManager = na
            Username = username;

        }

        //Process States: StartAsync, SendAsync, StopAsync,

        public async Task StartAsync(string roomName)
        {
            if (!started)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(HUBURL)
                    .Build();

                Console.WriteLine($"{TAG} Starting ChatClient: Username= {Username}");

                hubConnection.On<string, string>(MessageClientState.RECIEVE, (user, message) =>
                {
                    OnMessageRecieved(user, message);

                });

                await hubConnection.StartAsync();
                Console.WriteLine($"{TAG} A ChatClient Started: Username= {Username}");
                started = true;

                await hubConnection.SendAsync(MessageClientState.REGISTER, Username, roomName);
            }
        }

        public async Task SendAsync(string message)
        {
            if (!started)
            {
                throw new InvalidOperationException($"{TAG} Client not started");
            }
            await hubConnection.SendAsync(MessageClientState.SEND, Username, message);
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

        //Events: OnMessageRecieved


        private void OnMessageRecieved(string username, string message) // on message recieved 
        {

            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(username, message));
            //vs

            // if(MessageRecieved != null)
            // MessageRecieved(this, new MessageRecievedEventArgs(username, message));
        }
    }


    public class MessageRecievedEventArgs : EventArgs
    {
        public string Username { get; set; }
        public string Message { get; set; }

        public MessageRecievedEventArgs(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
}
