﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NA5.Models
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatContext()
        {
        }
        public ChatContext(DbContextOptions dbc) : base(dbc)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=WIN-Q595KQ6H928\\SQLEXPRESS;Database=HW5;Integrated Security=False;TrustServerCertificate=True;Trusted_Connection=True;").UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id).HasName("userPk");
                entity.HasIndex(x => x.FullName).IsUnique();

                entity.Property(e => e.FullName).HasColumnName("FullName").HasMaxLength(255);
            }
            );
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.HasKey(x => x.MessageId).HasName("messagePk");

                entity.Property(e => e.Text).HasColumnName("messageText");
                entity.Property(e => e.DateSend).HasColumnName("messageDate");
                entity.Property(e => e.IsSent).HasColumnName("isSent");
                entity.Property(e => e.MessageId).HasColumnName("id");

                entity.HasOne(x => x.UserTo).WithMany(m => m.MessagesTo).HasForeignKey(x => x.UserToId).HasConstraintName("messageToUserFK");
                entity.HasOne(x => x.UserFrom).WithMany(m => m.MessagesFrom).HasForeignKey(x => x.UserFromId).HasConstraintName("messageFromUserFK");
            }
            );
        }
    }
}
