using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public static class FileHelper
    {
        /// <summary>
        /// Get version details of file (assembly)
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns>Product version</returns>
        public static string GetProductVersion(string filePath)
        {
            try
            {
                if (File.Exists(filePath) == false)
                {
                    return null;
                }
                var myFileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
                return myFileVersionInfo.ProductVersion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Copy and overwrite file
        /// </summary>
        /// <param name="source">Source file path</param>
        /// <param name="destination">Destination file path</param>
        public static void CopyAndOverwrite(string source, string destination)
        {
            try
            {
                if (File.Exists(source))
                {
                    if (String.IsNullOrWhiteSpace(destination) == false)
                    {
                        File.Copy(source, destination, true);
                    }
                    else
                    {
                        throw new ArgumentException("Destination is not allowed empty value.");
                    }
                }
                else
                {
                    throw new FileNotFoundException(String.Format("File {0} is not found.", source));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Copy file
        /// </summary>
        /// <param name="source">Source file path</param>
        /// <param name="destination">Detination file path</param>
        public static void Copy(string source, string destination)
        {
            try
            {
                if (File.Exists(source))
                {
                    if (String.IsNullOrWhiteSpace(destination) == false)
                    {
                        File.Copy(source, destination);
                    }
                    else
                    {
                        throw new ArgumentException("Destination is not allowed empty value.");
                    }
                }
                else
                {
                    throw new FileNotFoundException(String.Format("File {0} is not found.", source));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Move File.
        /// </summary>
        /// <param name="source">Source file Path</param>
        /// <param name="destination">Destination file Path</param>
        public static void Move(string source, string destination)
        {
            try
            {
                if (File.Exists(source))
                {
                    if (String.IsNullOrWhiteSpace(destination) == false)
                    {
                        File.Move(source, destination);
                    }
                    else
                    {
                        throw new ArgumentException("Destination is not allowed empty value.");
                    }
                }
                else
                {
                    throw new FileNotFoundException(String.Format("File {0} is not found.", source));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check sum file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>A hash string</returns>
        //public static string GetChecksum(string filePath)
        //{
        //    try
        //    {
        //        if (File.Exists(filePath) == false)
        //        {
        //            throw new FileNotFoundException(String.Format("File {0} is not found.", filePath));
        //        }
        //        using (FileStream stream = File.OpenRead(filePath))
        //        {
        //            var sha = new SHA256Managed();
        //            byte[] checksum = sha.ComputeHash(stream);
        //            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>A hash string</returns>
        public static void Delete(string filePath)
        {
            try
            {
                if (File.Exists(filePath) == false)
                {
                    throw new FileNotFoundException(String.Format("File {0} is not found.", filePath));
                }
                File.Delete(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
