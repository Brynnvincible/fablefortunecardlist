using FableFortuneCardList.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FableFortuneCardList.Models
{
    public class Card : ICloneable
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
        public int Evolves { get; set; }
        public string Associated { get; set; }
        public ICollection<DeckCard> DeckCards { get; set; }
        [NotMapped]
        public ICollection<Card> Transforms { get; set; }
        [NotMapped]
        public ICollection<Card> AssociatedCards { get; set; }
    
        public static IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                if (int.TryParse(s, out int num))
                    yield return num;
            }
        }

        public Card()
        {
            Transforms = new HashSet<Card>();
            DeckCards = new HashSet<DeckCard>();
            AssociatedCards = new HashSet<Card>();
        }

        public Card Clone()
        {
            Card newCard = new Card();
            newCard.Class = this.Class;
            newCard.Rarity = this.Rarity;
            newCard.Gold = this.Gold;
            newCard.Name = this.Name;
            newCard.Strength = this.Strength;
            newCard.Health = this.Health;
            newCard.Ability = this.Ability;
            newCard.Type = this.Type;
            newCard.ImageUrl = this.ImageUrl;
            newCard.UnitClass = this.UnitClass;
            newCard.SheetId = this.SheetId;
            newCard.Evolves = this.Evolves;
            newCard.Associated = this.Associated;

            return newCard;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    
    }
}
