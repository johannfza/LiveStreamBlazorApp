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
            var currentId = Context.ConnectionId;

            if (!userLookup.TryGetValue(currentId, out ChatUser chatuser))
            {
                chatuser.UserName = "[unknown]";
                chatuser.RoomName = "noRoom";

            }

            List<string> usersinchat = await GetUsers(chatuser.RoomName);
            await Clients.Group(chatuser.RoomName).SendAsync(MessageClientState.RECIEVE, username, message, usersinchat );
        }

        public async Task Register(string username, string roomname)
        {
            var currentId = Context.ConnectionId;
            if (!userLookup.ContainsKey(currentId))
            {
                userLookup.Add(currentId, new ChatUser(username, roomname));
                await Groups.AddToGroupAsync(currentId, roomname);
                List<string> usersinchat = await GetUsers(roomname);
                await Clients.Group(roomname).SendAsync(MessageClientState.RECIEVE, username, $"{username} joined the chat", usersinchat);

            }
            // return list of users in the room
        }

        public async Task<List<string>> GetUsers(string roomname)
        {
            var allusers = userLookup.Values.ToList();
            List<string> usersinroom = new List<string>();
            await Task.Run(() =>
            {
                foreach (ChatUser cu in allusers)
                {
                    if (cu.RoomName == roomname)
                    {
                        usersinroom.Add(cu.UserName);

                    }
                }
            });
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
                chatuser.RoomName = "noRoom";

            }

            userLookup.Remove(id);
            List<string> usersinchat = await GetUsers(chatuser.RoomName);
            await Clients.Group(chatuser.RoomName).SendAsync(
                MessageClientState.RECIEVE,
                chatuser.UserName, $"{chatuser.UserName} has left the chat", usersinchat 
                );
            await base.OnDisconnectedAsync(exception);
        }

    }
}
