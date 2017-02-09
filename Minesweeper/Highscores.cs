using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
	//struct represents a single line within the highscore table
	struct Record
	{
		public string name;
		public int score;
	}

	//class represents a collective table of game highscores
	class Highscores
	{
		LinkedList<Record> top;//data structure to hold the highscores
		string file;//path to file to load/save highscores to/from
		
		//will initialise highscores, will save/load them from `file` filepath whenever appropriate methods called
		public Highscores(string file)
		{
			//initialise fields
			this.file = file;
			top = new LinkedList<Record>();
		}

		//returns whether the `score` can be considered a highscore or not
		//true if it is higher than some of the valuesin the table
		//false if all of the scores stored are higher than `score`
		public bool isHighscore(int score)
		{
			//if there are records in the table, check further
			if (top.Count > 0)
			{
				//if there are currently less highscores in the table than the limit, consider any score a highscore
				if (top.Count < Constants.HIGHSCORES_NUMBER)
					return true;

				//loop through the table top to bottom
				foreach (Record r in top)
					if (score >= r.score)//if there is a value lower than `score`, it is a highscore
						return true;
			}
			else//if there is no records in the table, any score is a highscore
				return true;

			return false;//if none of the above, it's not a highscore
		}

		//method adds the new highscore to the table
		//score - the score to add
		//name - name of the player
		public void addHighscore(int score, string name)
		{
			//create a new instance of the struct
			Record temp = new Record();
			temp.score = score;
			temp.name = name;

			//if no records present, just add it
			if(top.Count == 0)
			{
				top.AddLast(temp);
			}
			else
			{
				//loop through all of the nodes and find the one with lower or equal value
				LinkedListNode<Record> curNode = top.First;
				bool found = false;
				while(!found && curNode != null)//loop until the correct node found or until end of list reached
				{
					if(curNode.Value.score <= temp.score)
					{//if the node's value is <= to given score, break out of loop
						found = true;
						break;
					}
					curNode = curNode.Next;
				}

				if (found)
				{//if node with lower or equal value found, add it to list at it's place
					//if node value is equal, add the new value above it, otherwise add below
					if (curNode.Value.score == temp.score)
						top.AddBefore(curNode, temp);
					else if (curNode.Value.score < temp.score)
						top.AddBefore(curNode, temp);
				}
				else//if node with lower value wasn't found, only add if the table is not full
					if (top.Count <= Constants.HIGHSCORES_NUMBER)
						top.AddLast(temp);
			}

			//pop off the back until highscores are small enough
			while (top.Count > Constants.HIGHSCORES_NUMBER)
				top.RemoveLast();
			
		}

		//method saves highscores table into a file
		public void saveHighscores()
		{
			//form the line containing all of the highscores
			string fileline = "";
			foreach(Record r in top)
			{
				fileline += r.name + "," + r.score + "\n";
			}

			//create a stream with a file at `file` filepath
			System.IO.StreamWriter file = new System.IO.StreamWriter(this.file);
			file.WriteLine(fileline);
			file.Close();
		}

		//load highscores from a file
		public void loadHighscores()
		{
			//needs a try catch block in case file doesn't exist (doesn't load highscores in that case)
			try 
			{
				//establish the stream
				System.IO.StreamReader file = new System.IO.StreamReader(this.file);
				string line;
				//read file line by line until end reached
				while ((line = file.ReadLine()) != null)
				{
					//if line isn't empty, parse values
					if (line != "")
					{
						//parse values into array
						string[] parsedValue = line.Split(',');

						//try catch needed in case invalid information is input into the file where number expected
						try
						{
							//add the record to table
							this.addHighscore(Convert.ToInt32(parsedValue[1]), parsedValue[0]);
						}
						catch (FormatException)
						{//if invalid score, skip to next line
							continue;
						}
					}
				}
				file.Close();
			}
			catch(Exception)
			{
				return;
			}
		}

		//returns all of the highscores data in a single string variable
		public string getHighscoresLine()
		{
			string line = "";
			int counter = 1;
			//loop through every table record and add it to a new line in string
			foreach(Record r in top)
			{
				line += counter + ". " + r.name + " " + r.score + "\n";
				counter++;
			}
			return line;
		}


		#region testing methods - NOT USED ANYMORE ( ONLY FOR HISTORY PURPOSES:) )
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
		#endregion
	}
}
