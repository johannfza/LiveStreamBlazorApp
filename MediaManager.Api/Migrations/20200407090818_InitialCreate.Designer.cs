﻿// <auto-generated />
using MediaManager.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaManager.Api.Migrations
{
    [DbContext(typeof(MediaManagementApiContext))]
    [Migration("20200407090818_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MediaModelLibrary.LiveStream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DatePublished")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LiveStream");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DatePublished = "4/7/2020 5:08:17 PM",
                            Description = "This is Live Stream 1",
                            Title = "LiveStream 1",
                            Url = "http://www.golive.com/1",
                            Views = 0
                        },
                        new
                        {
                            Id = 2,
                            DatePublished = "4/7/2020 5:08:17 PM",
                            Description = "This is Live Stream 2",
                            Title = "LiveStream 2",
                            Url = "http://www.golive.com/2",
                            Views = 0
                        },
                        new
                        {
                            Id = 3,
                            DatePublished = "4/7/2020 5:08:17 PM",
                            Description = "This is Live Stream 3",
                            Title = "LiveStream 3",
                            Url = "http://www.golive.com/3",
                            Views = 0
                        },
                        new
                        {
                            Id = 4,
                            DatePublished = "4/7/2020 5:08:17 PM",
                            Description = "This is Live Stream 4",
                            Title = "LiveStream 4",
                            Url = "http://www.golive.com/4",
                            Views = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
