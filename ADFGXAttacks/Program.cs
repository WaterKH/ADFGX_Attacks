using System;
using ADFGXAttacks.DictionaryDictionaryAttack;
using ADFGXAttacks.EndOfTextWordAttack;

namespace ADFGXAttacks
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string ciphertext = "IXSODNOIINXXSNRSNRNSRASOWXPOKXNSQL";
            EndAttack ea = new EndAttack();
            ea.Attack(ciphertext, "wordsEn+Zombie.txt");

            //Index_Length_Word attack = new Index_Length_Word();
            //attack.Setup("words_test.txt");
            //attack.Attack();
        }
    }
}
