using System.ComponentModel.DataAnnotations;

namespace FableFortuneCardList.Models
{
    public class DeckRanking
    {
        [Key]
        public int Id { get; set; }
        public int DeckID { get; set; }
        public Deck Deck { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int Ranking { get; set; }
    }
}
