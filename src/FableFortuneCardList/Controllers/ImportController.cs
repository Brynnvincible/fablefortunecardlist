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

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadImages()
        {
            return View();
        }

        public async Task<IActionResult> DeleteAllCards()
        {
            _context.Card.RemoveRange(_context.Card.ToList());
            _context.Deck.RemoveRange(_context.Deck.ToList());
            _context.DeckCard.RemoveRange(_context.DeckCard.ToList());
            await _context.SaveChangesAsync();

            return View("UploadResult", "All cards and decks have been deleted successfully");
        }

        public async Task<IActionResult> DeleteAllDecks()
        {
            _context.Deck.RemoveRange(_context.Deck.ToList());
            _context.DeckCard.RemoveRange(_context.DeckCard.ToList());
            await _context.SaveChangesAsync();

            return View("UploadResult", "All decks have been deleted successfully");
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImages(IFormFile file)
        {
            if(file.Length > 0)
            {
                var filename = Path.Combine(_env.WebRootPath, @"Imports\", file.FileName);
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var dirInfoExtract = new DirectoryInfo(Path.Combine(_env.WebRootPath, @"images\cards\extract"));

                foreach(FileInfo imageFile in dirInfoExtract.GetFiles("*", SearchOption.AllDirectories))
                {
                    imageFile.Delete();
                }

                foreach(DirectoryInfo dir in dirInfoExtract.GetDirectories())
                {
                    dir.Delete();
                }
                
                using (ZipArchive za = ZipFile.OpenRead(filename))
                {
                    za.ExtractToDirectory(Path.Combine(_env.WebRootPath, @"images\cards\extract"));
                    
                    foreach(FileInfo imageFile in dirInfoExtract.GetFiles("*", SearchOption.AllDirectories))
                    {
                        var oldName = imageFile.Name;
                        var extension = imageFile.Extension;
                        var newPath = Path.Combine(_env.WebRootPath, @"images\Cards", oldName.Split('_')[0].Replace('-', '_') + extension);
                        if (System.IO.File.Exists(newPath))
                            System.IO.File.Delete(newPath);
                        imageFile.MoveTo(newPath);
                    }
                }
            }

            return View("UploadResult", string.Format("Images from file {0} have been uploaded successfully", file.FileName));
        }

        [HttpPost]
        public async Task<IActionResult> FixSheetIdsAction()
        {
            int highestId = FixSheetIds();

            return View("UploadResult", string.Format("All card IDs have been fixed.  Highest ID value is {0}.", highestId));
        }

        [HttpPost]
        public async Task<IActionResult> CheckImageURLs()
        {
            List<string> missingImageURLs = new List<string>();
            foreach (Card card in _context.Card)
            {
                if (card.ImageUrl == string.Empty)
                {
                    missingImageURLs.Add(string.Format("{0} has a blank image URL (Sheet ID = {1})", card.Name, card.SheetId));
                }
                else if (!System.IO.File.Exists(card.ImageUrl))
                {
                    missingImageURLs.Add(string.Format("{0} points to a missing image file {1} (Sheet ID = {2}", card.Name, card.ImageUrl, card.SheetId));
                }
            }

            string msg = string.Empty;
            if (missingImageURLs.Count > 0)
            {
                msg = "The following Image URL issues were found:";
                foreach (string item in missingImageURLs)
                {
                    msg += "<br>   - " + item;
                }
            }
            else
            {
                msg = "No issues were found with any card image URLs";
            }
            return View("UploadResult", msg);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            /* Sheet Layout     (This is how the sheet will be exported, so it must be imported in the same order)
             * 1 - ID           | 2 - Class         | 3 - Rarity        | 4 - Gold              | 5 - Name              | 
             * 6 - Strength     | 7 - Health        | 8 - Ability       | 9 - Transform         | 10 - Transform Type   | 
             * 11 - Type        | 12 - Image URL    | 13 - UnitClass    | 14 - SheetID          | 15 - DeckCards        | 16 - Transforms       | */             

            int updateCount = 0;
            int addCount = 0;
            int invalidCount = 0;
            List<string> missingImageURLs = new List<string>();

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(_env.WebRootPath, @"Imports\", file.FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    
                    var package = new ExcelPackage(fileStream);

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                    int highestId = FixSheetIds();

                    for (int j = workSheet.Dimension.Start.Row + 1; j <= workSheet.Dimension.End.Row; j++)
                    {
                        var row = workSheet.Row(j);

                        // If Class, Gold, Name or Rarity are null then entry is invalid
                        if (workSheet.Cells[j, 2].Value == null || workSheet.Cells[j, 3].Value == null || workSheet.Cells[j, 4] == null || workSheet.Cells[j, 5].Value == null)
                        {
                            invalidCount++;
                            continue;
                        }
                        ClassType classValue = (ClassType)Enum.Parse(typeof(ClassType), workSheet.Cells[j, 2].Value.ToString());
                        RarityType rarityValue = (RarityType)Enum.Parse(typeof(RarityType), workSheet.Cells[j, 3].Value.ToString().Replace('+', ' '));
                        int goldValue = int.Parse(workSheet.Cells[j, 4].Value.ToString());
                        string nameValue = workSheet.Cells[j, 5].Value.ToString();
                        int strengthValue = workSheet.Cells[j, 6].Value == null ? 0 : int.Parse(workSheet.Cells[j, 6].Value.ToString());
                        int healthValue = workSheet.Cells[j, 7].Value == null ? 0 : int.Parse(workSheet.Cells[j, 7].Value.ToString());
                        string abilityValue = workSheet.Cells[j, 8].Value == null ? string.Empty : workSheet.Cells[j, 8].Value.ToString();
                        string transformValue = workSheet.Cells[j, 9].Value == null ? string.Empty : workSheet.Cells[j, 9].Value.ToString();
                        string transformTypeValue = workSheet.Cells[j, 10].Value == null ? string.Empty : workSheet.Cells[j, 10].Value.ToString();
                        string cardTypeValue = workSheet.Cells[j, 11].Value == null || workSheet.Cells[j, 11].Value.ToString() == string.Empty ? healthValue == 0 ? "Spell" : "Unit" : workSheet.Cells[j, 11].Value.ToString();
                        string unitClassValue = workSheet.Cells[j, 13].Value == null ? string.Empty : workSheet.Cells[j, 13].Value.ToString();                        
                        int sheetIdValue = workSheet.Cells[j, 14].Value == null ? 0 : int.Parse(workSheet.Cells[j, 14].Value.ToString());

                        if (sheetIdValue == 0)
                        {
                            highestId++;
                            sheetIdValue = highestId;
                        }

                        var existingCard = _context.Card.Where(x => x.SheetId == sheetIdValue).FirstOrDefault();
                        if (existingCard != null)
                        {
                            bool updated = false;
                            // Check ImageURLs of existing cards
                            string imageURL = workSheet.Cells[j, 12].Value == null ? string.Empty : workSheet.Cells[j, 12].Value.ToString();
                            if (imageURL == string.Empty)
                            {
                                missingImageURLs.Add(string.Format("{0} has a blank image URL (Sheet ID = {1})", nameValue, sheetIdValue));
                            }
                            else if (!System.IO.File.Exists(imageURL))
                            {
                                missingImageURLs.Add(string.Format("{0} points to a missing image file {1}.  This has been changed to an empty string (Sheet ID = {2}", nameValue, imageURL, sheetIdValue));
                                existingCard.ImageUrl = string.Empty;
                            }

                            // Update existing card.  Flag if one or more fields have been updated                            
                            if (existingCard.Class != classValue)
                            {
                                existingCard.Class = classValue;
                                updated = true;
                            }
                            if (existingCard.Rarity != rarityValue)
                            {
                                existingCard.Rarity = rarityValue;
                                updated = true;
                            }
                            if (existingCard.Name != nameValue)
                            {
                                existingCard.Name = nameValue;
                                updated = true;
                            }
                            if (existingCard.Gold != goldValue)
                            {
                                existingCard.Gold = goldValue;
                                updated = true;
                            }
                            if (existingCard.Strength != strengthValue)
                            {
                                existingCard.Strength = strengthValue;
                                updated = true;
                            }
                            if (existingCard.Health != healthValue)
                            {
                                existingCard.Health = healthValue;
                                updated = true;
                            }
                            if (existingCard.Ability != abilityValue)
                            {
                                existingCard.Ability = abilityValue;
                                updated = true;
                            }
                            if (existingCard.Transform != transformValue)
                            {
                                existingCard.Transform = transformValue;
                                updated = true;
                            }
                            if (existingCard.TransformType != transformTypeValue)
                            {
                                existingCard.TransformType = transformTypeValue;
                                updated = true;
                            }
                            if (existingCard.UnitClass != unitClassValue)
                            {
                                existingCard.UnitClass = unitClassValue;
                                updated = true;
                            }
                            if (existingCard.Type != cardTypeValue)
                            {
                                existingCard.Type = cardTypeValue;
                                updated = true;
                            }
                            if (updated)
                                updateCount++;
                        }
                        else
                        {
                            // Add new card
                            var card = new Card
                            {
                                Class = classValue,
                                Rarity = rarityValue,
                                Name = nameValue,
                                Gold = goldValue,
                                Strength = strengthValue,
                                Health = healthValue,
                                Ability = abilityValue,
                                Transform = transformValue,
                                TransformType = transformTypeValue,
                                UnitClass = unitClassValue,
                                Type = cardTypeValue,
                                SheetId = sheetIdValue
                            };
                       
                            _context.Add(card);
                            addCount++;
                        }
                        
                        await _context.SaveChangesAsync();
                    }                 
                }
            }
            string msg = string.Format("All cards from file {0} have been processed successfully.<br>{1} Cards updated.<br>{2} New cards Added.<br>{3} Cards were skipped due to invalid entries.", file.FileName, updateCount, addCount, invalidCount);
            if (missingImageURLs.Count > 0)
            {
                msg += "<br> The following Image URL issues were found during the import:";
                foreach (string item in missingImageURLs)
                {
                    msg += "<br>   - " + item;
                }
            }
            return View("UploadResult", msg);
        }

        [HttpPost]
        public FileResult Export()
        {
            ICollection<Card> cards = _context.Card.ToList();

            string exportDir = Path.Combine(_env.WebRootPath, @"Exports");
            if (Directory.Exists(exportDir) == false)
                Directory.CreateDirectory(exportDir);
            string exportPath = Path.Combine (exportDir, "CardListExport.xlsx");
            if (System.IO.File.Exists(exportPath))
                System.IO.File.Delete(exportPath);
            FileInfo newFile = new FileInfo(string.Format(exportPath, DateTime.Now.ToString("ddMMyyyyhhmmss")));
            
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Cards");
                worksheet.Cells["A1"].LoadFromCollection(cards, true);
                var stream = new MemoryStream();
                pck.Save();
            }

            return File(newFile.OpenRead(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private int FixSheetIds()
        {
            bool fixedIds = false;
            int highestId = 0;
            List<int> idSet = new List<int>();
            // Get highest current Id
            foreach (Card card in _context.Card)
            {
                if (card.SheetId != 0)
                {
                    if (card.SheetId > highestId)
                    {
                        highestId = card.SheetId;
                    }
                    // Scan for duplicate Ids
                    if (idSet.Contains(card.SheetId))
                    {
                        // Duplicate ID is set to zero.  It will be fixed to a new Id below.
                        card.SheetId = 0;
                        fixedIds = true;
                    }
                    else
                    {
                        idSet.Add(card.SheetId);
                    }
                }
            }

            // Assign new Ids
            foreach (Card card in _context.Card)
            {
                if (card.SheetId == 0)
                {
                    highestId++;
                    card.SheetId = highestId;
                    fixedIds = true;
                }
            }

            if (fixedIds)
            {
                _context.SaveChanges();
            }

            return highestId;
        }
    }
}