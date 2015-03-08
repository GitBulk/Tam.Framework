using System;
using System.Security.Cryptography;
using System.Text;

namespace Tam.Util
{
    public static class CryptorEngine
    {
        private const string key = "shi-123-!!!-789-dylan!!!";

        public static string Encrypt(string toEncrypt, bool useHashing = true)
        {
            if (string.IsNullOrEmpty(toEncrypt))
            {
                return null;
            }
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing = true)
        {
            if (string.IsNullOrEmpty(cipherString))
            {
                return null;
            }
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #region Hash

        /// <summary>
        /// Hash a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt">A key that will increase security</param>
        /// <param name="hashType">HashType: md5, sha1, sha256, sha512</param>
        /// <returns></returns>
        public static string Hash(string input, string salt = "f@nc1Sha@p", HashType hashType = HashType.Md5)
        {
            string result = "";
            switch (hashType)
            {
                case HashType.Sha1:
                    result = HashSHA1(input + salt);
                    break;
                case HashType.Sha256:
                    result = HashSHA256(input + salt);
                    break;
                case HashType.Sha512:
                    return HashSHA512(input + salt);
                default:
                    result = HashMD5(input + salt);
                    break;
            }
            return result;
        }

        private static string HashMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //get hash result after compute it
            byte[] data = md5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(input));

            return ConvertHexaDigit(data);
        }

        private static string ConvertHexaDigit(byte[] data)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(data[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        private static string HashSHA1(string input)
        {
            var hash = new SHA1Managed();
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return ConvertHexaDigit(data);
        }

        private static string HashSHA256(string input)
        {
            var hash = new SHA256Managed();
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return ConvertHexaDigit(data);
        }

        private static string HashSHA512(string input)
        {
            var hash = new SHA512Managed();
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return ConvertHexaDigit(data);
        }
        #endregion
    }
}