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
    public class PlaylistEpisodeRepository:BaseRepository<PlaylistEpisode>,IPlaylistEpisodeRepository
    {
        public PlaylistEpisodeRepository(AppDbContext context) : base(context) { }

        public async Task<PlaylistEpisode> GetPlaylistEpisodeAsync(int playlistId, int episodeId)
        {
            return await _context.Set<PlaylistEpisode>().FirstOrDefaultAsync(pe => pe.PlaylistId == playlistId && pe.EpisodeId == episodeId);
        }
    }
}
