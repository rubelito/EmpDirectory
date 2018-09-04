using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BCS.Identity
{
    public class LoginModel : MembershipUser
    {
        public LoginModel(){
            ErrorMessages = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name="Remember me?")]
        public bool RememberMe { get; set; }

        public bool IsSuccessful { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
