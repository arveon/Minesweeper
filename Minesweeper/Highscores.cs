using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
	struct Record
	{
		public string name;
		public int score;
	}

	class Highscores
	{
		LinkedList<Record> top;
		string file;

		public Highscores(string file)
		{
			this.file = file;
			top = new LinkedList<Record>();
		}


		public bool isHighscore(int score)
		{
			if (top.Count > 0)
			{
				if (top.Count < Constants.HIGHSCORES_NUMBER)
					return true;

				foreach (Record r in top)
					if (score >= r.score)
						return true;
			}
			else
				return true;

			return false;
		}

		public void addHighscore(int score, string name)
		{
			Record temp = new Record();
			temp.score = score;
			temp.name = name;

			if(top.Count == 0)
			{
				top.AddLast(temp);
			}
			else
			{
				LinkedListNode<Record> curNode = top.First;
				bool found = false;
				while(!found && curNode != null)
				{
					if(curNode.Value.score <= temp.score)
					{
						found = true;
						break;
					}
					curNode = curNode.Next;
				}

				if (found)
				{
					if (curNode.Value.score == temp.score)
						top.AddBefore(curNode, temp);
					else if (curNode.Value.score < temp.score)
						top.AddBefore(curNode, temp);
				}
				else
					if (top.Count <= Constants.HIGHSCORES_NUMBER)
						top.AddLast(temp);
			}


			//pop off the back until highscores are small enough
			while (top.Count > Constants.HIGHSCORES_NUMBER)
				top.RemoveLast();
			
		}

		public void saveHighscores()
		{
			string fileline = "";
			foreach(Record r in top)
			{
				fileline += r.name + "," + r.score + "\n";
			}

			System.IO.StreamWriter file = new System.IO.StreamWriter(this.file);
			file.WriteLine(fileline);
			file.Close();
		}

		public void loadHighscores()
		{
			try 
			{
				System.IO.StreamReader file = new System.IO.StreamReader(this.file);
				string line;
				int counter = 0;
				while ((line = file.ReadLine()) != null)
				{
					if (line != "")
					{
						string[] parsedValue = line.Split(',');

						Record temp = new Record();
						temp.name = parsedValue[0];

						try
						{
							this.addHighscore(Convert.ToInt32(parsedValue[1]), parsedValue[0]);
						}
						catch (FormatException)
						{
							continue;
						}
						counter++;
					}
				}
				file.Close();
			}
			catch(Exception)
			{
				return;
			}
			

			
		}

		public string getHighscoresLine()
		{
			string line = "";
			int counter = 1;
			foreach(Record r in top)
			{
				line += counter + ". " + r.name + " " + r.score + "\n";
				counter++;
			}
			return line;
		}

		//testing method
		public void printHighscores()
		{
			Console.WriteLine("HIGHSCORES (" + top.Count + ")");
			foreach(Record r in top)
			{
				Console.Write(r.name);
				Console.WriteLine(" - " + r.score);
			}
		}

		//method for testing
		public void AddSample()
		{
			this.addHighscore(47, "asd1");
			this.addHighscore(71, "asd2");
			this.addHighscore(31, "asd3");
			this.addHighscore(47, "asd4");
			this.addHighscore(50, "asd5");
			this.addHighscore(62, "asd6");
		}
	}
}
