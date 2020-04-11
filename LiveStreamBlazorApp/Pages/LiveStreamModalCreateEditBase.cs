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

        [Parameter]
        public ModalType ModalType { get; set; }

        [Parameter]
        public string ModalTitle { get; set; }

        [Parameter]
        public EventCallback<string> OnCloseModal { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (ModalType == ModalType.Create)
            {
                liveStream.Url = $"http://{StreamServer.Address}:{StreamServer.Port}/hls/{StreamKey}.m3u8";
            }
            return base.OnInitializedAsync();
        }

        protected async Task SubmitLiveStream()
        {
            liveStream.DatePublished = DateTime.UtcNow.ToLocalTime().ToString();
            var response = await Http.PostJsonAsync<LiveStream>("https://localhost:44354/api/livestreams", liveStream);
            Debug.WriteLine(response.ToString());
            OnSubmit();
        }

        protected async Task CopyToClipboard(MouseEventArgs e, string id)
        {
            await JSRuntime.InvokeVoidAsync("copyToClipboard", id);
            //await JSRuntime.InvokeVoidAsync("createAlert","Copied to clipboard");
        }


        protected void CloseModal()
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
