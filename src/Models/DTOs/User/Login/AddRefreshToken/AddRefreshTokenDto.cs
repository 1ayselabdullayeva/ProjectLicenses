using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User.Login.AddRefreshToken
{
    public class AddRefreshTokenDto
    {
        public int? Id { get; set; }
        public string? RefreshToken { get; set; }
    }
}
