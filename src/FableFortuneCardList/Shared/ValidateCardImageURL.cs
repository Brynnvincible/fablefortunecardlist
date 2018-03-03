using System.Linq;

namespace FableFortuneCardList.Shared
{
    public class ValidateCardImageURL
    {
        private static string FilterSymbols(string input)
        {
            if (input.Contains(' '))
            {
                input = input.Replace(' ', '_');
            }
            if (input.Contains('!'))
            {
                input = input.Replace("!", string.Empty);
            }
            if (input.Contains('-'))
            {
                input = input.Replace('-', '_');
            }
            if (input.Contains(':'))
            {
                input = input.Replace(":", string.Empty);
            }
            return input;
        }

        public static string GetCardImageURL(string cardName)
        {
            string cardFilename = cardName.Replace(' ', '_') + ".png";                
            return FilterSymbols(cardFilename);
        }

        public static string GetCardEvolutionName(string cardName, int Evolve)
        {
            string cardfilename = FilterSymbols(cardName);
            cardfilename += "_0" + Evolve + ".png";
            return cardfilename;
        }
    }
}
