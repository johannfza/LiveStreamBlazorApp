using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MediaManager.Api.SignalR
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Debug.WriteLine($"Client with {Context.ConnectionId} Connected to NotificationHub");
            return base.OnConnectedAsync();
        }
    }
}
