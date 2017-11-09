using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using FableFortuneCardList.Models;
using FableFortuneCardList.Data;
using FableFortuneCardList.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO.Compression;

namespace FableFortuneCardList.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;


        public ImportController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
        }

        //public async Task<IActionResult> DeleteAll()
        //{
        //    _context.Card.RemoveRange(_context.Card.ToList());
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", "Cards");
        //}

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult UploadImages()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(IFormFile file)
        {
            if(file.Length > 0)
            {
                var filename = Path.Combine(_env.WebRootPath, @"Imports\", file.FileName);
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var directoryInfo = new DirectoryInfo(Path.Combine(_env.WebRootPath, @"images\cards"));

                foreach(FileInfo imageFile in directoryInfo.GetFiles())
                {
                    imageFile.Delete();
                }
                
                using (ZipArchive za = ZipFile.OpenRead(filename))
                {
                    za.ExtractToDirectory(Path.Combine(_env.WebRootPath, @"images\Cards"));
                                        
                    foreach(FileInfo imageFile in directoryInfo.GetFiles())
                    {
                        var oldName = imageFile.Name;
                        var extension = imageFile.Extension;
                        var newName = oldName.Split('_')[0].Replace('-', '_') + extension;
                        imageFile.MoveTo(Path.Combine(_env.WebRootPath, @"images\Cards", newName));
                    }
                }
            }

            return RedirectToAction("Index", "Cards");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(_env.WebRootPath, @"Imports\", file.FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    
                    var package = new ExcelPackage(fileStream);

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                    _context.DeckCard.RemoveRange(_context.DeckCard.ToList());
                    _context.Deck.RemoveRange(_context.Deck.ToList());
                    _context.Card.RemoveRange(_context.Card.ToList());
                    _context.SaveChanges();

                    for (int j = workSheet.Dimension.Start.Row + 1; j <= workSheet.Dimension.End.Row; j++)
                    {
                        var row = workSheet.Row(j);

                        object classValue = workSheet.Cells[j, 1].Value;
                        object goldValue = workSheet.Cells[j, 2].Value;
                        object transformValue = workSheet.Cells[j, 3].Value;
                        object transformTypeValue = workSheet.Cells[j, 4].Value;
                        object nameValue = workSheet.Cells[j, 5].Value;
                        object strengthValue = workSheet.Cells[j, 6].Value;
                        object healthValue = workSheet.Cells[j, 7].Value;
                        object abilityValue = workSheet.Cells[j, 8].Value;
                        object rarityValue = workSheet.Cells[j, 9].Value;

                        if (goldValue == null || classValue == null || rarityValue == null)
                        {
                            continue;
                        }

                        var card = new Card();
                        card.Class = (ClassType)Enum.Parse(typeof(ClassType), classValue.ToString());
                        card.Rarity = (RarityType)Enum.Parse(typeof(RarityType), rarityValue.ToString().Replace('+',' '));
                        card.Name = nameValue.ToString();
                        card.Gold = int.Parse(goldValue.ToString());
                        card.Strength = strengthValue == null ? 0 : int.Parse(strengthValue.ToString());
                        card.Health = healthValue == null ? 0 : int.Parse(healthValue.ToString());
                        card.Ability = abilityValue == null ? null : abilityValue.ToString();
                        card.Transform = transformValue == null ? null : transformValue.ToString();
                        card.TransformType = transformTypeValue == null ? null : transformTypeValue.ToString();

                        if (_context.Card.Where(x => x.Name == card.Name).Any())
                        {
                            continue;
                        }

                        _context.Add(card);
                        await _context.SaveChangesAsync();
                    }                 
                }
            }
            return RedirectToAction("Index", "Cards");
        }

        [HttpGet]
        public async Task<IActionResult> Export()
        {
            ICollection<Card> cards = _context.Card.ToList();

            var fileName = @"c:\temp\CardListExport_{0}.xlsx";
            FileInfo newFile = new FileInfo(string.Format(fileName, DateTime.Now.ToString("ddMMyyyyhhmmss")));
            
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Cards");
                worksheet.Cells["A1"].LoadFromCollection(cards, true);
                var stream = new MemoryStream();
                pck.Save();
            }

            return RedirectToAction("Index", "cards");
        }

        //public async Task<IActionResult> Update()
        //{
        //    var excelFile = new FileInfo(Path.Combine(_env.WebRootPath, @"Imports\Fable Fortune Complete Card List.xlsx"));
        //    var package = new ExcelPackage(excelFile);

        //    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

        //    for (int j = workSheet.Dimension.Start.Row + 1; j <= workSheet.Dimension.End.Row; j++)
        //    {
        //        var row = workSheet.Row(j);

        //        object classValue = workSheet.Cells[j, 1].Value;
        //        object goldValue = workSheet.Cells[j, 2].Value;
        //        object transformValue = workSheet.Cells[j, 3].Value;
        //        object transformTypeValue = workSheet.Cells[j, 4].Value;
        //        object nameValue = workSheet.Cells[j, 5].Value;
        //        object strengthValue = workSheet.Cells[j, 6].Value;
        //        object healthValue = workSheet.Cells[j, 7].Value;
        //        object abilityValue = workSheet.Cells[j, 8].Value;
        //        object rarityValue = workSheet.Cells[j, 9].Value;

        //        if (goldValue == null || classValue == null || rarityValue == null)
        //        {
        //            continue;
        //        }

        //        var card = new Card();
        //        card.Class = (ClassType)Enum.Parse(typeof(ClassType),classValue.ToString());
        //        card.Rarity = (RarityType)Enum.Parse(typeof(RarityType), rarityValue.ToString());
        //        card.Name = nameValue.ToString();
        //        card.Gold = int.Parse(goldValue.ToString());
        //        card.Strength = strengthValue == null ? 0 : int.Parse(strengthValue.ToString());
        //        card.Health = healthValue == null ? 0 : int.Parse(healthValue.ToString());
        //        card.Ability = abilityValue == null ? null : abilityValue.ToString();
        //        card.Transform = transformValue == null ? null : transformValue.ToString();
        //        card.TransformType = transformTypeValue == null ? null : transformTypeValue.ToString();

        //        if (_context.Card.Where(x => x.Name == card.Name).Any())
        //        {
        //            continue;
        //        }

        //        _context.Add(card);
        //        await _context.SaveChangesAsync();
        //    }


        //    return RedirectToAction("Index", "Cards");
        //}
    }
}