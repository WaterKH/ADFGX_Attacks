using System;
using System.Linq;

namespace ADFGXAttacks.Utilities
{
    public class ADFGXUtilities
    {
        static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static string RemoveSpecialCharactersFromWord(string s)
        {
            string rs = "";

            foreach(var c in s)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    rs += c;
            }

            return rs;
        }

        public static string ToAlphaPattern(string s)
        {
            string ap = "";

            string lower_s = s.ToLower();
            string key = String.Join("", s.Distinct()).ToLower();

            for (int i = 0; i < s.Length; ++i)
            {
                //Console.WriteLine(lower_s[i]);
                //Console.WriteLine(key.IndexOf(lower_s[i]));
                //Console.WriteLine(s[i]);
                ap += alphabet[key.IndexOf(lower_s[i])];
            }

            return ap;
        }
    }
}
