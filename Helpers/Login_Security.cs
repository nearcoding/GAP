using System;
using System.Security.Cryptography;
using System.Text;

namespace GAP.Helpers
{
    public class Login_Security
    {
        private static int SaltValueSize = 8;

        private string CreateSalt(string UserName)
        {

            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(UserName,
                System.Text.Encoding.Default.GetBytes(RandomString()), 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));
        }

        private string HashedPassword(string Salt, string Password)
        {
            Rfc2898DeriveBytes Hasher = new Rfc2898DeriveBytes(Password,
                System.Text.Encoding.Default.GetBytes(Salt), 10000);
            return Convert.ToBase64String(Hasher.GetBytes(25));
        }

        public string[] CreatePassword(string username, string clearpass)
        {
            string Salt = CreateSalt(username);
            string HashPassword = HashedPassword(Salt, clearpass);
            string[] returnvalue = new string[2];
            returnvalue[0] = Salt;
            returnvalue[1] = HashPassword;
            return returnvalue;
        }


        public string RandomString()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < SaltValueSize; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// Method that uses HashedPassword for public purposes
        /// 
        /// </summary>
        /// <returns></returns>
        public string hashPass(string Salt, string Password)
        {
            return HashedPassword(Salt, Password);
        }
    }
}
