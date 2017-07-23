using FableFortuneCardList.Data;
using FableFortuneCardList.Enums;
using FableFortuneCardList.Models;
using FableFortuneCardList.Models.DeckViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IViewComponentResult> InvokeAsync(int deckId, int cardId)
        {
            Deck deck = await _context.Deck.FirstOrDefaultAsync(x => x.ID == deckId);
            Card card = await _context.Card.FirstOrDefaultAsync(x => x.ID == cardId);
            var isDisabled = false;

            if ((deck.DeckCards.Count(x => x.CardId == card.ID) >= 2 || (deck.DeckCards.Count(x => x.Card.Class != ClassType.Trophy) >= 30)) ||
                      (deck.DeckCards.Count(x => x.Card.Class == ClassType.Trophy) >= 1 && card.Class == ClassType.Trophy) ||
                        (deck.DeckCards.Count(x => x.Card.Name == card.Name) >= 1 && card.Rarity == RarityType.Fabled))
            {
                isDisabled = true;
            }

            if(deck.DeckCards.Count(x => x.Card.Class == ClassType.Trophy) < 1 && card.Class == ClassType.Trophy)
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
