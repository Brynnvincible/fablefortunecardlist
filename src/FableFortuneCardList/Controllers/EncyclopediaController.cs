using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FableFortuneCardList.Data;

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