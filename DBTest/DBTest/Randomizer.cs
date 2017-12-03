using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
	public static class Randomizer
	{
		public static List<int> GetRandomList(int sample_size, int volume_general_set)
		{
			Random rand = new Random();
			List<int> randomizer = new List<int>(volume_general_set);
			for (int i = 0; i < volume_general_set; ++i)
			{
				randomizer.Add(i);
			}

			List<int> select_number = new List<int>(sample_size);
			int tmp_number;

			for (int i = 0; i < sample_size; ++i)
			{
				tmp_number = rand.Next(randomizer.Count);
				select_number.Add(randomizer[tmp_number]);
				randomizer.RemoveAt(tmp_number);
			}

			return select_number;
		}

		public static List<T> ShuffleList<T>(List<T> array)
		{
			List<T> new_list = new List<T>(array.Count);

			List<int> random_list = GetRandomList(array.Count, array.Count);

			foreach (int x in random_list)
			{
				new_list.Add(array[x]);
			}

			return new_list;
		}
	}
}
