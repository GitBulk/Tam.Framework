using System;
using System.IO;
using System.Security.AccessControl;

namespace Tam.Util
{
    // Any public static members of class Directory are thread safe. Any instance members are not guaranteed to be thread safe.
    // https://msdn.microsoft.com/en-us/library/system.io.directory%28v=vs.90%29.aspx
    public static class DirectoryHerlper
    {
        /// <summary>
        /// Create a directory
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>True: create directory successfully. False: create directory failed</returns>
        public static bool CreateDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path) == false) // not existing
                {
                    //DirectoryInfo dirInfo = new DirectoryInfo(path);
                    //DirectorySecurity dirSec = new DirectorySecurity(path, AccessControlSections.All);
                    //Directory.CreateDirectory(path, dirSec);
                    Directory.CreateDirectory(path);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create a directory FullAcess
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>True: create directory successfully. False: create directory failed</returns>
        public static bool CreateDirectoryFullAcess(string path)
        {
            try
            {
                if (Directory.Exists(path) == false) // not existing
                {
                    var dirInfo = new DirectoryInfo(path);
                    var dirSec = new DirectorySecurity(path, AccessControlSections.All);
                    Directory.CreateDirectory(path, dirSec);
                    //Directory.CreateDirectory(path);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deep copy directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void DeepCopy(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                // Recursively call the DeepCopy Method for each Directory
                foreach (DirectoryInfo dir in source.GetDirectories())
                {
                    DirectoryInfo subDir = target.CreateSubdirectory(dir.Name);

                    // --- IMPROVED SECTION ---
                    // Set the security settings for the new directory same as the source dir
                    subDir.SetAccessControl(new DirectorySecurity(dir.FullName, AccessControlSections.All));
                    DeepCopy(dir, target.CreateSubdirectory(dir.Name));
                }

                // Go ahead and copy each file in "source" to the "target" directory
                foreach (FileInfo file in source.GetFiles())
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true); // allow overwrite
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeepCopy(DirectoryInfo source, DirectoryInfo target, string excludeFileName)
        {
            try
            {
                // Recursively call the DeepCopy Method for each Directory
                foreach (DirectoryInfo dir in source.GetDirectories())
                {
                    DirectoryInfo subDir = target.CreateSubdirectory(dir.Name);

                    // --- IMPROVED SECTION ---
                    // Set the security settings for the new directory same as the source dir
                    subDir.SetAccessControl(new DirectorySecurity(dir.FullName, AccessControlSections.All));
                    DeepCopy(dir, target.CreateSubdirectory(dir.Name));
                }

                // Go ahead and copy each file in "source" to the "target" directory
                foreach (FileInfo file in source.GetFiles())
                {
                    if (file.Name != excludeFileName)
                    {
                        file.CopyTo(Path.Combine(target.FullName, file.Name), true); // allow overwrite
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeepCopy(string source, string target, string excludeFileName)
        {
            try
            {
                var dirSource = new DirectoryInfo(source);
                var dirTarget = new DirectoryInfo(target);
                DeepCopy(dirSource, dirTarget, excludeFileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deep copy directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void DeepCopy(string source, string target)
        {
            try
            {
                var dirSource = new DirectoryInfo(source);
                var dirTarget = new DirectoryInfo(target);
                DeepCopy(dirSource, dirTarget);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeepDelete(string targetDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(targetDirectory);
                string[] dirs = Directory.GetDirectories(targetDirectory);
                foreach (string file in files)
                {
                    // Remove read-only access attributes from the files right before delete them
                    // Otherwise that will raise an exception
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                foreach (string dir in dirs)
                {
                    //Directory.Delete(dir);
                    DeepDelete(dir);
                }
                Directory.Delete(targetDirectory, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}