using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTOs
{
    public class AuthenticateResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }

        public AuthenticateResponseDto(string firstName, string lastName, string token)
        {
            FirstName = firstName;
            LastName = lastName;
            Token = token;
        }
    }
}
