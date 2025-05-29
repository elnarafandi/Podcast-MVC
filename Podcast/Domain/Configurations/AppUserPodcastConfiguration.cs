using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class AppUserPodcastConfiguration:IEntityTypeConfiguration<AppUserPodcast>
    {
        public void Configure(EntityTypeBuilder<AppUserPodcast> builder)
        {
            builder.Property(t => t.AppUserId)
               .IsRequired();
            builder.Property(t => t.PodcastId)
               .IsRequired();
        }
    }
}
