using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ass2_Comp_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            FileCompare.CompMain();
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }

    class DataHandler
    {
        protected static string getData(string Filepath)
        {
            string newFile = File.ReadAllText(Filepath); return newFile;
        }
    }

    class FileCompare : DataHandler
    {
        static string FileA;
        static string FileB;
        public static void CompMain()
        {
            FileSelection();

        }
        static void FileSelection()
        {
            DisplayFilePaths();
            string Ans = "";
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"Type the EXACT path for file {i + 1}: ");
                Ans = Console.ReadLine();
                if (i == 0)
                    FileA = getData(Ans);
                else if (i == 1)
                    FileB = getData(Ans);
            }
            FileComp();
        }
        static void DisplayFilePaths()
        {
            Console.WriteLine("GitRepositories_1a.txt");
            Console.WriteLine("GitRepositories_1b.txt");
            Console.WriteLine("GitRepositories_2a.txt");
            Console.WriteLine("GitRepositories_2b.txt");
            Console.WriteLine("GitRepositories_3a.txt");
            Console.WriteLine("GitRepositories_3b.txt");
        }
        static void FileComp()
        {
            if (FileA == FileB)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"a.txt and b.txt are not different");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"a.txt and b.txt are different");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
