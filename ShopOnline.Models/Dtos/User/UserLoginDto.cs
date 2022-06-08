using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos.User
{
    public class UserLoginDto
    {
        [Required]
        public string LoginIdentifier { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
