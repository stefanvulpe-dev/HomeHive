﻿// <auto-generated />
using System;
using HomeHive.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    [DbContext(typeof(HomeHiveContext))]
    partial class HomeHiveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EstateUtility", b =>
                {
                    b.Property<Guid>("EstatesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UtilitiesId")
                        .HasColumnType("uuid");

                    b.HasKey("EstatesId", "UtilitiesId");

                    b.HasIndex("UtilitiesId");

                    b.ToTable("EstateUtility");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContractType")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EstateId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Estate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("EstateAvatar")
                        .HasColumnType("text");

                    b.Property<string>("EstateCategory")
                        .HasColumnType("text");

                    b.Property<string>("EstateType")
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("TotalArea")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Estates");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.EstatePhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EstateId");

                    b.ToTable("EstatePhotos");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.EstateRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EstateId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EstateId");

                    b.HasIndex("RoomId");

                    b.ToTable("EstateRoom");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoomType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Utility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UtilityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Utilities");
                });

            modelBuilder.Entity("EstateUtility", b =>
                {
                    b.HasOne("HomeHive.Domain.Entities.Estate", null)
                        .WithMany()
                        .HasForeignKey("EstatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeHive.Domain.Entities.Utility", null)
                        .WithMany()
                        .HasForeignKey("UtilitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Contract", b =>
                {
                    b.HasOne("HomeHive.Domain.Entities.Estate", "Estate")
                        .WithMany("Contracts")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estate");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.EstatePhoto", b =>
                {
                    b.HasOne("HomeHive.Domain.Entities.Estate", "Estate")
                        .WithMany("EstatePhotos")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estate");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.EstateRoom", b =>
                {
                    b.HasOne("HomeHive.Domain.Entities.Estate", "Estate")
                        .WithMany("EstateRooms")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeHive.Domain.Entities.Room", "Room")
                        .WithMany("EstateRooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estate");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Estate", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("EstatePhotos");

                    b.Navigation("EstateRooms");
                });

            modelBuilder.Entity("HomeHive.Domain.Entities.Room", b =>
                {
                    b.Navigation("EstateRooms");
                });
#pragma warning restore 612, 618
        }
    }
}
