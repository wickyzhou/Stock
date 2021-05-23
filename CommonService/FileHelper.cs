using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public static class FileHelper
    {   

        //写到磁盘文件
        public static void WriteToDisk(string httpStatusCode, int i,string fileName = "GatherLog.txt")
        {
            string fullName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            FileMode fileMode = File.Exists(fullName) ? FileMode.Append : FileMode.Create;
            using FileStream fileStream = new FileStream(fullName, fileMode, FileAccess.Write);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} \t {httpStatusCode} \t 重试{i}次");
        }

        public static void WriteToDisk(string message,string fileName = "GatherLog.txt")
        {
            string fullName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            FileMode fileMode = File.Exists(fullName) ? FileMode.Append : FileMode.Create;
            using FileStream fileStream = new FileStream(fullName, fileMode, FileAccess.Write);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} \t {message} ");
        }
    }
}
