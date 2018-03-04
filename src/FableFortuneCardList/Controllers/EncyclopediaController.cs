using FableFortuneCardList.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FableFortuneCardList.Controllers
{
    public class EncyclopediaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncyclopediaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Card.ToList());
        }

    }
}