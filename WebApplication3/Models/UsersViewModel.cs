using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Models
{
    public class UsersViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Username")]
        [Remote("IsUserNameAvailable", "Account", ErrorMessage = "Username Already Exist")]
        public string Username { get; set; }
       

        [Required]
        [Remote("IsEmailAvailable", "Account", ErrorMessage = "Email Id Already Exist")]
        [Display(Name = "Email ID")]
        [RegularExpression(@"^([\w.-]+)@([\w-]+)((.(\w){2,3})+)$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}