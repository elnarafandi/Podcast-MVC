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
    public class GuestConfiguration:IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.Property(t => t.FirstName)
               .IsRequired();
            builder.Property(t => t.LastName)
               .IsRequired();
            builder.Property(t => t.Email)
               .IsRequired();
            builder.Property(t => t.Image)
               .IsRequired();
            builder.Property(t => t.Information)
               .IsRequired();
            builder.Property(t => t.SocialMedia)
               .IsRequired();
        }
    }
}
