using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BKTree
{
    class ReadFile
    {

		public static List<string> readFile(string path) {
			List<string> words = new List<string>();

			foreach (string line in File.ReadLines(path)) {
				words.Add(line);
			}

			return words;
		}

    }
}
