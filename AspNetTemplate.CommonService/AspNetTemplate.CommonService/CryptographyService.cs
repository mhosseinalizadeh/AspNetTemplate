using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public class CryptographyService : ICryptographyService
    {
        public string ComputeHash(string input)
        {
            string msg = "";
            byte[] encode = new byte[input.Length];
            encode = Encoding.UTF8.GetBytes(input);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
    }
}
