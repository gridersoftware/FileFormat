using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat
{
    struct TargetLanguage
    {
        /// <summary>
        /// Determines if the target language allows access to non-static members of outer types. C# is not one of these languages.
        /// </summary>
        public bool outerTypeNonStaticAccess;
    }
}
