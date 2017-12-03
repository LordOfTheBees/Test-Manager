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
using System.Windows.Shapes;

namespace DBTest
{
	/// <summary>
	/// Логика взаимодействия для Result.xaml
	/// </summary>
	public partial class Result :Window
	{
		private int choise_situation = 1;
		public Result()
		{
			InitializeComponent();
		}

		public int ShowDialog(List<bool> answers)
		{
			int num_of_wrong = 0;
			foreach (bool x in answers)
			{
				if (!x)
					num_of_wrong++;
			}

			label_result.Content = "" + num_of_wrong + '/' + answers.Count + " были ошибочны";
			if (num_of_wrong == 0)
			{
				button_start_test_with_wrong_answer.IsEnabled = false;
			}
			this.ShowDialog();
			return choise_situation;
		}

		private void button_new_test_Click(object sender, RoutedEventArgs e)
		{
			choise_situation = 1;
			this.Close();
		}

		private void button_repeat_test_Click(object sender, RoutedEventArgs e)
		{
			choise_situation = 2;
			this.Close();
		}

		private void button_start_test_with_wrong_answer_Click(object sender, RoutedEventArgs e)
		{
			choise_situation = 3;
			this.Close();
		}
	}
}
