using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.Identity.Dapper;
using Encryptor;
using Microsoft.AspNetCore.Identity;

namespace WebAPI_BAL.IdentityManager
{
    public class PasswordHasherAes : IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            return AesEncryptor.Encrypt(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            var decryptedPassword = AesEncryptor.Decrypt(hashedPassword);
            if (string.CompareOrdinal(decryptedPassword, providedPassword) == 0)
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}
