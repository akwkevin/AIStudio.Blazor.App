using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    public class RefreshTokenInputDTO
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
