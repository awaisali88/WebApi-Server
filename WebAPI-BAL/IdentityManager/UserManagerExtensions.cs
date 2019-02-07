using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebAPI_BAL.IdentityManager
{
    public static class UserManagerExtensions
    {
        public static bool IsTokenExpired<TUser, TKey>(this UserManager<TUser> manager, TUser user, string token) where TKey : IEquatable<TKey> where TUser : class, IUser<TKey>
        {
            try
            {
                var tokenProvider = manager.UserTokenProvider as DataProtectorTokenProvider<TUser>;
                if (tokenProvider == null) return false;

                var unprotectedData = tokenProvider.Protector.Unprotect(Convert.FromBase64String(token));
                var ms = new MemoryStream(unprotectedData);
                using (var reader = ms.CreateReader())
                {
                    var creationTime = reader.ReadDateTimeOffset();
                    var expirationTime = creationTime + tokenProvider.TokenLifespan;
                    if (expirationTime < DateTimeOffset.UtcNow)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                // Do not leak exception
            }
            return true;
        }
    }

    internal static class StreamExtensions
    {
        internal static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public static BinaryReader CreateReader(this Stream stream)
        {
            return new BinaryReader(stream, DefaultEncoding, true);
        }

        public static BinaryWriter CreateWriter(this Stream stream)
        {
            return new BinaryWriter(stream, DefaultEncoding, true);
        }

        public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
        {
            return new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
        }

        public static void Write(this BinaryWriter writer, DateTimeOffset value)
        {
            writer.Write(value.UtcTicks);
        }
    }
}
