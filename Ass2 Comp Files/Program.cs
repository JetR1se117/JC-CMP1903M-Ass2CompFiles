using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Schema;

namespace Ass2_Comp_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Command.terminal();
        }
    }

    class DataHandler
    {
        protected static int differences = 0;
        protected static string[] getData(string Filepath)
        {
            string[] newFile = File.ReadAllLines(Filepath); return newFile;
        }

        protected static string getDataText(string Filepath)
        {
            string newFile = File.ReadAllText(Filepath); return newFile;
        }

        // This line by line initial method allows for a cleaner call with less variables in FileComp
        protected static void LineByLinePrint(string[] FileA, string[] FileB)
        {
            if (FileA.Length > FileB.Length)
                LineByLinePrint(FileA, FileB, FileB.Length);
            else
                LineByLinePrint(FileA, FileB, FileA.Length);
        }
        protected static void LineByLinePrint(string[] FileA, string[] FileB, int shortLines) // the 2nd method goes through each line and displays the diffferences between the two files, as the method name would suggest.
        {
            int i = 0;
            for (i = 0; i < shortLines; i++)
            {
                string[] tempA = FileA[i].Split(); string[] tempB = FileB[i].Split();
                int shortLength = tempA.Length; int longLength = tempB.Length;
                if (tempA.Length > tempB.Length)
                { shortLength = tempB.Length - 1; longLength = tempA.Length; }
                Console.Write($"{i + 1} |");
                string[] added = new string[longLength];
                string[] removed = new string[longLength];
                int adCount = 0; int remCount = 0;
                bool changePresent = false;
                for (int j = 0; j < shortLength; j++)
                {
                    if (tempA[j] == tempB[j])
                        Console.Write(tempA[j] + " ");
                    else
                    {
                        changePresent = true;
                        Console.ForegroundColor = ConsoleColor.Red; Console.Write(tempA[j] + " ");
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write(tempB[j] + " ");
                        added[adCount++] = tempB[j]; removed[remCount++] = tempA[j];
                        Console.ResetColor();
                        differences = differences + 2;

                    }
                }
                if (tempA.Length != tempB.Length)
                    if (tempA.Length > tempB.Length)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        for (int j = shortLength + 1; j < longLength; j++)
                        {
                            Console.Write(tempA[j] + " "); differences++;
                        }
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        for (int j = shortLength; j < longLength; j++)
                        {
                            Console.Write(tempB[j] + " "); differences++;
                        }
                        Console.ResetColor();
                    }
                if (changePresent)
                {
                    File.AppendAllText("GitDiffLog.log", $"\nLine {i + 1}: [ADDED] : ");
                    foreach (string word in added)
                    {
                        File.AppendAllText("GitDiffLog.log", $"{word}, ");
                    }
                    File.AppendAllText("GitDiffLog.log", $"\nLine {i + 1}: [REMOVED] : ");
                    foreach (string word in removed)
                    {
                        File.AppendAllText("GitDiffLog.log", $"{word}, ");
                    }
                }
                else
                {
                    File.AppendAllText("GitDiffLog.log", $"\nLine {i + 1}: [NO CHANGES] ");
                }
                Console.WriteLine();
            }
            if (differences == 0)
            {
                File.AppendAllText("GitDiffLog.log", $"\nDifferences: None\n");
            }
            else
            {
                File.AppendAllText("GitDiffLog.log", $"\nDifferences: {differences}\n");
            }
            if (FileB.Length != FileA.Length)
            {
                bool ALong = true;
                if (FileB.Length > FileA.Length)
                    ALong = false;
                if (ALong)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (i = i; i < FileA.Length; i++)
                    {
                        Console.WriteLine(FileA[i]);
                        string[] temp = FileA[i].Split();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    for (i = i; i < FileB.Length; i++)
                    {
                        Console.WriteLine(FileB[i]);
                        string[] temp = FileB[i].Split();
                    }
                }
                Console.ResetColor();
            }
        }
    }
    class Command
    {
        static bool terminalIni = false;
        public static void terminal()
        {
            string ans = "";
            while (ans != "quit")
            {
                if (!terminalIni)
                { Console.WriteLine("Type <help> for a list of commands and their usage"); terminalIni = true; }
                Console.Write(">: ");
                ans = choice(Console.ReadLine());
            }
        }
        static string choice(string ans)
        {
            string[] ansarr = ans.Split();
            switch (ansarr[0])
            {
                case "quit":
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "diff":
                    try
                    {
                        FileCompare.CompMain(ansarr);
                    }
                    catch
                    {
                        Console.WriteLine("There was something wrong with your input. Try retyping, or choose a different file.");
                    }
                    break;
                case "log":
                    try
                    {
                        switch (ansarr[1])
                        {
                            case "read":
                                logRead();
                                break;
                            case "clear":
                                Console.WriteLine("Are you sure? ( y/n )"); Console.Write(">: ");
                                string temp = Console.ReadLine();
                                if (temp == "y")
                                    File.WriteAllText("GitDiffLog.log", "");
                                break;
                            default:
                                Console.WriteLine($"{ansarr[1]} subcommand doesnt work with log");
                                break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"There was an error. Try retyping the subcommand, or type 'help log' for a list of subcommands for log");
                    }
                    break;
                case "help":
                    if (ansarr.Length == 1)
                        helpText();
                    else
                    {
                        if (ansarr[1] == "log")
                            Console.WriteLine("log read         Displays the log file \nlog clear        Deletes all previous logs from the file");
                    }
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine($"{ansarr[0]} is an invalid command. Type <help> for a list of commands and their usage");
                    break;
            }
            return ans;
        }
        static void logRead() 
        {
            string[] logFile = File.ReadAllLines("GitDiffLog.log");
            if (logFile.Length > 0)
                foreach (string line in logFile)
                    Console.WriteLine(line);
            else
                Console.WriteLine("Log is empty.");
                
        }
        static void helpText()
        {
            Console.WriteLine("diff <textfilea.txt> <textfileb.txt>     Compares The two files");
            Console.WriteLine("quit                                     ends the program");
            Console.WriteLine("help                                     displays these commands");
            Console.WriteLine("clear                                    clears the console window history");
            Console.WriteLine("log                                      handles the log. Type 'help log' for a list of subcommands");
        }
    }
    class FileCompare : DataHandler
    {
        static string filenamea;
        static string filenameb;

        static string[] FileA;
        static string[] FileB;
        static string[] OutputArr;

        static string TextA;
        static string TextB;

        public static void CompMain(string[] ans)
        {
            Console.WriteLine($"{ans[0]} {ans[1]} {ans[2]}");
            FileSelection(ans);
            FileComp();
        }
        static void FileSelection(string[] ans)
        {
            filenamea = ans[1];
            filenameb = ans[2];
            FileA = getData(filenamea); TextA = getDataText(filenamea);
            FileB = getData(filenameb); TextB = getDataText(filenameb);
            if (FileA.Length > FileB.Length)
                OutputArr = new string[FileA.Length];
            else
                OutputArr = new string[FileB.Length];
        }
        static void FileComp()
        {
            bool same;
            //int Len = getShortestLength(FileA, FileB);
            //CompareStringLines(FileA, FileB, Len);
            if (TextA == TextB)
                same = true;
            else
                same = false;
            File.AppendAllText("GitDiffLog.log", $"\n\n New Log :: File 1: {filenamea}; File 2: {filenameb}; \n");
            if (same)
            {        // red and green emulate the git command, and provide easier distinction between the additions and removals.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{filenamea} and {filenameb} are not different");
                File.AppendAllText("GitDiffLog.log", $"\nDifferences: None\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{filenamea} and {filenameb} are different");
                Console.ResetColor();
                LineByLinePrint(FileA, FileB);
                Console.ResetColor();
                Console.WriteLine($"LinesLength: {OutputArr.Length}");
            }
            Console.WriteLine(" -- Data Logged --");    // feedback to the user that the details are now in the log
            differences = 0;
            Console.WriteLine();
        }


    }
}
