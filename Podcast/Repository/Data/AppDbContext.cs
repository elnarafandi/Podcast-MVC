using Domain.Configurations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
        public DbSet<AppUserPodcast> AppUserPodcasts { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistEpisode> PlaylistEpisodes { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<PodcastCategory> PodcastCategories { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<EpisodeGuest> EpisodeGuests { get;set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserPodcastConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new EpisodeConfiguration());
            modelBuilder.ApplyConfiguration(new EpisodeGuestConfiguration());
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new PlaylistEpisodeConfiguration());
            modelBuilder.ApplyConfiguration(new PodcastCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PodcastConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMemberConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
