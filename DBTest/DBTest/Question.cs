using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
	public class Question
	{
		public string question;
		public List<KeyValuePair<bool, string>> answers;
		public Question(string t_question, List<KeyValuePair<bool, string>> t_answers)
		{
			question = t_question;
			answers = new List<KeyValuePair<bool, string>>(t_answers);
		}
	}
}
