﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RecipesRealm
{
    public class Utils
    {
        public static void WriteToLog(string className, string methodName, string msg)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Logs", DateTime.Now.ToString("yyyy-MM-dd_") + "log.txt");
                File.AppendAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [" + className + "][" + methodName + "] " + msg);
            }
            catch (Exception)
            {
            }
        }
    }
}