﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FableFortuneCardList.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}
