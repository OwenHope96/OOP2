using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press t for tests, to play the game type anything else");
            string Choice = Console.ReadLine();
            if (Choice == "t")
            {
                testing Game1 = new testing();
            } else
            {
                Game Game1 = new Game();
            }
            
        }
    }

    public class testing
    {
        public testing()
        {
            Game GameTest = new Game(true);

        }
    }

    public class Statistics
    {
        //Stat file for Three or more
        //PlayerScore --> EnemyScore --> Number of turns

        //Stat file for 7s out
        //PlayerScore --> EnemyScore --> Number of rolls

        private string path = "StatTxt.txt";
        private int Highscore;
        private int NumberOfPlays;
        private int AverageScorePlayer;
        private int AverageScoreEnemy;
        private int AverageNumberOfTurns;

        private List<string> Collumn1 = new List<string>();
        private List<string> Collumn2 = new List<string>();
        private List<string> Collumn3 = new List<string>();

        public Statistics(int Number = 1)
        {
            SetStats(Number);
        }

        public void SetStats(int Number)
        {
            ChangePath(Number);
            GSHighscore = GetHighScore();
            GSNumberOfPlays = NumberOfPlayes();
            GSAverageNumberOfTurns = GetAverage(3);
            GSAverageScoreEnemy = GetAverage(2);
            GSAverageScorePlayer = GetAverage(1);
        }

        public void RunStats()
        {
            if (path == "StatTxt.txt")
            {
                Console.WriteLine("Highscore for player is " + Highscore);
                Console.WriteLine("Average score for the player is " + AverageScorePlayer);
                Console.WriteLine("Average score for the enemy/Player2 is " + AverageScoreEnemy);
                Console.WriteLine("Average number of turns is " + AverageNumberOfTurns);
                Console.WriteLine("Total number of plays is " + NumberOfPlays);
                Console.WriteLine();
                Console.WriteLine("Player scores over 22: ");
                
                var PrintedItems = from thing in Collumn1
                                   where Convert.ToInt32(thing) > 22
                                   select thing;
                foreach (string thing in PrintedItems) { Console.WriteLine(thing); }


            } else
            {
                Console.WriteLine("Highscore for player is " + Highscore);
                Console.WriteLine("Average score for the player is " + AverageScorePlayer);
                Console.WriteLine("Average score for the enemy/Player2 is " + AverageScoreEnemy);
                Console.WriteLine("Average number of dice rolls per game is " + AverageNumberOfTurns);
                Console.WriteLine("Total number of plays is " + NumberOfPlays);
                Console.WriteLine();
                Console.WriteLine("Everytime the game went over 40 turns");

                var PrintedItems = from thing in Collumn3
                                   where Convert.ToInt32(thing) > 40
                                   select thing;
                foreach (string thing in PrintedItems) { Console.WriteLine(thing); }

            }


        }


        public void ChangePath(int Number = 1)
        {
            if (Number == 0)
            {
                path = "Stats7.txt";
            } else
            {
                path = "StatTxt.txt";
            }
        }

        public void AddToFile(string Content)
        {
            File.AppendAllText(path, Content);
        }

        public int GetAverage(int Number)
        
        {
            int tally = 0;
            int temp = 1;
            string TempString = "";
            string[] LineArray = File.ReadAllLines(path);

            foreach (string line in LineArray)
            {
                temp = 1;
                for (int i = 0; i < line.Length; i++)
                {

                    if (temp == Number)
                    {
                        while (Convert.ToString(line[i]) != " ")
                        {
                            TempString = TempString + line[i];
                            i++;
                            if (i > line.Length - 1)
                            {
                                break;
                            }
                            if (Convert.ToString(line[i]) == " ")
                            {
                                break;
                            }
                        }
                        if (Number == 1)
                        {
                            Collumn1.Add(TempString);
                        } else if (Number == 2)
                        {
                            Collumn2.Add(TempString);
                        } else
                        {
                            Collumn3.Add(TempString);
                        }
                    }

                    if (i > line.Length - 1)
                    {
                        break;
                    }

                    if (Convert.ToString(line[i]) == " ")
                    {
                        temp++;
                    }
                }
                tally += Convert.ToInt32(TempString);
                TempString = "";
            }

            int returnval = 0;
            
            try
            {
                returnval = tally / (LineArray.Length - 1);
            }
            catch (DivideByZeroException)
            {
                returnval = 0;
            }

            return returnval;
        }

        public int GSHighscore
        {
            get { return Highscore; }
            set { Highscore = value; }
        }

        public int GSNumberOfPlays
        {
            get { return NumberOfPlays; }
            set { NumberOfPlays = value; }
        }

        public int GSAverageScorePlayer
        {
            get { return AverageScorePlayer; }
            set { AverageScorePlayer = value; }
        }
        public int GSAverageScoreEnemy
        {
            get { return AverageScoreEnemy; }
            set { AverageScoreEnemy = value; }
        }
        public int GSAverageNumberOfTurns
        {
            get { return AverageNumberOfTurns; }
            set { AverageNumberOfTurns = value; }
        }


        public int NumberOfPlayes()
        {
            string[] LineArray = File.ReadAllLines(path);
            NumberOfPlays = (LineArray.Length - 1);
            return NumberOfPlays;
        }

        public int GetHighScore()
        {
            int temp = 0;
            string TempString = "";
            string[] LineArray = File.ReadAllLines(path);
            foreach (string line in LineArray)
            {
                if (line == "")
                {
                    break;
                }
                int i = 0;
                while (Convert.ToString(line[i]) != " ")
                {
                    TempString = TempString + line[i];
                    i++;
                }
                if (Convert.ToInt32(TempString) > temp)
                {
                    temp = Convert.ToInt32(TempString);
                }
                TempString = "";
            }
            Highscore = temp;
            return Highscore;
        }

        

    }

    public class SevensOut : AGame
    {
        private int NumberOfRolls = 0;
        private int RollValue = 0;
        public SevensOut(bool PBool, bool test = false)
        {
            Statistics stats = new Statistics(0);

            NumberOfDie = 2;

            PlayerPoints = RollTime(test);
            EnemyPoints = RollTime(test);
            string StatsThing = Convert.ToString(PlayerPoints) + " " + Convert.ToString(EnemyPoints) + " " + Convert.ToString(NumberOfRolls) + Environment.NewLine;
            stats.AddToFile(StatsThing);



        }


        public int GSRollValue
        {
            get { return RollValue; }
        }
        

        private int RollTime(bool test = false)
        {
            int tally = 0;
            int RollNum = 0;
            int subtally = 0;
            if (test == false)
            {
                Console.WriteLine("Roll time");
                while (subtally != 7)
                {
                    subtally = 0;
                    subtally = Dice1.RollDie() + Dice2.RollDie();
                    Console.WriteLine(Dice1.GetRoll() + " is roll 1, " + Dice2.GetRoll() + " is roll 2.");
                    RollNum += 2;
                    if (subtally == 7)
                    {
                        Console.WriteLine("Rolled 7, will stop rolling");
                    }
                    else if (Dice1.GetRoll() == Dice2.GetRoll())
                    {
                        Console.WriteLine("Double!");
                        subtally = subtally * 2;
                    }
                    Console.WriteLine("Adding " + subtally);
                    tally += subtally;
                    RollValue = subtally;
                }
                Console.WriteLine("You got " + tally);
                Console.ReadLine();
                NumberOfRolls += RollNum;
            } else
            {
                while (subtally != 7)
                {
                    subtally = 0;
                    subtally = Dice1.RollDie() + Dice2.RollDie();
                    RollNum += 2;
                    if (subtally == 7)
                    {
                        
                    }
                    else if (Dice1.GetRoll() == Dice2.GetRoll())
                    {
                        subtally = subtally * 2;
                    }
                    tally += subtally;
                    RollValue = subtally;
                }
                NumberOfRolls += RollNum;
            }
            return tally;
        }



    }

    public class ThreeOrMore : AGame
    {
        int[] RollList = new int[5];
        private int RoundTotal = 0;
        public ThreeOrMore(bool PBool, bool test = false)
        {
            VsComputer = PBool;
            NumberOfDie = 5;
            Die Dice3 = new Die();
            Die Dice4 = new Die();
            Die Dice5 = new Die();
            if (test == false)
            {
                Console.WriteLine("Roll time");
            }
            RollTime(Dice3, Dice4, Dice5, test);


        }

        public int PointsCalc()
        {
            int occurance = 0;
            int tally = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (RollList[i] == RollList[j])
                    {
                        occurance++;
                    }
                }
                if (occurance == 3)
                {
                    tally = 3;
                }
                else if (occurance == 4)
                {
                    tally = 6;
                }
                else if (occurance == 5)
                {
                    tally = 12;
                }
                occurance = 0;
            }
            return tally;
        }


        public int GetPlayerPoints()
        {
            return PlayerPoints;
        }

        public int GetEnemyPoints()
        {
            return EnemyPoints;
        }
        public int[] GetRollList
        {
            get { return RollList; }
        }

        public int GetRoundTotal () { return RoundTotal; }
        private void RollTime(Die Dice3, Die Dice4, Die Dice5, bool test = false)
        {

            Statistics stats = new Statistics();

            Console.WriteLine(stats.GetAverage(3));

            int occurance = 0;
            int turn = 0;
            int tally = 0;
            bool Reroll = false;
            int RerollCounter = 0;
            PlayerPoints = 0;
            EnemyPoints = 0;

            Console.Clear();

            while (GameWin() == false)
            {
                if (Reroll == false)
                {
                    RollList[0] = Dice1.RollDie();
                    RollList[1] = Dice2.RollDie();
                    RollList[2] = Dice3.RollDie();
                    RollList[3] = Dice4.RollDie();
                    RollList[4] = Dice5.RollDie();
                    RerollCounter = 0;
                } else
                {
                    RerollCounter = 1;
                    Reroll = false;
                }
                if (test == false)
                {
                    PrintList();
                }
                

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (RollList[i] == RollList[j])
                        {
                            occurance++;
                        }
                    }
                    if (occurance == 3)
                    {
                        tally = 3;
                    }
                    else if (occurance == 4)
                    {
                        tally = 6;
                    } else if (occurance == 5)
                    {
                        tally = 12;
                    } else if (occurance == 2 && RerollCounter == 0 )
                    {
                        
                        if (VsComputer == false || turn % 2 == 0)
                        {

                            string choice = "";
                            if (test == false)
                            {
                                Console.WriteLine("You can choose to reroll all the dice or just the 3 remaining dice that aren't the same");
                                Console.WriteLine("Would you like to roll the three remaining dice? y/n");
                                choice = Console.ReadLine();
                            } else
                            {
                                choice = "y";
                            }
                            Reroll = true;

                            if (choice == "y")
                            {
                                RerollRest(Dice1, Dice2, Dice3, Dice4, Dice5);
                            }
                            else
                            {
                                RollList[0] = Dice1.RollDie();
                                RollList[1] = Dice2.RollDie();
                                RollList[2] = Dice3.RollDie();
                                RollList[3] = Dice4.RollDie();
                                RollList[4] = Dice5.RollDie();
                            }
                            occurance = 0;
                            break;
                        } else if (turn % 2 == 1 && VsComputer == true)
                        {
                            if (test == false)
                            {
                                Console.WriteLine("Computer chooses to roll the dice");
                            }
                            Reroll = true;
                            RerollRest(Dice1, Dice2, Dice3, Dice4, Dice5);
                            occurance = 0;
                            break;
                        }

                    }
                    occurance = 0;
                }
                if (Reroll == true)
                {
                    tally = 0;
                }
                else if (turn % 2 == 0)
                {
                    PlayerPoints += tally;
                    RoundTotal = tally;
                    if (test == false)
                    {
                        Console.WriteLine("Player score goes up by " + tally + " total = " + PlayerPoints);
                    }
                    tally = 0;
                    turn ++;
                } else 
                { 
                    EnemyPoints += tally; 
                    turn ++; 
                    if (test == false)
                    {
                        Console.WriteLine("Enemy score goes up by " + tally + " total = " + EnemyPoints);
                    }
                    RoundTotal = tally;
                    tally = 0; 
                }
                if (Reroll == false)
                {
                    if (test == false)
                    {
                        Console.ReadLine();
                    }
                }
                Console.Clear();
            }
            if (test == false)
            {
                if (PlayerPoints > EnemyPoints)
                {
                    Console.WriteLine("Player wins!");
                }
                else
                {
                    Console.WriteLine("Enemy wins!");
                }
                Console.ReadLine();
            }

            string StatsThing = Convert.ToString(PlayerPoints) + " " + Convert.ToString(EnemyPoints) + " " + Convert.ToString(turn) + Environment.NewLine;
            stats.AddToFile(StatsThing);
        }

        private void PrintList()
        {
            foreach (int i in RollList)
            {
                Console.WriteLine("Dice rolled " + i);
            }
        }

        private void RerollRest(Die Dice1, Die Dice2, Die Dice3, Die Dice4, Die Dice5)
        {
            int temp = 0;
            int Occurance = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (RollList[i] == RollList[j])
                    {
                        Occurance++;
                        if (Occurance == 2)
                        {
                            temp = RollList[i];
                            break;
                        }
                    }
                }
                Occurance = 0;
            }
            int location1 = 0;
            int location2 = 0;
            for (int i = 0;i < 5;i++)
            {
                if (RollList[i] == temp) { location1 = i; break; }
            }
            for (int i = 0; i < 5 ; i++)
            {
                if ((RollList[i] == temp) && (i != location1))
                {
                    location2 = i; break;
                }
            }

            for (int i = 0; i<5 ; i++)
            {
                if (i != location1 && i != location2)
                {
                    RollList[i] = SpecificDie(i, Dice1, Dice2, Dice3, Dice4, Dice5);
                }
            }
        }

        private int SpecificDie(int i, Die Dice1, Die Dice2, Die Dice3, Die Dice4, Die Dice5)
        {
            if (i == 0)
            {
                return Dice1.RollDie();
            } else if (i == 1)
            {
                return Dice2.RollDie();
            } else if (i == 2)
            {
                return Dice3.RollDie();
            } else if (i == 3)
            {
                return Dice4.RollDie();
            }
            return Dice5.RollDie();
        }


        protected override bool GameWin()
        {
            if (PlayerPoints >= 20)
            {
                return true;
            } else if (EnemyPoints >= 20)
            {
                return true;
            }
            return false;
        }

    }

    public abstract class AGame
    {
        protected int NumberOfDie;
        protected bool VsComputer;
        protected int PlayerPoints;
        protected int EnemyPoints;
        protected Die Dice1 = new Die();
        protected Die Dice2 = new Die();

        protected virtual bool GameWin()
        {
            if (EnemyPoints < PlayerPoints)
            {
                return true;
            } else { return false; }
        }

    }

    public class Game
    {
        public Game(bool test = false) 
        {

            if (test == true)
            {
                tests();
            } else
            {
                string Choice3 = "";
                while (Choice3 != "n")
                {
                    Console.WriteLine("Would you like to play (Closes if you enter n)");
                    Choice3 = Console.ReadLine();

                    if (Choice3 == "n")
                    {
                        break;
                    }


                    bool VsComputer = false;
                    Console.WriteLine("Select which game you would like to play");
                    Console.WriteLine();
                    Console.WriteLine("1. Sevens Out");
                    Console.WriteLine("2. Three or More");
                    Console.WriteLine();
                    string Choice = Console.ReadLine();
                    Console.WriteLine("Type p if you want to play against a local player");
                    string Choice2 = Console.ReadLine();
                    if (Choice2 == "p")
                    {
                        VsComputer = false;
                    }
                    else
                    {
                        VsComputer = true;
                    }
                    if (Choice == "1")
                    {
                        SevensOut game = new SevensOut(VsComputer);
                    }
                    else
                    {
                        ThreeOrMore game = new ThreeOrMore(VsComputer);
                    }
                }

                Console.WriteLine("Would you like to view the stats before you go? (Pressing y will print the stats)");
                Choice3 = Console.ReadLine();
                if (Choice3 == "y")
                {
                    while (Choice3 != "1" && Choice3 != "2" && Choice3 != "3")
                    {
                        Console.WriteLine("Which would you like to view");
                        Console.WriteLine("1. Sevens out stat page");
                        Console.WriteLine("2. Three or more stat page");
                        Console.WriteLine("3. Both");
                        Choice3 = Console.ReadLine();
                    }
                    if (Choice3 == "3")
                    {
                        Statistics stats = new Statistics();
                        stats.RunStats();
                        Console.WriteLine();
                        stats.ChangePath();
                        stats.SetStats(0);
                        stats.RunStats();
                        Console.ReadLine();

                    }
                    else
                    {
                        Statistics stats = new Statistics(Convert.ToInt32(Choice3));
                        stats.RunStats();
                        Console.ReadLine();
                    }

                }
            }

        }

        public void tests()
        {
            SevensOut Game7 = new SevensOut(false, true);
            string LogString = "";
            string TempLogString = "";

            if (Game7.GSRollValue == 7)
            {
                TempLogString = "Test1 (Check if game ended when 7 was rolled): Working" + Environment.NewLine;
                LogString += TempLogString;
            } else
            {
                LogString += "Test1 (Check if game ended when 7 was rolled): Failed" + Environment.NewLine;
            }

            Debug.Assert(Game7.GSRollValue == 7, "Total did not stop when 7 was rolled");
            ThreeOrMore Game3 = new ThreeOrMore(false, true);

            bool a = (Game3.GetRoundTotal() == Game3.PointsCalc());
            bool b = true;

            if (a == true)
            {
                TempLogString = "Test2 (Check if points were added correctly): Working" + Environment.NewLine;
                LogString += TempLogString;
            }
            else
            {
                LogString += "Test2 (Check if points were added correctly): Failed" + Environment.NewLine;
            }

            Debug.Assert(a, "Points added incorrectly");

            

            a = Game3.GetPlayerPoints() < 20;
            b = Game3.GetEnemyPoints() < 20;

            if ((a && !b) || (!a && b))
            {
                TempLogString = "Test3 (Check if winning score is over 20): Working" + Environment.NewLine;
                LogString += TempLogString;
            }
            else
            {
                LogString += "Test3 (Check if winning score is over 20): Failed" + Environment.NewLine;
            }

            LogString += Environment.NewLine;

            Debug.Assert((a && !b) || (!a && b), "Game stopped before points became greater than 20");

            string path = "Log.txt";
            File.AppendAllText(path, LogString);

            

        }

    }

    interface IDice 
    {
        int RollDie();
        int GetRoll();
    }

    public class Die : IDice
    {
        private int Value;
        static Random R = new Random();

        public int RollDie()
        {
            Value = R.Next(1,7); return Value;
        }

        public int GetRoll()
        {
            return Value;
        }
    }
}
