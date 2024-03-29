﻿using FableFortuneCardList.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FableFortuneCardList.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DeckCard>().HasKey(x => new { x.Id });

            builder.Entity<DeckCard>()
                .HasOne(x => x.Card)
                .WithMany(x => x.DeckCards)
                .HasForeignKey(x => x.CardId);

            builder.Entity<DeckCard>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.DeckCards)
                .HasForeignKey(x => x.DeckId);

            builder.Entity<DeckRanking>().HasKey(x => new { x.Id });

            builder.Entity<DeckRanking>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.DeckRankings)
                .HasForeignKey(x => x.DeckID);

            builder.Entity<DeckRanking>()
                .HasOne(x => x.User)
                .WithMany(x => x.DeckRankings)
                .HasForeignKey(x => x.UserID);
            
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(e => e.Users)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Card> Card { get; set; }

        public DbSet<Deck> Deck { get; set; }         
        
        public DbSet<DeckCard> DeckCard { get; set; }   

        public DbSet<Comment> Comment { get; set; }

        public DbSet<CommentThread> CommentThread { get; set; }

        public DbSet<DeckRanking> DeckRanking { get; set; }
    }
}
