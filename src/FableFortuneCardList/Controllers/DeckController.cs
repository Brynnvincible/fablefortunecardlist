using FableFortuneCardList.Data;
using FableFortuneCardList.Enums;
using FableFortuneCardList.Models;
using FableFortuneCardList.Models.DeckViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Controllers
{
    public class DeckController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;
        private IEnumerable<SelectListItem> _classList;
        private IEnumerable<SelectListItem> _deckTypeList;
        private IEnumerable<SelectListItem> _arenaCoopList;
        private IEnumerable<SelectListItem> _arenaPVPList;

        public DeckController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;

            // Expose Enums to views
            _classList = Enum.GetValues(typeof(ClassType)).Cast<ClassType>().Where(e => e != ClassType.Neutral && e != ClassType.Trophy && e != ClassType.Quest).Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString()
            });

            _deckTypeList = Enum.GetValues(typeof(DeckType)).Cast<DeckType>().Where(e => e != DeckType.None).Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString()
            });

            _arenaCoopList = Enum.GetValues(typeof(DeckArenaCoop)).Cast<DeckArenaCoop>().Where(e => e != DeckArenaCoop.None).Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString()
            });

            _arenaPVPList = Enum.GetValues(typeof(DeckArenaPVP)).Cast<DeckArenaPVP>().Where(e => e != DeckArenaPVP.None).Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString()
            });
        }
                  
        public async Task<IActionResult> Index()
        {
            List<Deck> deckList = _context.Deck.Include(x=>x.CreatedBy).Include(x=>x.DeckCards).ThenInclude(x=>x.Card).Include(x=>x.DeckRankings).ToList();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewData["currentUserId"] = user.Id;
            }
            return View(deckList);
        }

        public async Task<IActionResult> AvailableCards(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new EditDeckViewModel();

            viewModel.AvailableCards = _context.Card.ToList();

            var deck = await _context.Deck.Include(x => x.DeckCards).SingleOrDefaultAsync(m => m.ID == id);

            viewModel.ID = deck.ID;
            viewModel.Deck = deck;
            viewModel.Name = deck.Name;
            viewModel.Description = deck.Description;
            viewModel.Class = deck.Class;

            if (deck == null)
            {
                return NotFound();
            }

            return PartialView("DeckAvailableCards", viewModel);
        }
       
        public async Task<IActionResult> Details(int id)
        {
            var deck = await _context.Deck.Include(x=>x.CreatedBy).Include(x=>x.DeckCards).ThenInclude(x=>x.Card).FirstOrDefaultAsync(x => x.ID == id);

            return View(deck);
        }

        [HttpPost]
        [Authorize(Roles = "DeckMaster, Admin")]
        public async Task<IActionResult> AddCard(int deckId, int cardId)
        {
            var deckCard = new DeckCard();
            deckCard.DeckId = deckId;
            deckCard.CardId = cardId;

            _context.Add(deckCard);
            await _context.SaveChangesAsync();

            var deck = await _context.Deck.Include(x => x.DeckCards).ThenInclude(x => x.Card).FirstOrDefaultAsync(x => x.ID == deckId);

            return PartialView("DeckCardSummary", deck);
        }
        
        [HttpPost]
        [Authorize(Roles = "DeckMaster, Admin")]
        public async Task<IActionResult> RemoveCard(int deckCardId)
        {
            DeckCard deckCard = _context.DeckCard.SingleOrDefault(m => m.Id == deckCardId);

            _context.Remove(deckCard);
            await _context.SaveChangesAsync();

            var deck = await _context.Deck.Include(x => x.DeckCards).ThenInclude(x => x.Card).FirstOrDefaultAsync(x => x.ID == deckCard.DeckId);

            return PartialView("DeckCardSummary", deck);
        }

        public async Task<IActionResult> GoldSummary(int id)
        {
            var deck = await _context.Deck.Include(x => x.DeckCards).ThenInclude(x=>x.Card).FirstOrDefaultAsync(x => x.ID == id);

            return PartialView("DeckGoldSummary", deck);
        }

        [Authorize(Roles = "DeckMaster, Admin")]
        public IActionResult Create()
        {
            var deck = new Deck();

            ViewBag.ClassList = _classList;
            ViewBag.DeckTypeList = _deckTypeList;
            ViewBag.DeckArenaCoop = _arenaCoopList;
            ViewBag.DeckArenaPVP = _arenaPVPList;

            return View(deck);
        }

        [Authorize(Roles = "DeckMaster, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name, Description, Strategy, Type, ArenaCoop, ArenaPVP, Class")]Deck deck)
        {
            deck.CreatedBy = await _userManager.GetUserAsync(User);

            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                if (deck.Type == DeckType.Coop)
                {
                    deck.ArenaPVP = DeckArenaPVP.None;
                }
                else if (deck.Type == DeckType.PVP)
                {
                    deck.ArenaCoop = DeckArenaCoop.None;
                }
                deck.Strategy = deck.Strategy.Replace("<", "").Replace(">", "");
                _context.Add(deck);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { Id = deck.ID });
            }

            return View(deck);
        }

        [Authorize(Roles = "DeckMaster, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new EditDeckViewModel();

            viewModel.AvailableCards = _context.Card.ToList();

            var deck = await _context.Deck.Include(x=>x.DeckCards).SingleOrDefaultAsync(m => m.ID == id);

            if (deck == null)
            {
                return NotFound();
            }

            viewModel.ID = deck.ID;
            viewModel.Deck = deck;
            viewModel.Name = deck.Name;
            viewModel.Description = deck.Description;
            viewModel.Strategy = deck.Strategy;
            viewModel.Type = deck.Type;
            viewModel.ArenaCoop = deck.ArenaCoop;
            viewModel.ArenaPVP = deck.ArenaPVP;
            viewModel.Class = deck.Class;

            ViewBag.ClassList = _classList;
            ViewBag.DeckTypeList = _deckTypeList;
            ViewBag.DeckArenaCoop = _arenaCoopList;
            ViewBag.DeckArenaPVP = _arenaPVPList;

            return View(viewModel);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "DeckMaster, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Name, Description, Strategy, Type, ArenaCoop, ArenaPVP, Class")] Deck deck)
        {
            if (id != deck.ID)
            {
                return NotFound();
            }

            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                if (deck.Type == DeckType.Coop)
                {
                    deck.ArenaPVP = DeckArenaPVP.None;
                }
                else if (deck.Type == DeckType.PVP)
                {
                    deck.ArenaCoop = DeckArenaCoop.None;
                }
                deck.Strategy = deck.Strategy.Replace("<", "").Replace(">", "");
                try
                {
                    _context.Update(deck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeckExists(deck.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            var viewModel = new EditDeckViewModel();

            viewModel.AvailableCards = _context.Card.ToList();

            viewModel.ID = deck.ID;
            viewModel.Deck = deck;
            viewModel.Name = deck.Name;
            viewModel.Description = deck.Description;
            viewModel.Strategy = deck.Strategy;
            viewModel.Class = deck.Class;

            return View(viewModel);
        }


        [Authorize(Roles = "DeckMaster, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _context.Deck.SingleOrDefaultAsync(m => m.ID == id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }


        [Authorize(Roles = "DeckMaster, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deck = await _context.Deck.SingleOrDefaultAsync(m => m.ID == id);
            var deckCards = await _context.DeckCard.Where(x => x.DeckId == deck.ID).ToListAsync();

            _context.DeckCard.RemoveRange(deckCards);
            _context.Deck.Remove(deck);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DeckExists(int id)
        {
            return _context.Deck.Any(e => e.ID == id);
        }

        [HttpGet]
        public IActionResult Vote()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int deckId, int voteType)
        {
            var user = await _userManager.GetUserAsync(User);
            var existingDeckRanking = _context.DeckRanking.FirstOrDefault(x => x.UserID == user.Id && x.DeckID == deckId);
            if(existingDeckRanking != null)
            {
                if (existingDeckRanking.Ranking == voteType)
                {
                    // Ranking has not changed
                    return Json(false);
                }
                else
                {
                    // Update existing ranking to new value
                    _context.DeckRanking.First(x => x.Id == existingDeckRanking.Id).Ranking = voteType;
                    await _context.SaveChangesAsync();
                    return Json(true);
                }
            }
            else
            {
                // New deck ranking
                var deckRanking = new DeckRanking();
                deckRanking.DeckID = deckId;
                deckRanking.UserID = user.Id;
                deckRanking.Ranking = voteType;

                _context.Add(deckRanking);
                await _context.SaveChangesAsync();

                return Json(true);
            }
        }
    }
}