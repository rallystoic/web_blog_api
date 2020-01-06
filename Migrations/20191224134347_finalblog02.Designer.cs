﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using finalBlog.Models;

namespace finalBlog.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20191224134347_finalblog02")]
    partial class finalblog02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("finalBlog.Models.category", b =>
                {
                    b.Property<int>("categoryid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("categoryname")
                        .HasColumnType("TEXT");

                    b.HasKey("categoryid");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("finalBlog.Models.content", b =>
                {
                    b.Property<int>("contentid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("contentdetail")
                        .HasColumnType("TEXT");

                    b.HasKey("contentid");

                    b.ToTable("contents");
                });

            modelBuilder.Entity("finalBlog.Models.highlight", b =>
                {
                    b.Property<int>("highlightid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("postcontentid")
                        .HasColumnType("INTEGER");

                    b.HasKey("highlightid");

                    b.HasIndex("postcontentid");

                    b.ToTable("highlights");
                });

            modelBuilder.Entity("finalBlog.Models.post", b =>
                {
                    b.Property<int>("postid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("posttitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("titleimgurl")
                        .HasColumnType("TEXT");

                    b.HasKey("postid");

                    b.ToTable("posts");
                });

            modelBuilder.Entity("finalBlog.Models.postcontent", b =>
                {
                    b.Property<int>("postcontentid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("categoryid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("contentid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("postcontentcreated")
                        .HasColumnType("TEXT");

                    b.Property<int>("postid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("userid")
                        .HasColumnType("INTEGER");

                    b.HasKey("postcontentid");

                    b.HasIndex("categoryid");

                    b.HasIndex("contentid");

                    b.HasIndex("postid");

                    b.HasIndex("userid");

                    b.ToTable("postcontents");
                });

            modelBuilder.Entity("finalBlog.Models.role", b =>
                {
                    b.Property<int>("roleid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("rolename")
                        .HasColumnType("TEXT");

                    b.HasKey("roleid");

                    b.ToTable("roles");

                    b.HasData(
                        new
                        {
                            roleid = 1,
                            rolename = "administrator"
                        },
                        new
                        {
                            roleid = 2,
                            rolename = "author"
                        });
                });

            modelBuilder.Entity("finalBlog.Models.user", b =>
                {
                    b.Property<int>("userid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("roleid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("userfirstname")
                        .HasColumnType("TEXT");

                    b.Property<string>("userhashed")
                        .HasColumnType("TEXT");

                    b.Property<string>("userlastname")
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .HasColumnType("TEXT");

                    b.Property<string>("usersalt")
                        .HasColumnType("TEXT");

                    b.HasKey("userid");

                    b.HasIndex("roleid");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            userid = 1,
                            roleid = 1,
                            userfirstname = "admin",
                            userhashed = "9DDbStYYrn8ExReQN62o5J3wpVZUztBVMoEFqW3Ko2A=",
                            userlastname = "admin",
                            username = "admin",
                            usersalt = "cztlN43JJ1nDn1NWuvAnpA=="
                        },
                        new
                        {
                            userid = 2,
                            roleid = 2,
                            userfirstname = "koala",
                            userhashed = "CTXXZzNjBghOuo1X2353N7VWJNysSVwcXR2+VsiZwuA=",
                            userlastname = "koalo",
                            username = "koala02",
                            usersalt = "vuc7ESG50L8bY9gUjvx9HQ=="
                        });
                });

            modelBuilder.Entity("finalBlog.Models.highlight", b =>
                {
                    b.HasOne("finalBlog.Models.postcontent", "postcontent")
                        .WithMany("highlights")
                        .HasForeignKey("postcontentid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("finalBlog.Models.postcontent", b =>
                {
                    b.HasOne("finalBlog.Models.category", "category")
                        .WithMany("postcontents")
                        .HasForeignKey("categoryid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("finalBlog.Models.content", "content")
                        .WithMany("postcontents")
                        .HasForeignKey("contentid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("finalBlog.Models.post", "post")
                        .WithMany("postcontents")
                        .HasForeignKey("postid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("finalBlog.Models.user", "user")
                        .WithMany("postcontents")
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("finalBlog.Models.user", b =>
                {
                    b.HasOne("finalBlog.Models.role", "role")
                        .WithMany("users")
                        .HasForeignKey("roleid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
