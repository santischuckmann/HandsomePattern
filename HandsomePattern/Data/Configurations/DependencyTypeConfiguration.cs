using HandsomePattern.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern.Data.Configurations
{
    public class DependencyTypeConfiguration : IEntityTypeConfiguration<DependencyType>
    {
        public void Configure(EntityTypeBuilder<DependencyType> builder)
        {
            builder.ToTable("DependencyType");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
