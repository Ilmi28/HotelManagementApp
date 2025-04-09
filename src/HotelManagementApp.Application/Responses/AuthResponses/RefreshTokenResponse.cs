using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.Responses.AuthResponses
{
    public class RefreshTokenResponse
    {
        public required string IdentityToken { get; set; }
    }
}
