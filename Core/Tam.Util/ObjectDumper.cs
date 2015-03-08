using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public class ObjectDumper
    {
        const string OBJECT_DUMPER_FILE_NAME = "ObjectDumper_{0}.txt";
        const string DATE_PATTERN_4Y_2M_2D = "yyyyMMdd";
        private static string CreatedTime = "";
        private static object LockObject = new object();

        private static string FolderPath = "";

        public ObjectDumper(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new NullOrEmptyArgumentException("FolderPath");
            }
            FolderPath = folderPath;
        }

        public void Print(object target, int index = 0)
        {
            CreatedTime = DateTime.Now.ToString(DATE_PATTERN_4Y_2M_2D);
            //string filePath = string.Format(Logger.Folder_Log_Name + "\\" + OBJECT_DUMPER_FILE_NAME, CreatedTime);
            string filePath = Path.GetFullPath(string.Format(FolderPath + "\\" + OBJECT_DUMPER_FILE_NAME, CreatedTime));
            if (DirectoryHerlper.CreateDirectory(FolderPath) == false)
            {
                return;
            }

            var properties =
                from property in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                select new
                {
                    Name = property.Name,
                    Value = property.GetValue(target, null)
                };

            var builder = new StringBuilder();
            builder.AppendLine("Date/Time: " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            if (index > 0)
            {
                builder.AppendLine("STT = " + index);
            }

            foreach (var property in properties)
            {
                builder
                    .Append(property.Name)
                    .Append(" = ")
                    .Append(property.Value)
                    .AppendLine();
            }
            builder.AppendLine();
            builder.AppendLine("=================================================");

            //set up a filestream
            var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            //set up a streamwriter for adding text
            StreamWriter sw = new StreamWriter(fs);

            lock (LockObject)
            {
                //find the end of the underlying filestream
                sw.BaseStream.Seek(0, SeekOrigin.End);

                //add the text
                sw.WriteLine(builder.ToString());
                //add the text to the underlying filestream

                sw.Flush();
                //close the writer
                sw.Close();
            }
        }

        public void Print(object target)
        {
            Print(target, 0);
        }
    }
}
