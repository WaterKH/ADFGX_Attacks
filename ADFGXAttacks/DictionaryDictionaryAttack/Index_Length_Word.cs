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
                        if (i + tempWord.Length < cipher.Length)
                        {
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
        }

        public void Attack()
        {
            //var startCipher = String.Join("", cipher.Distinct());

            foreach(var index in words)
            {
                foreach(var length in index.Value)
                {
                    foreach (var word in length.Value)
                    {
                        //Console.WriteLine(word);
                        var startIndex = length.Key - 1;
                        //var distinctWord = String.Join("", word.Distinct());
                        Dictionary<char, char> key = new Dictionary<char, char>();

                        for (int i = 0; i < word.Length; ++i)
                        {
                            key.Add(cipher[i], word[i]);
                        }

                        RecursiveAttack(2, startIndex, key, cipher);
                    }
                }
            }
        }

        public void RecursiveAttack(int depth, int index, Dictionary<char, char>cipherKey, string ciphertext)
        {
            if(depth >= 8)
            {
                using(StreamWriter writer = new StreamWriter("possibilities.txt", true))
                {
                    var pt = "";
                    foreach(var c in cipher)
                    {
                        if (cipherKey.ContainsKey(c))
                            pt += cipherKey[c];
                        else
                            pt += c;
                    }
                    writer.WriteLine(pt);
                }
                return;
            }

            foreach (var index_w in words)
            {
                foreach (var length in index_w.Value)
                {
                    index += length.Key;
                    foreach (var word in length.Value)
                    {
                        
                        //var distinctWord = String.Join("", word.Distinct());
                        var key = new Dictionary<char, char>();

                        foreach(var k in cipherKey)
                        {
                            key.Add(k.Key, k.Value);
                        }

                        bool equal = true;

                        for (int i = 0; i < word.Length; ++i)
                        {
                            if (!key.ContainsKey(ciphertext[index + i]))
                            {
                                if (!key.ContainsValue(word[i]))
                                {
                                    key.Add(ciphertext[index + i], word[i]);
                                }
                                else
                                {
                                    equal = false;
                                    break;
                                }
                            }
                            else
                            {
                                
                                if (key[ciphertext[index + i]] != word[i])
                                {
                                    equal = false;
                                    break;
                                }
                            }
                        }

                        if (equal)
                        {
                            depth += 1;
                            RecursiveAttack(depth, index, key, ciphertext);
                            depth -= 1;
                        }
                    }
                    index -= length.Key;
                    //Console.WriteLine(index);
                }
            }
        }
    }
}
