using System.Security.Cryptography;

namespace ApiRest.Helpers
{
    /* public class PasswordHasher
     {
         private static RNGCryptoserviceProvider rng = new RNGCryptoserviceProvider();
         private static readonly int SaltSize = 16;
         private static readonly int HashSize = 20;
         private static readonly int Iterations = 10000;

         public static string HashPassword(string password)
         {
             byte[] salt;
             rng.GetBytes(salt = new byte[SaltSize]);
             var key = new Rfc2898DeriveBytes(password, salt, Iterations);
             var hash = key.GetBytes(HashSize);

             var hashBytes = new byte[SaltSize + HashSize];
             Array.Copy(salt, 0, hashBytes, 0, SaltSize);
             Array.Copy(hash, 0, hashBytes, HashSize, SaltSize);

             var base64Hash = Convert.ToBase64String(hashBytes);

             return base64Hash;
         }
         public static bool VerifyPassword(string password, string base64Hash)
         {
             {
                 var hashBytes = Convert.FromBase64String(base64Hash);

                 var salt = new byte[SaltSize];
                 Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                 var key = new Rfc2898DeriveBytes(password, salt, Iterations);
                 byte[] hash = key.GetBytes(HashSize);
                 for (var i = 0; i < HashSize; i++)
                 {
                     if (hashBytes[i + SaltSize] != hash[i])
                         return false;
                 }
                 return true;
             }
         }
     }*/
    public class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] hash = key.GetBytes(HashSize);

                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    string base64Hash = Convert.ToBase64String(hashBytes);
                    return base64Hash;
                }
            }
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            byte[] hashBytes = Convert.FromBase64String(base64Hash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = key.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
            }

            return true;
        }
    }
}

