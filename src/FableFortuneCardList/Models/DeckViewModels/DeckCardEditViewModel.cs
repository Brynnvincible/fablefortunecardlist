using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Models.DeckViewModels
{
    public class DeckCardEditViewModel
    {
        public int DeckCardID { get; set; }
        public Card Card { get; set; }
        public int CardCount { get; set; }
    }
}
