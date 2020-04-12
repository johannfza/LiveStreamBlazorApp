using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaModelLibrary;
using System.Net.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using LiveStreamBlazorApp.Models;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamModalCreateEditBase : ComponentBase
    { 

        //Media Database Server 


        //StreamServer
        protected static string ServerAddress = "192.168.56.101";
        protected static string Port = "8080";
        protected static string AppName = "show";
        protected static ServerApplication StreamServer = new ServerApplication(ServerAddress, Port, AppName);


        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public string StreamUrl = $"rtmp://{StreamServer.Address}/{StreamServer.AppName}";
        public string StreamKey = Guid.NewGuid().ToString();
        public LiveStream liveStream = new LiveStream();

        protected ElementReference previewPlayer;

        //Modal UI
        public string ModalDisplay = "none";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        public void Open()
        {
            ModalDisplay = "block";
            ModalClass = "Show";
            ShowBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }

        [Parameter]
        public ModalType ModalType { get; set; }

        [Parameter]
        public string ModalTitle { get; set; }

        [Parameter]
        public EventCallback<string> OnCloseModal { get; set; }

        [Parameter]
        public EventCallback<string> OnOpenModal { get; set; }

        protected override Task OnInitializedAsync()
        {
            //OpenModalCallback();
            if (ModalType == ModalType.Create)
            {
                liveStream.Url = $"http://{StreamServer.Address}:{StreamServer.Port}/hls/{StreamKey}.m3u8";
            }
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (firstRender) 
            //{
            //    await JSRuntime.InvokeVoidAsync("videoplayer.previewPlayer", "videoPlayer");
            //    StateHasChanged();
            //}
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task SubmitLiveStream()
        {
            liveStream.DatePublished = DateTime.UtcNow.ToLocalTime().ToString();
            var response = await Http.PostJsonAsync<LiveStream>("https://localhost:44354/api/livestreams", liveStream);
            Debug.WriteLine(response.ToString());
            Close();
        }

        protected async void InitializePreviewPlayer()
        {
            await JSRuntime.InvokeVoidAsync("previewPlayer", "previewPlayer");
        }

        protected async Task CopyToClipboard(MouseEventArgs e, string id)
        {
            await JSRuntime.InvokeVoidAsync("copyToClipboard", id);
            //await JSRuntime.InvokeVoidAsync("createAlert","Copied to clipboard");
        }

        protected void OpenModalCallback()
        {
            if (OnOpenModal.HasDelegate == true)
            {
                OnOpenModal.InvokeAsync("Add Live Stream Open Modal");
            }
        }

        protected void CloseModalCallback()
        {
            if (OnCloseModal.HasDelegate == true)
            {
                OnCloseModal.InvokeAsync("Add Live Stream Cancelled");

            }
        }

        protected void OnSubmit()
        {
            if (OnCloseModal.HasDelegate == true)
            {
                OnCloseModal.InvokeAsync("Submitted");
            }
        }
    }
}
