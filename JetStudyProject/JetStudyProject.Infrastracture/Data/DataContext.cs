﻿using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.Configurations;
using JetStudyProject.Infrastracture.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeyStudyProject.Infrastracture.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Basket>()
                .HasOne(x => x.User)
                .WithOne(x => x.Basket)
                .HasForeignKey<Basket>(x => x.UserId);
            builder.Entity<Basket>()
                .HasMany(x => x.BasketItems)
                .WithOne(x => x.Basket)
                .HasForeignKey(x => x.BasketId);
            new EventEntityTypeConfiguration().Configure(builder.Entity<Event>());
            builder.Seed();
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems{ get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ListenCourse> ListenCourses { get; set; }
        public DbSet<ReadCourse> ReadCourses { get; set; }
        public DbSet<StatusForAdministrator> StatusForAdministrators { get; set; }
        public DbSet<StatusForInstructor> StatusForInstructors { get; set; }
        public DbSet<StatusForStudent> StatusForStudents { get; set; }
    }
}
