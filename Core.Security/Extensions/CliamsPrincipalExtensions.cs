using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Constants;

namespace Core.Security.Extensions
{
    public static class CliamsPrincipalExtensions // this class represents user's itself
    {
        public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType) =>
             claimsPrincipal?.FindAll(claimType).Select(t => t.Value).ToList();

        public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claims(ClaimNames.Role);

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
            Convert.ToInt32(claimsPrincipal?.Claims(ClaimNames.NameIdentifier)?.FirstOrDefault());
    }
}
