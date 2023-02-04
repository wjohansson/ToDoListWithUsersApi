﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoListWithUsersApi;

#nullable disable

namespace ToDoListWithUsersApi.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataLibrary.Models.CategoryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SortLists")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("DataLibrary.Models.SubTaskModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("SubTasks", (string)null);
                });

            modelBuilder.Entity("DataLibrary.Models.TaskListModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SortTasks")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskLists", (string)null);
                });

            modelBuilder.Entity("DataLibrary.Models.TaskModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("SortSubTasks")
                        .HasColumnType("int");

                    b.Property<Guid>("TaskListId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TaskListId");

                    b.ToTable("Tasks", (string)null);
                });

            modelBuilder.Entity("DataLibrary.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Age")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OldPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int?>("Permission")
                        .HasColumnType("int");

                    b.Property<int>("SortCategories")
                        .HasColumnType("int");

                    b.Property<int>("SortLists")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("DataLibrary.Models.CategoryModel", b =>
                {
                    b.HasOne("DataLibrary.Models.UserModel", null)
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLibrary.Models.SubTaskModel", b =>
                {
                    b.HasOne("DataLibrary.Models.TaskModel", null)
                        .WithMany("SubTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLibrary.Models.TaskListModel", b =>
                {
                    b.HasOne("DataLibrary.Models.CategoryModel", null)
                        .WithMany("TaskLists")
                        .HasForeignKey("CategoryId");

                    b.HasOne("DataLibrary.Models.UserModel", null)
                        .WithMany("TaskLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLibrary.Models.TaskModel", b =>
                {
                    b.HasOne("DataLibrary.Models.TaskListModel", null)
                        .WithMany("Tasks")
                        .HasForeignKey("TaskListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLibrary.Models.CategoryModel", b =>
                {
                    b.Navigation("TaskLists");
                });

            modelBuilder.Entity("DataLibrary.Models.TaskListModel", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("DataLibrary.Models.TaskModel", b =>
                {
                    b.Navigation("SubTasks");
                });

            modelBuilder.Entity("DataLibrary.Models.UserModel", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("TaskLists");
                });
#pragma warning restore 612, 618
        }
    }
}
