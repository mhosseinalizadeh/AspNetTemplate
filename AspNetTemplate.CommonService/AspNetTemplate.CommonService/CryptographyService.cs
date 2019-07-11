using System;
using System.Security.Cryptography;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public class CryptographyService : ICryptographyService
    {
        public string ComputeHash(string input)
        {
            byte[] result;
            var data = Encoding.ASCII.GetBytes(input);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] sha1data = sha.ComputeHash(data);

            ASCIIEncoding ascii = new ASCIIEncoding();
            return ascii.GetString(sha1data);

        }
    }
}
