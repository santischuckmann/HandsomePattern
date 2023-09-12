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
    public class FileConfiguration : IEntityTypeConfiguration<FileDB>
    {
        public void Configure(EntityTypeBuilder<FileDB> builder)
        {
            builder.ToTable("File");

            builder.HasKey(d => d.FileId);

            builder.Property(d => d.Filename)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.PathsToFile)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(d => d.Template)
                .HasColumnType("nvarchar(MAX)")
                .IsRequired()
                .IsUnicode(false);

            builder.HasOne(f => f.DependencyType)
                .WithMany(d => d.Files)
                .HasForeignKey(f => f.DependencyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
