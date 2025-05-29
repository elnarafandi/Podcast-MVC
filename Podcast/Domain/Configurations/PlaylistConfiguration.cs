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
    public class PlaylistConfiguration:IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.Property(t => t.Name)
               .IsRequired();
            builder.Property(t => t.AppUserId)
               .IsRequired();
        }
    }
}
