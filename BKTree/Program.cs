using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BKTree{
    class Program{

        const int MAX_INPUT_LENGTH = 16;

        static void Main(string[] args){
			MatchTree<string> m = new MatchTree<string>();

			string file = "words.txt";

            Console.WriteLine("Reading file...");

            // Reads each word from the input file (dictionary)
            List<string> readWords = ReadFile.readFile(file);
            // Converts all the words to lower case
            readWords = readWords.ConvertAll(s => s.ToLower());
            // Adds the read words to our autocomplete tree
            m.add(readWords);

            Console.WriteLine("Finished reading");

            string inputString = "";
            while (true) {
                ConsoleKeyInfo cki = Console.ReadKey(true);

                Console.Clear();

                // Only add if we didn't hit the maximum input length
                if (inputString.Length <= MAX_INPUT_LENGTH)
                    inputString += cki.KeyChar;

                // If the user deleted a char we need to remove it from the input string
                if (cki.Key == ConsoleKey.Backspace ||
                    cki.Key == ConsoleKey.Delete) {
                    if (inputString.Length > 1)
                        inputString = inputString.Remove(inputString.Length - 2);
                }

                Console.WriteLine("Input: " + inputString);
                if (inputString.Length == 0) continue;

                Dictionary<string, int> matches = m.query(inputString, inputString.Length + 1);

                // Using LINQ sorts the dictionary by value
                var items = from pair in matches orderby pair.Value ascending select pair;
                //foreach (KeyValuePair<string, int> p in items) {
                //    Console.WriteLine(p.Key + "[" + p.Value + "]");
                //}
                for (int i = 0; i < 3; i++)
                    if (items.Count() - 1 >= i)
                        Console.WriteLine("- " + items.ElementAt(i).Key);
            }
		}
    }
}
