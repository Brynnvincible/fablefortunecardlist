using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Enums
{
    public sealed class Roles
    {
        private readonly String name;
        private readonly String description;

        // Available Roles
        public static readonly Roles ADMIN = new Roles("Admin", "Administrative Role.  Full access to all site functionality.");
        public static readonly Roles DECKMASTER = new Roles("DeckMaster", "DeckMastter Role.  Can create decks, vote and add comments.");
        public static readonly Roles USER = new Roles("User", "User Role.  Read-only access to site.");
        public static readonly List<Roles> AllRoles = new List<Roles> { ADMIN, DECKMASTER, USER };

        private Roles(String Name, String Description)
        {
            name = Name;
            description = Description;
        }

        public String Name { get { return name; } }
        public String Description { get { return description; } }

        public override String ToString()
        {
            return name;
        }
    }
}
