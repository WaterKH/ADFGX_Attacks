using System;
using ADFGXAttacks.DictionaryDictionaryAttack;

namespace ADFGXAttacks
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Index_Length_Word attack = new Index_Length_Word();
            attack.Attack("wordlist.txt");
        }
    }
}
