using Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class UserRefreshToken : BaseEntity<int>
    {
        [Required]
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
