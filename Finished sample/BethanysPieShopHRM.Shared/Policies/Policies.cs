using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BethanysPieShopHRM.Shared
{
    public static class Policies
    {
        public const string CanManageEmployees = "CanManageEmployees";

        public static AuthorizationPolicy CanManageEmployeesPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "BE")
                .Build();
        }
    }
}
