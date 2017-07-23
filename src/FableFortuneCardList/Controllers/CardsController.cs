using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FableFortuneCardList.Data;
using FableFortuneCardList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace FableFortuneCardList.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public CardsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Cards
        public async Task<IActionResult> Index()
        {
            List<Card> cardList = _context.Card.ToList();

            var transformCards = cardList.Where(c => c.Transform != null).ToList();

            foreach(var childCard in transformCards)
            {
                //assign to parent
                Card parentCard = cardList.Where(x => x.Name == childCard.Transform).SingleOrDefault();
                
                parentCard.Transforms.Add(childCard);
            }

            foreach(var card in cardList.Where(x=> string.IsNullOrEmpty(x.ImageUrl)))
            {
                var filePath = Path.Combine(_environment.WebRootPath,
                        "images", "cards", card.Name.Replace(' ', '_') + ".png");

                if (System.IO.File.Exists(filePath))
                {
                    card.ImageUrl = filePath;
                    _context.Card.Update(card);                    
                }
            }
            _context.SaveChanges();

            return View(cardList);
        }

        public async Task<IActionResult> GridView()
        {
            List<Card> cardList = await _context.Card.ToListAsync();
            
            return View(cardList);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var name = id.Replace("_", " ");

            var card = await _context.Card.SingleOrDefaultAsync(m => m.Name == name);

            card.Transforms = await _context.Card.Where(x => x.Transform == card.Name).ToListAsync();

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        [Authorize(Roles = "Admin")]
        // GET: Cards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ability,Class,Gold,Health,Name,Rarity,Strength")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(card);
        }

        [Authorize(Roles = "Admin")]
        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card.SingleOrDefaultAsync(m => m.ID == id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ability,Class,Gold,Health,Name,Rarity,Strength")] Card card)
        {
            if (id != card.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.ID))
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
            return View(card);
        }

        // GET: Cards/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card.SingleOrDefaultAsync(m => m.ID == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Card.SingleOrDefaultAsync(m => m.ID == id);
            _context.Card.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CardExists(int id)
        {
            return _context.Card.Any(e => e.ID == id);
        }
    }
}
