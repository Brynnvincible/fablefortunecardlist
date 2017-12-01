using FableFortuneCardList.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Models.DeckViewModels
{
    public class EditDeckViewModel
    {
        public Deck Deck { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeckType Type { get; set; }
        [DisplayName("Arena")]
        public DeckArenaCoop ArenaCoop { get; set; }
        [DisplayName("Arena")]
        public DeckArenaPVP ArenaPVP { get; set; }
        public ClassType Class { get; set; }
        public bool Completed { get; set; }
        public List<Card> AvailableCards { get; set; }
    }
}
