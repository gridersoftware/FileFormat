using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualFileFormat
{
    static class CompileFlags
    {
        public enum OutputLanguage
        {
            CSharp,
            VisualBasic
        }

        /// <summary>
        /// Gets or sets the output language. This is set to OutputLang.CSharp by default.
        /// </summary>
        public static OutputLanguage Lang { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether array and structure count reference values are 
        /// implicitly determined from array lengths. Default value is false.
        /// </summary>
        public static bool ImplicitCounts { get; set; }

        /// <summary>
        /// Gets or sets a value determining if the output code should be printed to the console. Default value is false.
        /// </summary>
        public static bool PrintOutput { get; set; }
    }
}
