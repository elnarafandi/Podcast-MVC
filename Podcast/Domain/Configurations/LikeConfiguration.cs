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
    public class LikeConfiguration:IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.Property(t => t.AppUserId)
               .IsRequired();
            builder.Property(t => t.EpisodeId)
               .IsRequired();
        }
    }
}
