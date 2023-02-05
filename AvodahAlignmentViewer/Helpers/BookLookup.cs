using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvodahAlignmentViewer.Helpers
{
    public static class BookLookup
    {
        // Paratext list from https://ubsicap.github.io/usfm/identification/books.html
        // and normalized to the CLEAR standard of having Matthew as book 40 not book 41
        public static string GetBookNameFromBookNum(string value)
        {
            Dictionary<string, string> lookup = new Dictionary<string, string>
            {
                { "01", "GEN"},
                { "02", "EXO"},
                { "03", "LEV"},
                { "04", "NUM"},
                { "05", "DEU"},
                { "06", "JOS"},
                { "07", "JDG"},
                { "08", "RUT"},
                { "09", "1SA"},
                { "10", "2SA"},
                { "11", "1KI"},
                { "12", "2KI"},
                { "13", "1CH"},
                { "14", "2CH"},
                { "15", "EZR"},
                { "16", "NEH"},
                { "17", "EST"},
                { "18", "JOB"},
                { "19", "PSA"},
                { "20", "PRO"},
                { "21", "ECC"},
                { "22", "SNG"},
                { "23", "ISA"},
                { "24", "JER"},
                { "25", "LAM"},
                { "26", "EZK"},
                { "27", "DAN"},
                { "28", "HOS"},
                { "29", "JOL"},
                { "30", "AMO"},
                { "31", "OBA"},
                { "32", "JON"},
                { "33", "MIC"},
                { "34", "NAM"},
                { "35", "HAB"},
                { "36", "ZEP"},
                { "37", "HAG"},
                { "38", "ZEC"},
                { "39", "MAL"},
                { "40", "MAT"},
                { "41", "MRK"},
                { "42", "LUK"},
                { "43", "JHN"},
                { "44", "ACT"},
                { "45", "ROM"},
                { "46", "1CO"},
                { "47", "2CO"},
                { "48", "GAL"},
                { "49", "EPH"},
                { "50", "PHP"},
                { "51", "COL"},
                { "52", "1TH"},
                { "53", "2TH"},
                { "54", "1TI"},
                { "55", "2TI"},
                { "56", "TIT"},
                { "57", "PHM"},
                { "58", "HEB"},
                { "59", "JAS"},
                { "60", "1PE"},
                { "61", "2PE"},
                { "62", "1JN"},
                { "63", "2JN"},
                { "64", "3JN"},
                { "65", "JUD"},
                { "66", "REV"}
            };

            string sTmp;
            try
            {
                sTmp = lookup[value];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                sTmp = "";
            }

            return sTmp;
        }
    }
}
