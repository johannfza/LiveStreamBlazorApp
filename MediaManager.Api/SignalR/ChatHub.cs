using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MediaModels;


namespace MediaManager.Api.SignalR
{
    public class ChatHub : Hub
    {

        public static readonly string TAG = "ChatHub: ";
        private static readonly Dictionary<string, string> userLookup = new Dictionary<string, string>();

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync(MessageClientState.RECIEVE, username, message);
        }

        public async Task Register(string username, string roomName)
        {
            var currentId = Context.ConnectionId;
            if (!userLookup.ContainsKey(currentId))
            {
                userLookup.Add(currentId, username);
                await Groups.AddToGroupAsync(currentId, roomName);
                await Clients.OthersInGroup(roomName).SendAsync(MessageClientState.RECIEVE, username, $"{username} joined the chat");

            }
            // return list of users in the room
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{TAG} Connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{TAG} Disonnected {exception?.Message} {Context.ConnectionId}");
            string id = Context.ConnectionId;

            if (!userLookup.TryGetValue(id, out string username))
            {
                username = "[unknown]";
            }

            userLookup.Remove(id);
            await Clients.AllExcept(Context.ConnectionId).SendAsync(
                MessageClientState.RECIEVE,
                username, $"{username} has left the chat"
                );
            await base.OnDisconnectedAsync(exception);
        }

    }
}
