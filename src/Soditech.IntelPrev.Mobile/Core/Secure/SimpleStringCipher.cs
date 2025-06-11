using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SavvyTech.R2A.Core.Secure;


public class SimpleStringCipher
{
    const  string EncryptionKey = "your-encryption-key-123"; // Utilisez une clé sécurisée.

    // Méthode de chiffrement
    public static string Encrypt(string plainText)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
        aesAlg.IV = new byte[16]; // Initialisation vector (IV) avec des zéros, peut être améliorée.
            
        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using var swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(plainText);
        }
        
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    // Méthode de déchiffrement
    public static string Decrypt(string cipherText)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
        aesAlg.IV = new byte[16]; // Utilisez le même IV que pour le chiffrement.
            
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }
}
