using FableFortuneCardList.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FableFortuneCardList.Models
{
    public class Deck
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ClassType Class { get; set; }
        public string Description { get; set; }
        public string Strategy { get; set; }
        public DeckType Type { get; set; }
        [DisplayName("Arena")]
        public DeckArenaPVP ArenaPVP { get; set; }
        [DisplayName("Arena")]
        public DeckArenaCoop ArenaCoop { get; set; }
        public bool Completed { get; set; }
        public bool Private { get; set; }
        public string GameVersion { get; set; }
        public ICollection<DeckCard> DeckCards { get; set; }
        public ICollection<DeckRanking> DeckRankings { get; set; }
        [Required]
        public ApplicationUser CreatedBy { get; set; } 
        
        public Deck()
        {
            this.DeckCards = new HashSet<DeckCard>();
            this.DeckRankings = new HashSet<DeckRanking>();
        }

        public int GetInkCost()
        {
            int totalCost = 0;
            foreach(DeckCard dc in DeckCards)
            {
                totalCost += StandardInk.GetInkCost(dc.Card.Rarity);
            }
            return totalCost;
        }
    }
}
