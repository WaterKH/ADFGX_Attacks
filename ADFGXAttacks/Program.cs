using System;
using ADFGXAttacks.DictionaryDictionaryAttack;
using ADFGXAttacks.EndOfTextWordAttack;
using ADFGXAttacks.MiddleAttack;
using System.IO;

namespace ADFGXAttacks
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string ciphertext = "IXSODNOIINXXSNRSNRNSRASOWXPOKXNSQL";

            MiddlePatternAttack mpa = new MiddlePatternAttack();
            mpa.Setup(ciphertext);
            Console.WriteLine("Finished!");

            //EndAttack ea = new EndAttack();
            //ea.Attack(ciphertext, "wordsEn+Zombie.txt");

            //Index_Length_Word attack = new Index_Length_Word();
            //attack.Setup("words_test.txt");
            //attack.Attack();
        }
    }
}
