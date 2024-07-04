﻿using Book.Archieve.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Infrastructure.Context
{

    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>()
                       .ToTable("User")
                       .HasMany(u => u.Friends)
                       .WithMany()
                       .UsingEntity<UserFriend>(
                           j => j
                               .HasOne(uf => uf.Friend)
                               .WithMany()
                               .HasForeignKey(uf => uf.FriendId),
                           j => j
                               .HasOne(uf => uf.User)
                               .WithMany()
                               .HasForeignKey(uf => uf.UserId),
                           j =>
                           {
                               j.HasKey(t => new { t.UserId, t.FriendId });
                               j.ToTable("FriendShip");
                           });
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserFriend> FriendShip { get; set; }
        public DbSet<Domain.Entity.Book> Book { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Author> Author { get; set; }
       
    }
}
