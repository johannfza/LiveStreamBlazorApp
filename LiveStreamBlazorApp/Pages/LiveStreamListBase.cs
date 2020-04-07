using MediaModelLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamListBase : ComponentBase
    {

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
    }
}
