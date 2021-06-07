﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WetalkAPI.Helpers;

namespace WetalkAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210607192513_ChatEntities")]
    partial class ChatEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WetalkAPI.Entities.Chat", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("WetalkAPI.Entities.ChatOwner", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("ChatID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "ChatID");

                    b.HasIndex("ChatID");

                    b.ToTable("ChatOwners");
                });

            modelBuilder.Entity("WetalkAPI.Entities.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ChatID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SenderID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ChatID");

                    b.HasIndex("SenderID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WetalkAPI.Entities.MessageRead", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("MessageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReadAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID", "MessageID");

                    b.HasIndex("MessageID");

                    b.ToTable("MessagesReads");
                });

            modelBuilder.Entity("WetalkAPI.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("PermissionID")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("PermissionID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WetalkAPI.Entities.UserFile", b =>
                {
                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FileName");

                    b.HasIndex("UserID");

                    b.ToTable("UserFiles");
                });

            modelBuilder.Entity("WetalkAPI.Entities.UserPermission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("UserPermissions");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Description = "Admin"
                        },
                        new
                        {
                            ID = 2,
                            Description = "User"
                        });
                });

            modelBuilder.Entity("WetalkAPI.Entities.ChatOwner", b =>
                {
                    b.HasOne("WetalkAPI.Entities.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WetalkAPI.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WetalkAPI.Entities.Message", b =>
                {
                    b.HasOne("WetalkAPI.Entities.Chat", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatID");

                    b.HasOne("WetalkAPI.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("WetalkAPI.Entities.MessageRead", b =>
                {
                    b.HasOne("WetalkAPI.Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WetalkAPI.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WetalkAPI.Entities.User", b =>
                {
                    b.HasOne("WetalkAPI.Entities.UserPermission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("WetalkAPI.Entities.UserFile", b =>
                {
                    b.HasOne("WetalkAPI.Entities.User", "User")
                        .WithMany("Files")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WetalkAPI.Entities.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("WetalkAPI.Entities.User", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
