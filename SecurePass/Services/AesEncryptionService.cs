// Fichier : Services/AesEncryptionService.cs
using System;
using System.Security.Cryptography;
using System.Text;
using WatchlistV2.Interface;

public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesEncryptionService(string key, string iv)
    {
        // Décoder la clé et l'IV depuis Base64
        _key = Convert.FromBase64String(key);
        _iv = Convert.FromBase64String(iv);

        // Vérifier la taille de la clé
        if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
        {
            throw new ArgumentException("La clé doit avoir une taille de 16, 24 ou 32 octets (128, 192 ou 256 bits).");
        }

        // Vérifier la taille de l'IV
        if (_iv.Length != 16)
        {
            throw new ArgumentException("L'IV doit avoir une taille de 16 octets (128 bits).");
        }
    }

    public string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}