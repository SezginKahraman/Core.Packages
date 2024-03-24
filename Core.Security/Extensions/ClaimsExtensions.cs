using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Constants;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Core.Security.Extensions
{
    public static class ClaimsExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email) => 
            claims.Add(new Claim(ClaimNames.Email, email));

        public static void AddName(this ICollection<Claim> claims, string name) =>
            claims.Add(new Claim(ClaimNames.Name, name));

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
            claims.Add(new Claim(ClaimNames.NameIdentifier, nameIdentifier));

        public static void AddRole(this ICollection<Claim> claims, string role) =>
            claims.Add(new Claim(ClaimNames.Role, role));

        public static void AddOperationalClaims(this ICollection<Claim> claims, string[] operationalClaims) =>
            operationalClaims.ToList().ForEach(operationalClaim => claims.Add(new Claim(ClaimNames.OperationalClaims, operationalClaim)));
    }
}
