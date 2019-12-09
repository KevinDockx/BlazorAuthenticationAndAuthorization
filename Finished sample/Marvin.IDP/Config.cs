// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Marvin.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("country", new [] { "country" })
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("bethanyspieshophrapi", 
                    "Bethany's Pie Shop HR API", 
                    new [] { "country" })
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            { 
                new Client
                {
                    ClientId = "bethanyspieshophr",
                    ClientName = "Bethany's Pie Shop HR",
                    AllowedGrantTypes = GrantTypes.Hybrid, 
                    ClientSecrets = { new Secret("108B7B4F-BEFC-4DD2-82E1-7F025F0F75D0".Sha256()) },
                    RedirectUris = { "https://localhost:44329/signin-oidc" }, 
                    PostLogoutRedirectUris = { "https://localhost:44329/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AllowedScopes = { "openid", "profile", "email", "bethanyspieshophrapi", "country" } 
                }                 
            };
    }
}