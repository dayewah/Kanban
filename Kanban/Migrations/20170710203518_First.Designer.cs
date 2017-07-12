using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Kanban.Model;

namespace Kanban.Migrations
{
    [DbContext(typeof(KanbanDbContext))]
    [Migration("20170710203518_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Kanban.Model.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Header");

                    b.Property<bool>("IsArchived");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("TaskItems");
                });
        }
    }
}
