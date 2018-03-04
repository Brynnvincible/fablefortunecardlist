using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FableFortuneCardList.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<IdentityUserRole<string>> Users { get; } = new List<IdentityUserRole<string>>();
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }
    }
}
