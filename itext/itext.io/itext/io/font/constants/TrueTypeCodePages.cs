using System;

namespace iText.IO.Font.Constants {
    /// <summary>The code pages possible for a True Type font.</summary>
    public sealed class TrueTypeCodePages {
        private TrueTypeCodePages() {
        }

        private static readonly String[] codePages = new String[] { "1252 Latin 1", "1250 Latin 2: Eastern Europe"
            , "1251 Cyrillic", "1253 Greek", "1254 Turkish", "1255 Hebrew", "1256 Arabic", "1257 Windows Baltic", 
            "1258 Vietnamese", null, null, null, null, null, null, null, "874 Thai", "932 JIS/Japan", "936 Chinese: Simplified chars--PRC and Singapore"
            , "949 Korean Wansung", "950 Chinese: Traditional chars--Taiwan and Hong Kong", "1361 Korean Johab", null
            , null, null, null, null, null, null, "Macintosh Character Set (US Roman)", "OEM Character Set", "Symbol Character Set"
            , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "869 IBM Greek"
            , "866 MS-DOS Russian", "865 MS-DOS Nordic", "864 Arabic", "863 MS-DOS Canadian French", "862 Hebrew", 
            "861 MS-DOS Icelandic", "860 MS-DOS Portuguese", "857 IBM Turkish", "855 IBM Cyrillic; primarily Russian"
            , "852 Latin 2", "775 MS-DOS Baltic", "737 Greek; former 437 G", "708 Arabic; ASMO 708", "850 WE/Latin 1"
            , "437 US" };

        /// <summary>Gets code page description based on ulCodePageRange bit settings (OS/2 table).</summary>
        /// <remarks>
        /// Gets code page description based on ulCodePageRange bit settings (OS/2 table).
        /// See https://www.microsoft.com/typography/unicode/ulcp.htm for more details.
        /// </remarks>
        /// <param name="bit">index from ulCodePageRange bit settings (OS/2 table). From 0 to 63.</param>
        /// <returns>code bage description.</returns>
        public static String Get(int bit) {
            System.Diagnostics.Debug.Assert(bit >= 0 && bit < 64);
            return codePages[bit];
        }
    }
}
