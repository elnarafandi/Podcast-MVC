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
    public class PodcastConfiguration:IEntityTypeConfiguration<Podcast>
    {
        public void Configure(EntityTypeBuilder<Podcast> builder)
        {
            builder.Property(t => t.Title)
               .IsRequired();
            builder.Property(t => t.Description)
               .IsRequired();
            builder.Property(t => t.Image)
               .IsRequired();
            builder.Property(t => t.TeamMemberId)
               .IsRequired();
            builder.Property(t => t.PodcastCategoryId)
               .IsRequired();
        }
    }
}
