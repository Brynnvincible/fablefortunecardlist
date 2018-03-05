using FableFortuneCardList.Data;
using FableFortuneCardList.Enums;
using FableFortuneCardList.Models;
using FableFortuneCardList.Models.DeckViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.ViewComponents
{
    public class DeckAddCardButtonViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DeckAddCardButtonViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int deckId, int cardId, ClassType cardClass, string cardName, RarityType cardRarity)
        {
            Deck deck = await _context.Deck.FirstOrDefaultAsync(x => x.ID == deckId);            
            var isDisabled = false;

            if ((deck.DeckCards.Count(x => x.CardId == cardId) >= 2 || (deck.DeckCards.Count(x => x.Card.Class != ClassType.Trophy) >= 30)) ||
                      (deck.DeckCards.Count(x => x.Card.Class == ClassType.Trophy) >= 1 && cardClass == ClassType.Trophy) ||
                        (deck.DeckCards.Count(x => x.Card.Name == cardName) >= 1 && cardRarity == RarityType.Fabled))
            {
                isDisabled = true;
            }

            if(deck.DeckCards.Count(x => x.Card.Class == ClassType.Trophy) < 1 && cardClass == ClassType.Trophy)
            {
                isDisabled = false;
            }

            var viewModel = new DeckAddCardButtonViewModel();
            viewModel.CardID = cardId;
            viewModel.IsDisabled = isDisabled;
            return View(viewModel);
        }
    }
}
