using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Responses.AuthResponses
{
    public class RefreshTokenResponse
    {
        public required string IdentityToken { get; set; }
    }
}
