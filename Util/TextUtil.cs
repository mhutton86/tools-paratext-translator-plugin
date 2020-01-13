﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvpMain.Text;

namespace TvpMain.Util
{
    /// <summary>
    /// Bible book-related utilities.
    /// </summary>
    public class TextUtil
    {
        /// <summary>
        /// Abbreviations for bible book names.
        /// </summary>
        private static readonly string[] AbbrevNames = {
            "GEN", "EXO", "LEV", "NUM", "DEU", "JOS",
            "JDG", "RUT", "1SA", "2SA", "1KI", "2KI",
            "1CH", "2CH", "EZR", "NEH", "EST", "JOB",
            "PSA", "PRO", "ECC", "SNG", "ISA", "JER",
            "LAM", "EZK", "DAN", "HOS", "JOL", "AMO",
            "OBA", "JON", "MIC", "NAM", "HAB", "ZEP",
            "HAG", "ZEC", "MAL", "MAT", "MRK", "LUK",
            "JHN", "ACT", "ROM", "1CO", "2CO", "GAL",
            "EPH", "PHP", "COL", "1TH", "2TH", "1TI",
            "2TI", "TIT", "PHM", "HEB", "JAS", "1PE",
            "2PE", "1JN", "2JN", "3JN", "JUD", "REV",
            "TOB", "JDT", "ESG", "WIS", "SIR", "BAR",
            "LJE", "S3Y", "SUS", "BEL", "1MA", "2MA",
            "3MA", "4MA", "1ES", "2ES", "MAN", "PS2",
            "ODA", "PSS", "JSA", "JDB", "TBS", "SST",
            "DNT", "BLT", "XXA", "XXB", "XXC", "XXD",
            "XXE", "XXF", "XXG", "FRT", "BAK", "OTH",
            "3ES", "EZA", "5EZ", "6EZ", "INT", "CNC",
            "GLO", "TDX", "NDX", "DAG", "PS3", "2BA",
            "LBA", "JUB", "ENO", "1MQ", "2MQ", "3MQ",
            "REP", "4BA", "LAO"
        };

        /// <summary>
        /// Get abbreviation text from enum.
        /// </summary>
        /// <param name="bookNum">Book number (1-based).</param>
        /// <returns>Book abbreviation.</returns>
        public static string GetBookCode(int bookNum)
        {
            if (bookNum >= 1 && bookNum <= AbbrevNames.Length)
            {
                return AbbrevNames[(bookNum - 1)];
            }
            else
            {
                return $"#{bookNum:N0}";
            }
        }

        /// <summary>
        /// Converts a Paratext coordinate reference to specific book, chapter, and verse.
        /// </summary>
        /// <param name="inputRef">Input coordinate reference (BBBCCCVVV).</param>
        /// <param name="outputBook">Output book number (1-based).</param>
        /// <param name="outputChapter">Output chapter number (1-1000; Max varies by book & versification).</param>
        /// <param name="outputVerse">Output verse number (1-1000; Max varies by chapter & versification).</param>
        public static void RefToBcv(int inputRef, out int outputBook, out int outputChapter, out int outputVerse)
        {
            outputBook = (inputRef / MainConsts.BookRefMultiplier);
            outputChapter = (inputRef / MainConsts.ChapRefMultiplier) % MainConsts.RefPartRange;
            outputVerse = inputRef % MainConsts.RefPartRange;
        }


        /// <summary>
        /// Converts specific book, chapter, and verse to a Paratext coordinate reference.
        /// </summary>
        /// <param name="inputBook">Input book number (1-based).</param>
        /// <param name="inputChapter">Input chapter number (1-1000; Max varies by book & versification).</param>
        /// <param name="inputVerse">Input verse number (1-1000; Max varies by chapter & versification).</param>
        /// <returns>Output coordinate reference (BBBCCCVVV).</returns>
        public static int BcvToRef(int inputBook, int inputChapter, int inputVerse)
        {
            return (inputBook * MainConsts.BookRefMultiplier)
                   + (inputChapter * MainConsts.ChapRefMultiplier)
                   + inputVerse;
        }
    }
}
