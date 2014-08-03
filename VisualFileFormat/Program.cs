using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VisualFileFormat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        public static string GetEncodingString(string ffEncoding)
        {
            switch (ffEncoding)
            {
                case "utf7": return "Encoding.UTF7";
                case "utf8": return "Encoding.UTF8";
                case "utf32": return "Encoding.UTF32";
                case "unicode": return "Encoding.Unicode";
                case "ascii": return "Encoding.ASCII";
                default: return "";
            }
        }
    }
}
