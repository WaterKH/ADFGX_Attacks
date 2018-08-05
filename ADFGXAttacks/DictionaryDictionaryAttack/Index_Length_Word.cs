using System;
using System.Collections.Generic;
using System.IO;
using ADFGXAttacks.Utilities;
using System.Linq;

namespace ADFGXAttacks.DictionaryDictionaryAttack
{
    public class Index_Length_Word
    {
        public Dictionary<int, Dictionary<int, List<string>>> words = new Dictionary<int, Dictionary<int, List<string>>>();
        public int maxWordLength = 8;
        public string cipher = "IUNOLSOGTNZNSSMSSMNTMAOOWPPOWXNTLL".ToLower();

        public void Setup(string file)
        {
            var wordsForAttack = File.ReadAllLines(file);

            for (int i = 0; i < cipher.Length; ++i)
            {
                var dict = new Dictionary<int, List<string>>();

                for (int j = 1; j <= maxWordLength; ++j)
                {
                    dict.Add(j, new List<string>());
                }

                words.Add(i, dict);
            }

            for (int i = 0; i < cipher.Length; ++i)
            {
                foreach (var w in wordsForAttack)
                {
                    var tempWord = ADFGXUtilities.RemoveSpecialCharactersFromWord(w);
                    var alphaWord = ADFGXUtilities.ToAlphaPattern(tempWord);

                    if (tempWord.Length <= maxWordLength && tempWord.Length > 0)
                    {
                        // TODO Maybe go through 0 - maxWordLength and make substrings of those outside of the for loop?
                        var tempSub = cipher.Substring(i, tempWord.Length);
                        var alphaSub = ADFGXUtilities.ToAlphaPattern(alphaWord);

                        if (alphaSub == alphaWord)
                        {
                            words[i][tempWord.Length].Add(tempWord);
                        }
                    }
                }
            }
        }

        public void Attack()
        {
            var startCipher = String.Join("", cipher.Distinct());

            foreach(var index in words)
            {
                foreach(var length in index.Value)
                {
                    foreach (var word in length.Value)
                    {
                        var startIndex = length.Key - 1;
                        var distinctWord = String.Join("", word.Distinct());
                        Dictionary<char, char> key = new Dictionary<char, char>();

                        for (int i = 0; i < distinctWord.Length; ++i)
                        {
                            key.Add(startCipher[i], distinctWord[i]);
                        }

                        RecursiveAttack(2, startIndex, key, startCipher);
                    }
                }
            }
        }

        public void RecursiveAttack(int depth, int index, Dictionary<char, char>cipherKey, string distinctCipher)
        {
            if(depth >= 8)
            {
                using(StreamWriter writer = new StreamWriter("possibilities.txt"))
                {
                    var pt = "";
                    foreach(var c in cipher)
                    {
                        pt += cipherKey[c];
                    }
                    writer.WriteLine();
                }
                return;
            }

            foreach (var index_w in words)
            {
                foreach (var length in index_w.Value)
                {
                    foreach (var word in length.Value)
                    {
                        index += length.Key - 1;
                        var distinctWord = String.Join("", word.Distinct());
                        var key = new Dictionary<char, char>();

                        foreach(var k in cipherKey)
                        {
                            key.Add(k.Key, k.Value);
                        }

                        bool equal = true;

                        for (int i = 0; i < distinctWord.Length; ++i)
                        {
                            if (!cipherKey.ContainsKey(distinctCipher[i]))
                            {
                                if (cipherKey[distinctCipher[i]] == distinctWord[i])
                                {
                                    cipherKey.Add(distinctCipher[i], distinctWord[i]);
                                }
                                else
                                {
                                    equal = false;
                                    break;
                                }
                            }
                        }

                        if (equal)
                        {
                            RecursiveAttack(2, index, cipherKey, distinctCipher);
                        }

                        index -= length.Key + 1;
                    }
                }
            }
        }
    }
}
