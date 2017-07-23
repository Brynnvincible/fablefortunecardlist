using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FableFortuneCardList.Models;
using FableFortuneCardList.Models.UserManagerViewModels;

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
            
            base.OnModelCreating(builder);            
        }

        public DbSet<Card> Card { get; set; }

        public DbSet<Deck> Deck { get; set; }         
        
        public DbSet<DeckCard> DeckCard { get; set; }   
    }
}
