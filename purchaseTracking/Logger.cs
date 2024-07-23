using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace purchaseTracking
{
    public class Logger
    {
        private static readonly object lockObj = new object();
        private static string logFilePath = @"C:\Marcaje\Logs\log.txt";
        public static void Log(string message)
        {
            lock (lockObj)
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(logFilePath))
                    {
                        sw.WriteLine($"{DateTime.Now} - {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al escribir en el archivo de log: {ex.Message}");
                }
            }
        }
    }
}
