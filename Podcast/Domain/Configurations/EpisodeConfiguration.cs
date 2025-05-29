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
    public class EpisodeConfiguration:IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.Property(t => t.Title)
               .IsRequired();
            builder.Property(t => t.Description)
               .IsRequired();
            builder.Property(t => t.Image)
               .IsRequired();
            builder.Property(t => t.Audio)
               .IsRequired();
            builder.Property(t => t.PodcastId)
               .IsRequired();
        }
    }
}
