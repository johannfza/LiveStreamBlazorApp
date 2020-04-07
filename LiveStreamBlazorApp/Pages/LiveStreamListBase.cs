using MediaModelLibrary;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveStreamBlazorApp.Pages
{
    public class LiveStreamListBase : ComponentBase
    {
        public List<LiveStream> LiveStreamList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(LoadList); 
        }

        private void LoadList()
        {
            System.Threading.Thread.Sleep(3000);
            LiveStreamList = new List<LiveStream>();


        }


    }
}
