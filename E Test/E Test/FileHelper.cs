using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Test
{
	public static class FileHelper
	{
		public static List<WordSet> GetQuestionos(string path_to_file)
		{
			StreamReader file = new StreamReader(path_to_file, Encoding.GetEncoding(1251));
			string line;
			string[] parse;

			List<WordSet> questions = new List<WordSet>();

			while ((line = file.ReadLine()) != null)
			{
				parse = line.Split(';');
				if (parse.Length!=2)
					continue;
				questions.Add(new WordSet(parse[0].ToLower(), parse[1].ToLower()));
			}
			return questions;
		}
	}
}
