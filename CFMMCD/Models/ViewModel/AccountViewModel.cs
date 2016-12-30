using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CFMMCD.Sessions;

namespace CFMMCD.Models.ViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Username required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        public string PasswordVerify { get; set; }

        public string SearchItem { get; set; }

        public bool AllExceptUAC { get; set; }
        public bool MIMInput { get; set; }
        public bool RIMInput { get; set; }
        public bool MERInput { get; set; }
        public bool STPInput { get; set; }
        public bool SCMInput { get; set; }
        public bool VEMInput { get; set; }
        public bool VAMInput { get; set; }
        public bool UAPInput { get; set; }
        public bool MIPInput { get; set; }
        public bool RIPInput { get; set; }
        public bool AULInput { get; set; }
        public bool REGInput { get; set; }
        public bool TEGInput { get; set; }
        public bool TIPInput { get; set; }
        public bool BUEInput { get; set; }
        public bool OWNInput { get; set; }
        public bool PRCInput { get; set; }
        public bool LOCInput { get; set; }
        public string UserAccess { get; set; }

        public bool EditMode { get; set; }
        public string OldUsername { get; set; }
        public List<AccountViewModel> AccountList { get; set; }
    }
    
}