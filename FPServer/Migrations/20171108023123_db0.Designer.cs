﻿// <auto-generated />
using FPServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace FPServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20171108023123_db0")]
    partial class db0
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("FPServer.Models.AppConfigModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("AppConfig");
                });

            modelBuilder.Entity("FPServer.Models.BasicApplicationModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("BasicApplication");
                });

            modelBuilder.Entity("FPServer.Models.StorageModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("StorageModel");
                });

            modelBuilder.Entity("FPServer.Models.TestModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("TestModel");
                });

            modelBuilder.Entity("FPServer.Models.UserModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EXT");

                    b.Property<string>("LID")
                        .IsRequired();

                    b.Property<string>("PWD")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FPServer.Models.UserRecordModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("LID")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("UserRecord");
                });
#pragma warning restore 612, 618
        }
    }
}
