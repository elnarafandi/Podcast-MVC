using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories.Interfaces;
using Service.Services;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IPodcastService,PodcastService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPodcastCategoryService, PodcastCategoryService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<IAppUserPodcastService, AppUserPodcastService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IPlaylistEpisodeService, PlaylistEpisodeService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
