using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Utils.Constants
{
    public static class Constants
    {
        public const string SHOWTIME_STATUS_ACTIVE = "OPEN";
        public const string SHOWTIME_STATUS_INACTIVE = "CLOSED";

        public static string GetUsername(HttpRequest request)
        {
            string token = request.Headers[HeaderNames.Authorization];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.Split("Bearer ")[1]);
            var username = jwt.Claims.First(claim => claim.Type == "sub").Value;
            return username;
        }
    }
}
