using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
