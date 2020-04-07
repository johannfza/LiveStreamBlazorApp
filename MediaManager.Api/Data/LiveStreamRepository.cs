using MediaModelLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaManager.Api.Data
{
    public class LiveStreamRepository : ILiveStreamRepository
    {
        private readonly MediaManagementApiContext apiDbContext;

        public LiveStreamRepository(MediaManagementApiContext apiContext)
        {
            this.apiDbContext = apiContext;
        }

        public async Task<LiveStream> AddLiveStream(LiveStream livestream)
        {
            var result = await apiDbContext.LiveStream.AddAsync(livestream);
            await apiDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async void DeleteLiveStream(int Id)
        {
            var result = await apiDbContext.LiveStream.FirstOrDefaultAsync(ls => ls.Id == Id);
            if (result != null)
            {
                apiDbContext.LiveStream.Remove(result);
                await apiDbContext.SaveChangesAsync();
            }
        }

        public async Task<LiveStream> GetLiveStream(int Id)
        {
            return await apiDbContext.LiveStream.FirstOrDefaultAsync(ls => ls.Id == Id);
        }

        public async Task<IEnumerable<LiveStream>> GetLiveStreams()
        {
            return await apiDbContext.LiveStream.ToListAsync();
        }

        public async Task<bool> LiveStreamExist(int Id)
        {
            return await apiDbContext.LiveStream.AnyAsync(ls => ls.Id == Id);
        }

        public async Task<LiveStream> UpdateLiveStream(LiveStream livestream)
        {
            var result = await apiDbContext.LiveStream.FirstOrDefaultAsync(ls => ls.Id == livestream.Id);
            if (result != null)
            {
                result.Title = livestream.Title;
                result.Description = livestream.Description;
                result.Url = livestream.Url;
                await apiDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
