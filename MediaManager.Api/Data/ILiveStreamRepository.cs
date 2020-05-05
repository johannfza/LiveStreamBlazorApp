using MediaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaManager.Api.Data
{
    public interface ILiveStreamRepository
    {
        Task<IEnumerable<LiveStream>> GetLiveStreams();
        Task<LiveStream> GetLiveStream(int Id);
        Task<LiveStream> AddLiveStream(LiveStream livestream);
        Task<LiveStream> UpdateLiveStream(LiveStream livestream);
        void DeleteLiveStream(int Id);
        Task<bool> LiveStreamExist(int Id);
       
    }
}
