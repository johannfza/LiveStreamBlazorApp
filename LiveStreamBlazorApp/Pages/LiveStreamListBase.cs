using LiveStreamBlazorApp.Models;
using MediaModelLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamListBase : ComponentBase
    {
        //Diagnostics 
        public string Timetaken = string.Empty;

        //UI
        public string CallbackMsg = string.Empty;
        public string NotificationServerConnectionStatus = "Not Connected";
        public string NotificationServerConnectionStatusColor = "black";
        public string NotificationMsg = string.Empty;
        public string MediaDbServerConnectionStatus = "Not Connected";
        public string MediaDbServerConnectionStatusColor = "black";
        public string MediaDbNotificationMsg = string.Empty;

        //ApiEnpoints
        private string DbLiveStreamsUrl = "https://localhost:44354/api/livestreams";

        //ServerHubUrl
        private string notificationServerUrl = "https://localhost:44354/notificationhub" ;
        private string MedaiDbServerUrl = "https://localhost:44354/mediadbhub";

        protected ModalType modalType = ModalType.None;

        [Inject]
        protected HttpClient http { get; set; }

        HubConnection notificationServerConnection = null;
        HubConnection mediaDbServerConnection = null;

        public List<LiveStream> LiveStreams { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var watch = Stopwatch.StartNew();
            await ConnectToNotificationServerAsync();
            await ConnectToMedaiDbServerAsync();
            await OnUpdateLiveStreamsAsync();
            Timetaken = $"'OnInitializedAsync()' timetaken:  {watch.ElapsedMilliseconds}";
        }

        protected async Task OnUpdateLiveStreamsAsync()
        {
            LiveStreams = await http.GetJsonAsync<List<LiveStream>>(DbLiveStreamsUrl);
        }

        protected void AddNewStream()
        {
            modalType = ModalType.Create;
        }

        protected void OnCloseModalCallback(string msg)
        {
            modalType = ModalType.None;
            CallbackMsg = msg;
        }

        protected void OnNotificationServerConnectedUI()
        {
            NotificationServerConnectionStatus = "Connected";
            NotificationServerConnectionStatusColor = "green";
            NotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  Connected" ;
        }

        protected void OnNotificationServerDisconnectedUI()
        {
            NotificationServerConnectionStatus = "Disconnected";
            NotificationServerConnectionStatusColor = "red";
            NotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  Disconnected";
        }

        protected void OnMediaDbServerConnectedUI()
        {
            MediaDbServerConnectionStatus = "Connected";
            MediaDbServerConnectionStatusColor = "green";
            MediaDbNotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  Connected";
        }

        protected void OnMediaDbServerDisconnectedUI()
        {
            MediaDbServerConnectionStatus = "Disconnected";
            MediaDbServerConnectionStatusColor = "red";
            MediaDbNotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  Disonnected";

        }

        protected async Task ConnectToNotificationServerAsync()
        {
            notificationServerConnection = new HubConnectionBuilder().WithUrl(notificationServerUrl).Build();

            await notificationServerConnection.StartAsync();
            OnNotificationServerConnectedUI();

            notificationServerConnection.Closed += async (s) =>
            {
                await notificationServerConnection.StartAsync();
                OnNotificationServerDisconnectedUI();
            };

            notificationServerConnection.On<string>("nofitication", msg =>
            {
                NotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  " + msg;
                StateHasChanged();
            });
        }

        protected async Task ConnectToMedaiDbServerAsync()
        {
            mediaDbServerConnection = new HubConnectionBuilder().WithUrl(MedaiDbServerUrl).Build();

            await mediaDbServerConnection.StartAsync();
            OnMediaDbServerConnectedUI();

            mediaDbServerConnection.Closed += async (s) =>
            {
                await mediaDbServerConnection.StartAsync();
                OnMediaDbServerDisconnectedUI();
            };

            mediaDbServerConnection.On<string>("dbupdate",async msg =>
            {
                MediaDbNotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  " + msg;
                await OnUpdateLiveStreamsAsync();
                StateHasChanged();
            });
        }
    }
}
