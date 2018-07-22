using System;
using System.Collections;
using System.Collections.Generic;

namespace BKTree{
    class Program{
        static void Main(string[] args){
			MatchTree<string> m = new MatchTree<string>();

			string file = "words.txt";

			foreach (string word in ReadFile.readFile(file)) {
				m.add(word);
			}

			Dictionary<string, int> matches = m.query("feed", 2);

			foreach (KeyValuePair<string, int> pair in matches) {
				Console.WriteLine(pair);
			}
		}
    }
}
