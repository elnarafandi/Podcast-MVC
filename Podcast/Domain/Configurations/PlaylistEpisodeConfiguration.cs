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
    public class PlaylistEpisodeConfiguration:IEntityTypeConfiguration<PlaylistEpisode>
    {
        public void Configure(EntityTypeBuilder<PlaylistEpisode> builder)
        {
            builder.Property(t => t.PlaylistId)
               .IsRequired();
            builder.Property(t => t.EpisodeId)
               .IsRequired();
        }
    }
}
