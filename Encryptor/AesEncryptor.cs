using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encryptor
{
    public class AesEncryptor
    {
        static string _passKey = "tV+nNskA_QEE35up7D=mHV-ubVCtMUhcx3XC?56W74RJg^mtkD!r2$emGUAx=-hbesn3GhPsJqx5gPZYKHHQEvV?GfxjNnZ+VXpZ_pFZ#9rptpHc$wwhwZa^&3-qQfM2#9%+3zp-J-Pd*c5h*DAEkFe#g=C!CB%BMwya+e5Lz?vSALD!f%9Zb#HXD$M6kd24JM!2$%4T*LP8s6aWR^r!eqV^3_y4PLWnmEVRZcTj&vXE$x+MTJc2$&gN!z*VBYx6";
        private static int _iterations = 2;
        private static int _keySize = 256;

        private static string _salt = "tT,>_6&eBJ=mw#*H"; // Random
        private static string _vector = "Z9fy?=K82,k$jS'r"; // Random

        /// <summary>
        /// Protect the data with a wrapper of Protection around it
        /// </summary>
        /// <param name="data">Data to be Protected</param>
        /// <returns>return protected data</returns>
        public static string Encrypt(string data)
        {
            try
            {
                return Encrypt(data, _passKey);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Unprotect the Data that has wrapped by prootection
        /// </summary>
        /// <param name="data">data to be unprotected</param>
        /// <returns>return unprotected data</returns>
        public static string Decrypt(string data)
        {
            try
            {
                return Decrypt(data, _passKey);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }


        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }
        public static string Encrypt<T>(string value, string password)
                where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            byte[] encrypted;
            using (T cipher = new T())
            {
                Rfc2898DeriveBytes passwordBytes =
                    new Rfc2898DeriveBytes(password, saltBytes, _iterations);
                byte[] keyBytes = passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }
        public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount;

            using (T cipher = new T())
            {
                Rfc2898DeriveBytes passwordBytes =
                    new Rfc2898DeriveBytes(password, saltBytes, _iterations);
                byte[] keyBytes = passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return String.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}
