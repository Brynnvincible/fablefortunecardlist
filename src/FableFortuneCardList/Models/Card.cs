using FableFortuneCardList.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Models
{
    public class Card
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public ClassType Class { get; set; }
        [Required]
        public RarityType Rarity { get; set; }
        [Required]
        public int Gold { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Strength { get; set; }
        [Required]
        public int Health { get; set; }
        public string Ability { get; set; }
        public string Transform { get; set; }
        public string TransformType { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string UnitClass { get; set; }
        public int SheetId { get; set; }
        public ICollection<DeckCard> DeckCards { get; set; }
        [NotMapped]
        public ICollection<Card> Transforms { get; set; }
    
        public Card()
        {
            Transforms = new HashSet<Card>();
            DeckCards = new HashSet<DeckCard>();
        }
    
    }
}
