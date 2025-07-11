﻿namespace Soditech.IntelPrev.Users.Application.Helpers.Models;

public class WelcomeTemplateMailModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = "IntelPrev";
    public string ApplicationUrl { get; set; } = string.Empty;
}
