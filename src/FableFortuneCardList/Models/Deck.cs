using FableFortuneCardList.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        public ICollection<DeckCard> DeckCards { get; set; }
        [Required]
        public ApplicationUser CreatedBy { get; set; } 
        
        public Deck()
        {
            this.DeckCards = new HashSet<DeckCard>();
        }
    }
}
