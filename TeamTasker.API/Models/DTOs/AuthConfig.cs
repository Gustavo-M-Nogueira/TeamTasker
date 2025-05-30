﻿namespace TeamTasker.API.Models.DTOs
{
    public class AuthConfig
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
