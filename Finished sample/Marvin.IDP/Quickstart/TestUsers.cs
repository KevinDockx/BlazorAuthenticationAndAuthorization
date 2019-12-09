// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marvin.IDP
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "06c71238-0137-4df6-bb6a-e50e62a4a7c5", 
                Username = "Jack", Password = "password",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Jack Torrance"),
                    new Claim(JwtClaimTypes.GivenName, "Jack"),
                    new Claim(JwtClaimTypes.FamilyName, "Torrance"),
                    new Claim(JwtClaimTypes.Email, "jack.torrance@email.com"),
                    new Claim("country", "BE")
                }
            },
            new TestUser{SubjectId = "37d0f2fa-1069-489f-9d65-48c9ba44639b", 
                Username = "Wendy", Password = "password",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Wendy Torrance"),
                    new Claim(JwtClaimTypes.GivenName, "Wendy"),
                    new Claim(JwtClaimTypes.FamilyName, "Torrance"),
                    new Claim(JwtClaimTypes.Email, "wendy.torrance@email.com"),
                    new Claim("country", "NL")
                }
            }
        };
    }
}