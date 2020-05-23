using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using MediaModels;

namespace LiveStreamBlazorApp.Models
{
    public class ChatClient
    {
        public static readonly string TAG = "ChatClient: ";

        public string HUBURL = StreamConnectionManager.CHATSERVER.GetURL();
        private HubConnection hubConnection;
        public readonly string Username;
        public readonly string Roomname;

        public List<string> UsersInChat;
        private bool started = false;

        public EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public ChatClient(string username, string roomname)
        {
            //navigationManager = na
            Username = username;
            Roomname = roomname;

        }

        //Process States: StartAsync, SendAsync, StopAsync,

        public async Task StartAsync()
        {
            if (!started)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(HUBURL)
                    .Build();

                Console.WriteLine($"{TAG} Starting ChatClient: Username= {Username}");

                hubConnection.On<string, string, List<string>>(MessageClientState.RECIEVE, async (user, message, usersinchat) =>
                {
                    await OnMessageRecieved(user, message, usersinchat);
                });

                await hubConnection.StartAsync();
                Console.WriteLine($"{TAG} A ChatClient Started: Username= {Username}");
                started = true;

                await hubConnection.SendAsync(MessageClientState.REGISTER, Username, Roomname);
                await GetUsers();
            }
        }

        public async Task GetUsers()
        {
            UsersInChat = await hubConnection.InvokeAsync<List<string>>(MessageClientState.GETUSERS, Roomname);

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


        private async Task OnMessageRecieved(string username, string message , List<string> usersinchat) // on message recieved 
        {

            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(username, message, usersinchat));
            await GetUsers();
        }
    }


    public class MessageRecievedEventArgs : EventArgs
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public List<string> UsersInChat {get; set;}

        public MessageRecievedEventArgs(string username, string message, List<string> usersinchat)
        {
            Username = username;
            Message = message;
            UsersInChat = usersinchat;
        }
    }
}
