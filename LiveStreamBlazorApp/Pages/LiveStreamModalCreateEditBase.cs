using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaModels;
using System.Net.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using LiveStreamBlazorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamModalCreateEditBase : ComponentBase
    {

        //Media Database Server 
        

        //StreamServer
        protected static string ServerAddress = "139.162.24.99";
        protected static string Port = "443";
        protected static string AppName = "live";
        protected static ServerApplication StreamServer = new ServerApplication(ServerAddress, Port, AppName);


        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public string StreamUrl = $"rtmp://{StreamServer.Address}/{StreamServer.AppName}";
        [Parameter]
        [Required(ErrorMessage = "A stream key is required")]
        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string StreamKey { get; set; } = "";
        public LiveStream liveStream = new LiveStream();
        protected ElementReference previewPlayer;

        //Modal UI
        public string ModalDisplay = "none";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

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
            return base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) 
            {
                //await JSRuntime.InvokeVoidAsync("videoplayer.previewPlayer", "previewPlayer");
                //await JSRuntime.InvokeVoidAsync("webcam.oninit");
                //StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Open()
        {
            ModalDisplay = "block";
            ModalClass = "Show";
            ShowBackdrop = true;
            StreamKey = Guid.NewGuid().ToString();
            liveStream = new LiveStream();
            liveStream.Url = $"https://{StreamServer.Address}:{StreamServer.Port}/hls/{StreamKey}.m3u8";
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }

        protected async Task SubmitLiveStream()
        {
            liveStream.DatePublished = DateTime.UtcNow.ToLocalTime().ToString();
            var response = await Http.PostJsonAsync<LiveStream>("https://localhost:5001/api/livestreams", liveStream);
            Debug.WriteLine(response.ToString());
            Close();
        }

        protected async void Load()
        {
            await JSRuntime.InvokeVoidAsync("loadPlayer", "previewPlayer", liveStream.Url);
        }

        protected async Task CopyToClipboard(MouseEventArgs e, string id)
        {
            await JSRuntime.InvokeVoidAsync("copyToClipboard", id);
            //await JSRuntime.InvokeVoidAsync("createAlert","Copied to clipboard");
        }

        protected void UpdateStreamUrl(ChangeEventArgs args)
        {
            StreamKey = args.Value.ToString();
            liveStream.Url = $"https://{StreamServer.Address}:{StreamServer.Port}/hls/{StreamKey}.m3u8";
            StateHasChanged();
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
