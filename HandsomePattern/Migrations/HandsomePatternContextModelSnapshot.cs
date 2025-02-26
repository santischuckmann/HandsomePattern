﻿// <auto-generated />
using HandsomePattern.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HandsomePattern.Migrations
{
    [DbContext(typeof(HandsomePatternContext))]
    partial class HandsomePatternContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HandsomePattern.Entitys.DependencyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DependencyType", (string)null);
                });

            modelBuilder.Entity("HandsomePattern.Entitys.FileDB", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileId"));

                    b.Property<int>("DependencyTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PathsToFile")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Template")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("nvarchar(MAX)");

                    b.HasKey("FileId");

                    b.HasIndex("DependencyTypeId");

                    b.ToTable("File", (string)null);
                });

            modelBuilder.Entity("HandsomePattern.Entitys.FileDB", b =>
                {
                    b.HasOne("HandsomePattern.Entitys.DependencyType", "DependencyType")
                        .WithMany("Files")
                        .HasForeignKey("DependencyTypeId")
                        .IsRequired();

                    b.Navigation("DependencyType");
                });

            modelBuilder.Entity("HandsomePattern.Entitys.DependencyType", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
