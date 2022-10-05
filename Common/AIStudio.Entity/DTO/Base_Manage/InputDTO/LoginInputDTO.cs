using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    public class LoginInputDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string LoginType { get; set; }

        public bool AutoLogin { get; set; }
    }
}
