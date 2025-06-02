﻿using System;
using System.Collections.Generic;

namespace Soditech.IntelPrev.Mobile.Models.Common;

public class AuthenticateResultPersistanceModel
{
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    public int ExpireInSeconds { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpireDate { get; set; }

    public long UserId { get; set; }

    public bool ShouldResetPassword { get; set; }

    public bool RequiresTwoFactorVerification { get; set; }

    public List<string> TwoFactorAuthProviders { get; set; }

    public AuthenticateResultPersistanceModel()
    {
        TwoFactorAuthProviders = new List<string>();
    }
}