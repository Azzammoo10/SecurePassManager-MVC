using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistV2.Interface
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}