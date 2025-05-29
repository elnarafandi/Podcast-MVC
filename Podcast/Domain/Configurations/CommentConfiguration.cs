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
    public class CommentConfiguration:IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(t => t.Content)
               .IsRequired();
            builder.Property(t => t.AppUserId)
               .IsRequired();
            builder.Property(t => t.PodcastId)
               .IsRequired();
        }
    }
}
