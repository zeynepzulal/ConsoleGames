using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames.Games
{
    internal class HanoiTower : Game
    {
        // PUBLIC PROPERTIES (Eigenschaften)
        public override string Name => "Hanoi Tower";
        public override string Description => "The objective of the game is to shift the entire stack of disks from one rod to another rod ";
        public override string Rules => " 1) Only one disk can be moved at a time.\n " +
            "2) Only the uppermost disk from one stack can be moved on to the top of another stack or an empty rod.\n " +
            "3) Larger disks cannot be placed on the top of smaller disks";
        public override string Credits => "Zeynep Keskin, zekeskin@ksr.ch";
        public override int Year => 2024;
        public override bool TheHigherTheBetter => false;
        public override int LevelMax => 4;
        public override Score HighScore { get; set; }

        //---------------------------------------------------//
        // PRIVATE FIELDS
        //---------------------------------------------------//


        //---------------------------------------------------//
        // PUBLIC METHODS
        //---------------------------------------------------//
        private int[] diskNumberOfLevels = new int[] { 3, 4, 5, 6 };



        public class Disk
        {

            public int size;
            public string color;
            public Disk(int _size, string _color)
            {
                size = _size;
                color = _color;

            }
        }
        //Variables
        int numberOfDisks;
        List<Disk>[] rods = new[]{
               new List<Disk>{},
               new List<Disk>{},
               new List<Disk>{}
        };


        int maxWidthOfTheRod = 21;
        int heightOfTheRod = 10;

        int[] validRodNum = new int[3] { 1, 2, 3 };

        /// <summary>
        /// Public method <c>Play</c> that is called by framework (in Program.cs) to start the game.
        /// </summary>
        /// <remarks>
        /// If player is not successful (fail or quit), set score.points=-1.
        /// If sucessfull, score.points is positive int representing score achieved
        /// </remarks>
        /// <param name="level">the level of the game</param>
        /// <returns>
        /// Score object that contains info about score achieved.
        /// </returns>
        public override Score Play(int level = 1)
        {

            Score score = new Score();
            score.LevelCompleted = false;

            bool levelCompleted = false; // number guessed in at most max attempts



            DisplayInitial(ref level, ref numberOfDisks, rods);
            while (true)
            {
                int fromWhichRod = 0;
                int toWhichRod = 0;

                ViewDisk(rods, maxWidthOfTheRod, ref numberOfDisks, heightOfTheRod);
                MoveTo(rods, ref validRodNum, ref fromWhichRod, ref toWhichRod);

                if (HaveYouSucceeded(rods, ref numberOfDisks, ref level, ref levelCompleted, maxWidthOfTheRod, LevelMax, heightOfTheRod))
                {
                    score.LevelCompleted = levelCompleted;
                    score.Level = level;
                    break;
                }
                
            }

            return score;


        }


        //---------------------------------------------------//
        // PRIVATE METHODS FOR MODEL
        //---------------------------------------------------//

        /// <summary>
        /// Private method <c>EvaluateGuess</c> evaluates guess and checks if level completed.
        /// </summary>
        /// <remarks>
        /// Guessed number is evaluated: too small/correct/too big?
        /// When level completed (correct guess) check if level
        /// completed successfully (<= max attempts)
        /// </remarks>
        /// <param name="level">the level of the game</param>
        /// <param name="nrToGuess">number player is supposed to guess</param>
        /// <param name="guess">number player guessed</param>
        /// <param name="attempts">number of attempts so far</param>
        /// <param name="levelCompleted">bool level completed successfully, passed as ref</param>
        /// <returns>
        /// int with value -1 (guess too small), 0 (correct guess) or 1 (guess too big)
        /// </returns>


        static void ViewDisk(List<Disk>[] rods, int maxWidthOfRod, ref int numberOfDisk, int heightOfRod)
        {
            for (int j = heightOfRod - 1; j >= 0; j--) // 10 == cubugun uzunlugu, eksi bir koymazsak 11 oluyor o yüzden -1 koydum
            {
                for (int i = 0; i < rods.Length; i++) //rods.length =3 cubuk var
                {
                    var rod = rods[i];

                    if (j >= rod.Count) // cubugun uzunlugu yani 10 > 3, 9>3 8> ----- yani 10 kere bu saarti saglarsa for döngüsünde belirlendigi gibi 10 kere | koycak
                    {// cubugun 10>3  9>3  8>3  7>3  6>3   5>3  4>3  3>3 buraya kadar sadece cubuk komur. 2<3 olduguna asagidaki else olusur
                        Console.Write(new String(' ', maxWidthOfRod / 2));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                        Console.Write(new String(' ', maxWidthOfRod / 2));

                    }
                    else // 2<3  1<3 0<3   diger üc |  cizgi ve 3 disk bu else ile konur
                    {
                        var disk = rod[j];  // rods[0][10] =  disk(2, sari), 0,9.. 0,8 .....1,10 ..1,8.... 1,2 .... 2,10 ....2,2 ...2,1 2,0
                        int emptySpaces = (maxWidthOfRod - disk.size) / 2;
                        Console.Write(new String(' ', emptySpaces));
                        var color = ConsoleColor.White;
                        if (disk.color == "red")
                        {
                            color = ConsoleColor.Red;
                        }
                        if (disk.color == "blue")
                        {
                            color = ConsoleColor.Blue;
                        }
                        if (disk.color == "green")
                        {
                            color = ConsoleColor.Green;
                        }
                        if (disk.color == "yellow")
                        {
                            color = ConsoleColor.Yellow;
                        }
                        if (disk.color == "cyan")
                        {
                            color = ConsoleColor.Cyan;
                        }
                        if (disk.color == "magenta")
                        {
                            color = ConsoleColor.Magenta;
                        }
                        Console.ForegroundColor = color;
                        Console.Write(new String('■', disk.size / 2));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                        Console.ForegroundColor = color;
                        Console.Write(new String('■', disk.size / 2));

                        Console.Write(new String(' ', emptySpaces));
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                }

                Console.WriteLine();
                

            }

        }
        static void MoveTo(List<Disk>[] rods, ref int[] validRodNum, ref int fromWhichRod, ref int toWhichRod)

        {

            while (true)
            {

                Console.WriteLine("From which rod do you want to move the disc ? (1, 2 or 3)");

                fromWhichRod = int.Parse(Console.ReadLine());

                if (!validRodNum.Contains(fromWhichRod))
                {
                    Console.WriteLine("This is not a valid rod number. Please enter a valid rod number!");
                    continue;
                }
                if (rods[fromWhichRod - 1].Count == 0)
                {
                    Console.WriteLine("There is not any disk to move");
                    continue;
                }
                if (FindMax(rods, rods[fromWhichRod - 1].Last().size) && rods[1].Count != 0 && rods[2].Count != 0 && rods[0].Count != 0)
                {
                    Console.WriteLine("This is the biggest disk, you cant move this rigt now to anywhere! ");
                    continue;
                }
                if (fromWhichRod == 1 && rods[0].Count != 0 && rods[1].Count != 0 && rods[2].Count != 0)
                {
                    if (rods[fromWhichRod - 1].Last().size > rods[1].Last().size && rods[fromWhichRod - 1].Last().size > rods[2].Last().size)
                    {
                        Console.WriteLine("You canr move this disk to anywhere rigth now!");
                        continue;
                    }
                    else { break; }

                }
                if (fromWhichRod == 2 && rods[0].Count != 0 && rods[1].Count != 0 && rods[2].Count != 0)
                {
                    if (rods[fromWhichRod - 1].Last().size > rods[0].Last().size && rods[fromWhichRod - 1].Last().size > rods[2].Last().size)
                    {
                        Console.WriteLine("You canr move this disk to anywhere rigth now!");
                        continue;
                    }
                    else { break; }
                }
                if (fromWhichRod == 3 && rods[0].Count != 0 && rods[1].Count != 0 && rods[2].Count != 0)
                {
                    if (rods[fromWhichRod - 1].Last().size > rods[0].Last().size && rods[fromWhichRod - 1].Last().size > rods[1].Last().size)
                    {
                        Console.WriteLine("You canr move this disk to anywhere rigth now!");
                        continue;
                    }
                    else { break; }
                }
                /* if (rods[toWhichRod - 1].Count != 0)
                 {
                     if (rods[fromWhichRod - 1].Last().size > rods[0].Last().size &&
                         rods[fromWhichRod - 1].Last().size > rods[1].Last().size && rods[fromWhichRod - 1].Last().size > rods[2].Last().size)
                     {
                         Console.WriteLine("You cant move this disk rigth now to anywhere!");
                         continue;
                     }
                     else
                     {
                         break;
                     }
                 }*/
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("To Which rod do you want to move the disc ? (1, 2 or 3)");
                toWhichRod = int.Parse(Console.ReadLine());
                if (!validRodNum.Contains(toWhichRod))
                {
                    Console.WriteLine("This is not a valid rod number. Please enter a valid rod number!");
                    continue;
                }
                if (rods[toWhichRod - 1].Count != 0)
                {
                    if (rods[toWhichRod - 1].Last().size < rods[fromWhichRod - 1].Last().size)
                    {
                        Console.WriteLine("You cant put a larger disk on a smaller disk! Choose a smaller disk to put on it.");
                        continue;
                    }

                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;

                }


            }

            var theMovingDisk = rods[fromWhichRod - 1].LastOrDefault(); //tipi disk
            rods[fromWhichRod - 1].RemoveAt(rods[fromWhichRod - 1].Count - 1); // tipi int olmali
            rods[toWhichRod - 1].Add(theMovingDisk);
            Console.Clear();

        }
        static bool FindMax(List<Disk>[] rods, int lastDisk)
        {
            int maxSize = 0;
            List<int> sizes = new List<int> { };
            for (int i = 0; i < rods.Length; i++)
            {
                for (int j = 0; j < rods[i].Count; j++)
                {
                    sizes.Add(rods[i][j].size);
                }
            }
            for (int i = 0; i < sizes.Count; i++)
            {
                if (sizes[i] > maxSize)
                {
                    maxSize = sizes[i];
                }
            }
            //Console.WriteLine(maxSize);
            if (maxSize == lastDisk)
            {
                return true;
            }
            return false;
        }
        private void DisplayInitial(ref int level, ref int numberOfDisk, List<Disk>[] rods)
        {

            if (level == 1)
            {
                numberOfDisk = diskNumberOfLevels[0];

            }
            if (level == 2)
            {
                numberOfDisk = diskNumberOfLevels[1];

            }
            if (level == 3)
            {
                numberOfDisk = diskNumberOfLevels[2];

            }
            if (level == 4)
            {
                numberOfDisk = diskNumberOfLevels[3];
            }
            AddDisk(rods, numberOfDisks);

        }
        static void AddDisk(List<Disk>[] rods, int numberOfDisks)
        {
            foreach (var rod in rods)
            {
                rod.Clear();
            }

            for (int i = 0; i < numberOfDisks; i++)
            {

                string[] colors = { "blue", "cyan", "magenta", "red", "green", "yellow" };


                List<string> randomColors = new List<string>();
                for (int j = 0; j < numberOfDisks; j++)
                {
                    string randomColor = colors[j];
                    if (!randomColors.Contains(randomColor))
                    {
                        randomColors.Add(randomColor);
                    }

                }

                rods[0].Insert(0, new Disk((i + 1) * 2, colors[i])); // Her disk boyutunu artırarak ekliyoruz
            }
        }
        static bool HaveYouSucceeded(List<Disk>[] rods, ref int numberOfDisk, ref int level, ref bool levelCompleted, int maxWidthOfRod, int LevelMax, int heightOfTheRod)
        {
            if (rods[1].Count == numberOfDisk || rods[2].Count == numberOfDisk)
            {
                ViewDisk(rods, maxWidthOfRod, ref numberOfDisk, heightOfTheRod);

                levelCompleted = true;


                if (level < LevelMax)
                {
                    if (levelCompleted)
                    {
                        Console.WriteLine("Gratuliere, du hast das Level erfolgreich absolviert.");

                    }
                    else
                    {
                        Console.WriteLine("Leider.");
                    }

                    Console.WriteLine("Drücke irgend eine Taste um fortzufahren.");
                    Console.ReadKey();
                }

                return true;
            }
            return false;
        }





    }


}