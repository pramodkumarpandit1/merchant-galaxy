using MerchantGalaxyLib;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var translator = new Translator();

            while (true)
            {
                Console.Write("Hello Galaxy Merchant, Please enter sentence? ");
                var sentence = Console.ReadLine();

                if (sentence == "exit") break;

                var result = translator.ParseAndExecute(sentence);
                Console.WriteLine(result.ResultText);
            }
        }
    }
}
