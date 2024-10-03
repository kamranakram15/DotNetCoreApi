using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTOs
{
    public class AuthenticateUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? IPAddress { get; set; }
        public string? Device { get; set; }
        public string? Browser { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
