using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaModels;

namespace MediaManager.Api.SignalR
{
    public class NotificationHub : Hub
    {
        public static readonly string TAG = "NotificationHub: ";

        public override Task OnConnectedAsync()
        {
            Console.WriteLine(TAG + $"Client with {Context.ConnectionId} Connected");
            return base.OnConnectedAsync();
        }


    }
}
