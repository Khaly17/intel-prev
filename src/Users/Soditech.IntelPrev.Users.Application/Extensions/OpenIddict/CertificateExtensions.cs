using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Soditech.IntelPrev.Users.Application.Extensions.OpenIddict;

public static class CertificateExtensions
{
    public static OpenIddictServerBuilder AddCertificate(this OpenIddictServerBuilder options,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        if (environment.IsDevelopment())
        {
            // Register the signing and encryption credentials.
            options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();
        }
        else
        {
            /*
             * Don't enable `IsSelfCertificateEnable` before fixing this :
             * https://learn.microsoft.com/en-us/troubleshoot/windows-server/certificates-and-public-key-infrastructure-pki/cannot-import-aes256-sha256-encrypted-pfx-certificate */

            if (configuration.GetSection("OpenIddict:IsSelfCertificateEnable").Get<bool>())
            {
                var signingCertificate = configuration.GetSection("OpenIddict:SigningCertificateFile").Get<string>();

                if (File.Exists(signingCertificate))
                {
                    var signingCertificateKey = configuration.GetSection("OpenIddict:SigningCertificateKey").Get<string>() ?? string.Empty;
                    options.AddSigningCertificate(new X509Certificate2(signingCertificate, signingCertificateKey));
                }
                
                var encryptionCertificate = configuration.GetSection("OpenIddict:EncryptionCertificateFile").Get<string>();

                if (File.Exists(encryptionCertificate))
                {
                    var encryptionCertificateKey = configuration.GetSection("OpenIddict:EncryptionCertificateKey").Get<string>() ?? string.Empty;
                    options.AddSigningCertificate(new X509Certificate2(encryptionCertificate, encryptionCertificateKey));
                }
            }
            else if (configuration.GetSection("OpenIddict:Certificates:Store:Enabled").Get<bool>())
            {
                var certName = configuration.GetSection("OpenIddict:Certificates:Store:Name").Get<string>() ;
                if (certName != null)
                {
                    var cert = GetCertificateFromStore(certName);
                    if (cert != null)
                    {
                        options.AddEncryptionCertificate(cert)
                            .AddSigningCertificate(cert);
                    }
                    else
                    {
                        // add logging to inform the user that the certificate was not found
                        throw new InvalidOperationException($"the certificate ({certName}) was not found.");

                    }
                }
            }
            else
            {
                /* https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html */
                options
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey();
            }

        }
        
        options.DisableAccessTokenEncryption();

		
        return options;
    }

    private static X509Certificate2? GetCertificateFromStore(string certificateName)
    {
        var store = new X509Store(StoreName.My, StoreLocation.LocalMachine); // choose the local machine store
        store.Open(OpenFlags.ReadOnly);

        var cert = store.Certificates
            .FirstOrDefault(cer => 
                cer.Subject.Contains(certificateName, StringComparison.OrdinalIgnoreCase));

        store.Close();

        return cert;
    }

}