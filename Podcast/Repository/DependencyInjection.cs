using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories.Interfaces;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IPodcastRepository, PodcastRepository>();
            services.AddScoped<IPodcastCategoryRepository, PodcastCategoryRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<IAppUserPodcastRepository, AppUserPodcastRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IPlaylistEpisodeRepository, PlaylistEpisodeRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            return services;
        }
    }
}
