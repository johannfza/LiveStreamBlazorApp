using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MediaManager.Api.Data;
using MediaModels;
using System.Linq;


namespace MediaManager.Api.SignalR
{
    public class ChatHub : Hub
    {
        private readonly MediaManagementApiContext dbContext;
        public static readonly string TAG = "ChatHub: ";
        private static readonly Dictionary<string, ChatUser> userLookup = new Dictionary<string, ChatUser>();

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync(MessageClientState.RECIEVE, username, message);
        }

        public async Task Register(string username, string roomname)
        {
            var currentId = Context.ConnectionId;
            if (!userLookup.ContainsKey(currentId))
            {
                userLookup.Add(currentId, new ChatUser( username, roomname));
                await Groups.AddToGroupAsync(currentId, roomname);
                await Clients.Group(roomname).SendAsync(MessageClientState.RECIEVE, username, $"{username} joined the chat");

            }
            // return list of users in the room
        }

        public async Task<List<string>> GetUsers(string roomname)
        {
            var allusers = userLookup.Values.ToList();
            List<string> usersinroom = new List<string>();

            foreach (ChatUser cu in allusers)
            {
                if(cu.RoomName == roomname)
                {
                    usersinroom.Add(cu.UserName);

                }
            }
            return usersinroom;

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

            if (!userLookup.TryGetValue(id, out ChatUser chatuser))
            {
                chatuser.UserName = "[unknown]";
            }

            userLookup.Remove(id);
            await Clients.AllExcept(Context.ConnectionId).SendAsync(
                MessageClientState.RECIEVE,
                chatuser.UserName, $"{chatuser.UserName} has left the chat"
                );
            await base.OnDisconnectedAsync(exception);
        }

    }
}
