using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FableFortuneCardList.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; } = new List<IdentityUserLogin<string>>();
        public ICollection<DeckRanking> DeckRankings { get; set; }

        public ApplicationUser()
        {
            this.DeckRankings = new HashSet<DeckRanking>();
        }
    }
}
