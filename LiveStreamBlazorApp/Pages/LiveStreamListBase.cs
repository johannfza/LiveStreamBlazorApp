using MediaModelLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamListBase : ComponentBase
    {
        HubConnection connection = null;

        protected string connectionStatus = "Closed";

        protected List<string> notifications = new List<string>();

        protected bool isConnected = false;

        private string hubNotificationUrl = "http://localhost:50395/notificationhub";

        [Inject]
        protected HttpClient http { get; set; }

        public bool AddStreamApprear { get; set; } = false;

        public List<LiveStream> LiveStreams { get; set; }

        protected override async Task OnInitializedAsync()
        {

            try
            {
                LiveStreams = await http.GetJsonAsync<List<LiveStream>>("http://localhost:50395/api/livestreams");

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void AddNewStream()
        {
            AddStreamApprear = !AddStreamApprear;
        }

        protected async Task ConnectToServer()
        {
            this.connection = new HubConnectionBuilder()
                .WithUrl(hubNotificationUrl)
                .Build();

            await connection.StartAsync();
            isConnected = true;
            connectionStatus = "Connected";

            connection.Closed += async (s) =>
            {
                isConnected = false;
                connectionStatus = "Disconnected";
                await connection.StartAsync();
                isConnected = true;
            };

            connection.On<string>("notification", m =>
            {
                notifications.Add(m);
                StateHasChanged();
            });

        }
    }
}
