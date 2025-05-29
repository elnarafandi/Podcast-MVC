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
    public class PodcastCategoryConfiguration:IEntityTypeConfiguration<PodcastCategory>
    {
        public void Configure(EntityTypeBuilder<PodcastCategory> builder)
        {
            builder.Property(t => t.Name)
               .IsRequired();
        }
    }
}
