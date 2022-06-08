﻿using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ShopOnline.Web.Authorization
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IManageUserLocalStorageService userLocalStorageService;
        private readonly HttpClient httpClient;

        public CustomAuthStateProvider(IManageUserLocalStorageService userLocalStorageService,HttpClient httpClient)
        {
            this.userLocalStorageService = userLocalStorageService;
            this.httpClient = httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await userLocalStorageService.GetToken();
            var identity = new ClaimsIdentity();
            httpClient.DefaultRequestHeaders.Authorization = null;
            if ( !string.IsNullOrEmpty(token))
            {
                 identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer",token);
            }
 

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
