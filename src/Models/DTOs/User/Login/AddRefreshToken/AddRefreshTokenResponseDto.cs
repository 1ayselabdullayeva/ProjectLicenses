using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User.Login.AddRefreshToken
{
    public class AddRefreshTokenResponseDto
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }
    }
}
