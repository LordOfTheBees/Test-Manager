using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
	public static class FileHelper
	{
		public static List<Question> GetQuestionos(string path_to_file)
		{
			StreamReader file = new StreamReader(path_to_file, Encoding.GetEncoding(1251));
			string line;

			List<Question> questions = new List<Question>();
			List<KeyValuePair<bool, string>> answers;

			string text_question;

			bool answer_checker;
			string answer_text;

			bool answer_reading, question_reading;
			while ((line = file.ReadLine()) != null)
			{
				if (line == "")
					continue;
				if (line[0] == '/' && line[1] == '\\')
				{
					answers = new List<KeyValuePair<bool, string>>();
					text_question = "";
					answer_text = "";
					answer_checker = false;

					line = line.Remove(0, 2);
					text_question += line;

					question_reading = true;
					answer_reading = false;
					while (true)
					{
						line = file.ReadLine();
						if (line == "")
						{
							continue;
						}
						if (question_reading == true && answer_reading == false && line[0] != '+' && line[0] != '-')
						{
							text_question = text_question + " " + line;
						}
						else if (line[0] == '+')
						{ 
							if (answer_reading == true)
							{
								answers.Add(new KeyValuePair<bool, string>(answer_checker, answer_text));
								answer_text = "";
							}
							answer_reading = true;
							answer_checker = true;
							answer_text = line.Remove(0,1);
						}
						else if (line[0] == '-')
						{
							if (answer_reading == true)
							{
								answers.Add(new KeyValuePair<bool, string>(answer_checker, answer_text));
								answer_text = "";
							}
							answer_reading = true;
							answer_checker = false;
							answer_text = line.Remove(0,1);
						}
						else if (line[0] == '\\' && line[1] == '/')
						{
							answers.Add(new KeyValuePair<bool, string>(answer_checker, answer_text));
							questions.Add(new Question(text_question, answers));
							break;
						}
						else if (answer_reading == true)
						{
							answer_text = answer_text + " " + line;
						}

					}
				}
			}
			return questions;
		}
	}
}
