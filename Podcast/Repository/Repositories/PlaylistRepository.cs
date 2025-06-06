using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PlaylistRepository:BaseRepository<Playlist>,IPlaylistRepository
    {
        public PlaylistRepository(AppDbContext context) : base(context) { }

        public async Task AddEpisodeToPlaylistAsync(int episodeId, int playlistId)
        {
            var playlistEpisode = new PlaylistEpisode
            {
                EpisodeId = episodeId,
                PlaylistId = playlistId
            };

            await _context.PlaylistEpisodes.AddAsync(playlistEpisode);
            await _context.SaveChangesAsync();
        }

        public async Task<Playlist> GetByIdAsync(int id)
        {
            return await _context.Set<Playlist>().Include(p=>p.PlaylistEpisodes).ThenInclude(e=>e.Episode).ThenInclude(eg=>eg.EpisodeGuests).ThenInclude(g=>g.Guest).FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(string userId)
        {
            return await _context.Set<Playlist>().Include(p => p.PlaylistEpisodes).Where(p => p.AppUserId == userId).ToListAsync();
        }

        public async Task<bool> IsEpisodeInPlaylistAsync(int episodeId, int playlistId)
        {
            return await _context.Set<PlaylistEpisode>().AnyAsync(pe => pe.EpisodeId == episodeId && pe.PlaylistId == playlistId);
        }
    }
}
