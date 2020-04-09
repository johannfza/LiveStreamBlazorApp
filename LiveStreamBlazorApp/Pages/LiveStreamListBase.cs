using LiveStreamBlazorApp.Models;
using MediaModelLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamListBase : ComponentBase
    {
        //UI
        public string CallbackMsg = "";
        public string NotificationServerConnectionStatus = "Not Connected";
        public string NotificationServerConnectionStatusColor = "black";
        public string NotificationMsg = "";
        public string MediaDbServerConnectionStatus = "Not Connected";
        public string MediaDbServerConnectionStatusColor = "black";
        public string MediaDbNotificationMsg = "";

        //ApiEnpoints
        private string DbLiveStreamsUrl = "https://localhost:44354/api/livestreams";

        //ServerHubURl
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
            await ConnectToNotificationServer();
            try
            {
                LiveStreams = await http.GetJsonAsync<List<LiveStream>>(DbLiveStreamsUrl);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task UpdateLiveStreams()
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
        }

        protected void OnNotificationServerDisconnectedUI()
        {
            NotificationServerConnectionStatus = "Disconnected";
            NotificationServerConnectionStatusColor = "red";
        }

        protected void OnMediaDbServerConnectedUI()
        {
            MediaDbServerConnectionStatus = "Connected";
            MediaDbServerConnectionStatusColor = "green";
        }

        protected void OnMediaDbServerDisconnectedUI()
        {
            MediaDbServerConnectionStatus = "Disconnected";
            MediaDbServerConnectionStatusColor = "red";
        }

        protected async Task ConnectToNotificationServer()
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

        protected async Task ConnectToMedaiDbServer()
        {
            mediaDbServerConnection = new HubConnectionBuilder().WithUrl(MedaiDbServerUrl).Build();

            await mediaDbServerConnection.StartAsync();
            OnMediaDbServerConnectedUI();

            mediaDbServerConnection.Closed += async (s) =>
            {
                await mediaDbServerConnection.StartAsync();
                OnMediaDbServerDisconnectedUI();
            };

            mediaDbServerConnection.On<string>("dbupdate", async (msg) =>
            {
                MediaDbNotificationMsg = DateTime.UtcNow.ToLocalTime().ToString() + ":  " + msg;
                await UpdateLiveStreams();
                StateHasChanged();
            });
        }
    }
}
