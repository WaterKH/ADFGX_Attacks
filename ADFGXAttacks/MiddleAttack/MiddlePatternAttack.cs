using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADFGXAttacks.MiddleAttack
{
    public class MiddlePatternAttack
    {
        public string consonants = "bcdfghklmnpqrstvwxz";
        public string vowels = "aeiouy";

        public void Setup(string ciphertext)
        {
            // Write to file all patterns for SNR
            // Append X and A
            // We'll start 12 and go 9 spaces
            var wordpattern = ciphertext.Substring(12, 3);
            Console.WriteLine(wordpattern);

            foreach(var N in consonants)
            {
                var dictionary = new Dictionary<char, char>();
                dictionary.Add(wordpattern[1], N);

                foreach(var R in vowels)
                {
                    if(dictionary.ContainsValue(R))
                    {
                        continue;
                    }

                    dictionary.Add(wordpattern[2], R);

                    foreach (var S in vowels)
                    {
                        dictionary.Add(wordpattern[0], S);

                        WritePlaintextToFile(dictionary, ciphertext);

                        dictionary.Remove(wordpattern[0]);
                    }
                    dictionary.Remove(wordpattern[2]);
                }
                dictionary.Remove(wordpattern[1]);
            }
        }

        public void WritePlaintextToFile(Dictionary<char, char> dictionary, string ciphertext)
        {
            using (StreamWriter writer = new StreamWriter("middle_setup.txt", true))
            {
                var pt = "";
                foreach (var c in ciphertext)
                {
                    if (dictionary.ContainsKey(c))
                    {
                        pt += dictionary[c];
                    }
                    else
                    {
                        pt += c;
                    }
                }

                writer.WriteLine(pt);
            }
        }

        public void Attack()
        {
            // Find words that fit these patterns
        }
    }
}
