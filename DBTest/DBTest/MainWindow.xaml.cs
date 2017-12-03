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

namespace DBTest
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow :Window
	{
		private Result result;
		private List<Question> questions;
		private List<Question> select_questions;
		private List<int> select_number_of_questions;
		List<int> random_selector;
		List<Button> buttons;
		private List<bool> select_answer;		private int num_of_question;
		private bool test_start;
		public MainWindow()
		{
			result = new Result();
			questions = FileHelper.GetQuestionos("questions.txt");
			test_start = false;

			InitializeComponent();

			label_progress.Content = "0/0";
			button_hint_and_next.IsEnabled = false;
			button_hint_and_next.Click += Hint_Question_Click;
		}

		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			NewTest();
		}

		private void NextStep(int choise)
		{
			if (choise == 1)
				NewTest();
			else if (choise == 2)
				Repeat();
			else if (choise == 3)
				CorrectionOfMistakes();

		}


		private void NewTest()
		{
			label_loger.Foreground = Brushes.Black;
			label_loger.Content = "Powered by Slava";
			int numer = Convert.ToInt32(text_box_num_of_question.Text);

			if (numer > questions.Count)
			{
				test_start = false;
				label_loger.Content = "Число тестов на выборку неверно. Максимальное кол-во: " + questions.Count;
				label_loger.Foreground = Brushes.Red;
				return;
			}
			else if (numer < 1)
			{
				test_start = false;
				label_loger.Content = "Число тестов не может быть нулём или отрицательным числом";
				label_loger.Foreground = Brushes.Red;
				return;
			}

			test_start = true;
			button_hint_and_next.IsEnabled = true;
			select_number_of_questions = Randomizer.GetRandomList(Convert.ToInt32(text_box_num_of_question.Text), questions.Count);

			select_questions = new List<Question>(select_number_of_questions.Count);
			select_answer = new List<bool>(select_number_of_questions.Count);
			for (int i = 0; i < select_number_of_questions.Count; ++i)
			{
				select_questions.Add(questions[select_number_of_questions[i]]);
			}

			num_of_question = 0;
			PrintQuestion(num_of_question);
		}
		private void Repeat()
		{
			label_loger.Foreground = Brushes.Black;
			label_loger.Content = "Powered by Slava";

			test_start = true;
			button_hint_and_next.IsEnabled = true;
			select_answer.Clear();

			select_questions = Randomizer.ShuffleList(select_questions);

			num_of_question = 0;
			PrintQuestion(num_of_question);
		}
		private void CorrectionOfMistakes()
		{
			label_loger.Foreground = Brushes.Black;
			label_loger.Content = "Powered by Slava";

			test_start = true;
			button_hint_and_next.IsEnabled = true;
			List<Question> question_with_mistake = new List<Question>();

			for (int i = 0; i < select_answer.Count; ++i)
			{
				if (!select_answer[i])
				{
					question_with_mistake.Add(select_questions[i]);
				}
			}

			select_questions = Randomizer.ShuffleList(question_with_mistake);
			select_answer.Clear();

			num_of_question = 0;
			PrintQuestion(num_of_question);
		}

		private void PrintQuestion(int t_num_of_question)
		{
			stack_panel_question.Children.Clear();
			label_progress.Content = "" + (t_num_of_question + 1) + "/" + select_questions.Count;

			//перемешиваем ответы
			random_selector = Randomizer.GetRandomList(select_questions[t_num_of_question].answers.Count, select_questions[t_num_of_question].answers.Count);

			List<KeyValuePair<bool, string>> copy_answers = new List<KeyValuePair<bool, string>>();
			for (int i = 0; i < random_selector.Count; ++i)
			{
				copy_answers.Add(new KeyValuePair<bool, string>(select_questions[t_num_of_question].answers[random_selector[i]].Key, select_questions[t_num_of_question].answers[random_selector[i]].Value));
			}
			select_questions[t_num_of_question].answers = new List<KeyValuePair<bool, string>>(copy_answers);
			buttons = GenerateButton(select_questions[t_num_of_question].answers.Count);

			stack_panel_question.Children.Add(GetContent(select_questions[t_num_of_question].question));
			for (int i = 0; i < buttons.Count; ++i)
			{
				buttons[i].Content = GetContent("" + (i + 1) + ")" + select_questions[t_num_of_question].answers[i].Value);
				stack_panel_question.Children.Add(buttons[i]);
			}
		}

		private List<Button> GenerateButton(int num_of_buttons)
		{
			Button tmp_button;
			List<Button> list_of_buttons = new List<Button>(num_of_buttons);
			for (int i = 0; i < num_of_buttons; ++i)
			{
				tmp_button = new Button();
				tmp_button.Margin = new Thickness(5, 5, 5, 5);
				tmp_button.HorizontalContentAlignment = HorizontalAlignment.Left;
				tmp_button.Click += Tmp_button_Click;

				list_of_buttons.Add(tmp_button);
			}
			return list_of_buttons;
		}

		private void Tmp_button_Click(object sender, RoutedEventArgs e)
		{
			button_hint_and_next.Click -= Hint_Question_Click;
			button_hint_and_next.Click += Next_Question_Click;
			button_hint_and_next.Content = "Next";

			string content = ((TextBlock)((Button)sender).Content).Text;
			content = content.Split(')')[0];

			label_loger.Foreground = Brushes.Black;
			label_loger.Content = "Powered by Slava";
			int num_of_answer = Convert.ToInt32(content);
			if (num_of_answer < 0 || num_of_answer > select_questions[num_of_question].answers.Count)
			{
				label_loger.Foreground = Brushes.Red;
				label_loger.Content = "This answer does not exist. Change your choise!";
				return;
			}
			else
			{
				select_answer.Add(select_questions[num_of_question].answers[num_of_answer - 1].Key);
			}

			ShowAnswer();
		}


		private void Next_Question_Click(object sender, RoutedEventArgs e)
		{
			button_hint_and_next.Click -= Next_Question_Click;
			button_hint_and_next.Click += Hint_Question_Click;
			button_hint_and_next.Content = "Hint";

			num_of_question++;

			if (num_of_question == select_questions.Count)
			{
				test_start = false;
				button_hint_and_next.IsEnabled = false;
				stack_panel_question.Children.Clear();
				NextStep(result.ShowDialog(select_answer));
				result = new Result();
				return;
			}

			PrintQuestion(num_of_question);
		}

		private void Hint_Question_Click(object sender, RoutedEventArgs e)
		{
			button_hint_and_next.Click -= Hint_Question_Click;
			button_hint_and_next.Click += Next_Question_Click;
			Button tmp_btn = (Button)sender;

			ShowAnswer();
			select_answer.Add(false);
		}

		private TextBlock GetContent(string text)
		{
			TextBlock text_block = new TextBlock();
			text_block.TextWrapping = TextWrapping.Wrap;
			text_block.Margin = new Thickness(5, 5, 5, 5);
			text_block.FontSize = 15;
			text_block.Text = text;

			return text_block;
		}

		private void ShowAnswer()
		{
			for (int i = 0; i < buttons.Count; ++i)
			{
				buttons[i].Click -= Tmp_button_Click;
				buttons[i].Click += (s, e) =>
				{
				};
				if (select_questions[num_of_question].answers[i].Key)
					buttons[i].Background = Brushes.LightGreen;
				else
					buttons[i].Background = Brushes.IndianRed;
			}
		}
	}
}
