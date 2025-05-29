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
    public class EpisodeGuestConfiguration:IEntityTypeConfiguration<EpisodeGuest>
    {
        public void Configure(EntityTypeBuilder<EpisodeGuest> builder)
        {
            builder.Property(t => t.EpisodeId)
               .IsRequired();
            builder.Property(t => t.GuestId)
               .IsRequired();
        }
    }
}
