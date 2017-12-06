using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace E_Test
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow :Window
	{
		private Result result;
		private List<KeyValuePair<string, string>> test_list;
		private List<WordSet> questions;
		private List<bool> select_answers;

		private int num_of_question;

		public MainWindow()
		{
			questions = FileHelper.GetQuestionos("questions.txt");
			test_list = new List<KeyValuePair<string, string>>(questions.Count);
			select_answers = new List<bool>(questions.Count);
			result = new Result();

			InitializeComponent();

			button_check_and_next.IsEnabled = false;
			button_hint_and_next.IsEnabled = false;
		}

		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			button_check_and_next.IsEnabled = true;
			button_hint_and_next.IsEnabled = true;

			StartNewTest();

			button_check_and_next.Click += btn_Check;
			button_hint_and_next.Click += btn_Hint;
		}

		private void MakeChoise(int i)
		{
			switch (i)
			{
				case 1:
					StartNewTest();
					break;
				case 2:
					Repeat();
					break;
				case 3:
					CorrectMistakes();
					break;
				default:
					StartNewTest();
					break;
			}
		}
		private void StartNewTest()
		{
			test_list.Clear();
			select_answers.Clear();
			if (rad_but_eng_to_rus.IsChecked == true)
			{
				foreach (WordSet x in questions)
				{
					test_list.Add(new KeyValuePair<string, string>(x.english_word, x.russian_word));
				}
			}
			else
			{
				foreach (WordSet x in questions)
				{
					test_list.Add(new KeyValuePair<string, string>(x.russian_word, x.english_word));
				}
			}

			test_list = Randomizer.ShuffleList(test_list);
			num_of_question = 0;
			PrintQuestion();
		}
		private void Repeat()
		{
			select_answers.Clear();
			test_list = Randomizer.ShuffleList(test_list);
			num_of_question = 0;
			PrintQuestion();
		}
		private void CorrectMistakes()
		{
			List<KeyValuePair<string, string>> question_with_mistakes = new List<KeyValuePair<string, string>>();

			for (int i = 0; i < select_answers.Count; ++i)
			{
				if (!select_answers[i])
					question_with_mistakes.Add(new KeyValuePair<string, string>(test_list[i].Key, test_list[i].Value));
			}

			select_answers.Clear();
			test_list = Randomizer.ShuffleList(question_with_mistakes);
			num_of_question = 0;
			PrintQuestion();
		}

		private void PrintQuestion()
		{
			label_progress.Content = "" + (num_of_question + 1) + '/' + test_list.Count;
			label_question.Content = test_list[num_of_question].Key;
			text_box_answer.Text = "";
		}


		private void btn_Next(object sender, RoutedEventArgs e)
		{
			text_box_answer.IsEnabled = true;
			num_of_question++;

			button_check_and_next.Content = "Check";
			button_hint_and_next.Content = "Hint";

			button_check_and_next.Click -= btn_Next;
			button_hint_and_next.Click -= btn_Next;

			button_check_and_next.Click += btn_Check;
			button_hint_and_next.Click += btn_Hint;

			if (num_of_question == test_list.Count)
			{
				MakeChoise(result.ShowDialog(select_answers));
				result = new Result();
				return;
			}

			PrintQuestion();
		}

		private void btn_Hint(object sender, RoutedEventArgs e)
		{
			button_check_and_next.Content = "Next";
			button_hint_and_next.Content = "Next";

			button_check_and_next.Click -= btn_Check;
			button_hint_and_next.Click -= btn_Hint;

			button_check_and_next.Click += btn_Next;
			button_hint_and_next.Click += btn_Next;

			text_box_answer.IsEnabled = false;

			label_loger.Foreground = Brushes.Red;
			label_loger.Content = "Ответ будет незасчитан! Посмотрите и запомните правильный";
			select_answers.Add(false);

			text_box_answer.Text = test_list[num_of_question].Value;
		}

		private void btn_Check(object sender, RoutedEventArgs e)
		{
			button_check_and_next.Content = "Next";
			button_hint_and_next.Content = "Next";

			button_check_and_next.Click -= btn_Check;
			button_hint_and_next.Click -= btn_Hint;

			button_check_and_next.Click += btn_Next;
			button_hint_and_next.Click += btn_Next;

			text_box_answer.IsEnabled = false;
			if (text_box_answer.Text.ToLower() == test_list[num_of_question].Value)
			{
				label_loger.Foreground = Brushes.LightGreen;
				label_loger.Content = "Ответ верен!";
				select_answers.Add(true);
			}
			else
			{
				label_loger.Foreground = Brushes.Red;
				label_loger.Content = "Ответ неверен! Посмотрите и запомните правильный";
				select_answers.Add(false);

				text_box_answer.Text = test_list[num_of_question].Value;
			}
		}
	}
}
