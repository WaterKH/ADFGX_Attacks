using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ADFGXAttacks.EndOfTextWordAttack
{
    public class EndAttack
    {
        public void Attack(string ciphertext, string file)
        {
            var lines = File.ReadAllLines(file);
            var writer = new StreamWriter("endAttack.txt");

            foreach (var l in lines)
            {
                var lower_l = l.ToLower();
                var sub_cipher_end = ciphertext.Substring(ciphertext.Length - l.Length);
                var dict = new Dictionary<char, char>();

                var equal = AddToDictionary(lower_l, sub_cipher_end, ref dict);

                if (!equal)
                    continue;
                
                foreach (var k in lines)
                {
                    var lower_k = k.ToLower();
                    var sub_cipher_start = ciphertext.Substring(0, k.Length);
                    var pt = "";

                    equal = AddToDictionary(lower_k, sub_cipher_start, ref dict);

                    if (!equal)
                        continue;

                    foreach (var c in ciphertext)
                    {
                        if (dict.ContainsKey(c))
                            pt += dict[c];
                        else
                            pt += c;
                    }
                    writer.WriteLine(pt);
                    RemoveFromDictionary(lower_k, ref dict);

                }
            }
            writer.Close();
        }

        public bool AddToDictionary(string word, string sub_cipher, ref Dictionary<char, char> dict)
        {
            var equal = true;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!dict.ContainsKey(sub_cipher[i]))
                {
                    if (!dict.ContainsValue(word[i]))
                    {
                        dict.Add(sub_cipher[i], word[i]);
                    }
                    else
                    {
                        equal = false;
                        break;
                    }
                }
                else
                {
                    if (dict[sub_cipher[i]] != word[i])
                    {
                        equal = false;
                        break;
                    }
                }
            }

            return equal;
        }

        public void RemoveFromDictionary(string word, ref Dictionary<char, char> dict)
        {
            foreach(var c in String.Join("", word.Distinct()))
            {
                dict.Remove(c);
            }
        }
    }
}
