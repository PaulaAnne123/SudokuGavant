using Microsoft.Practices.Unity;
using PaulaSudokuInterfaces;
using PaulaSudokuInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PaulaSudoku;

namespace SudokuSolverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get any puzzles with the right naming convention
            DirectoryInfo dir = new DirectoryInfo("../../Puzzles/");
            FileInfo[] info = dir.GetFiles("*puzzle*");

            var regex = @"^\bpuzzle\d\b.txt$";

            foreach (FileInfo f in info)
            {
                var match = System.Text.RegularExpressions.Regex.Match(f.ToString(), regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    Console.Write("File not in correct input name format");
                }
                else
                {
                    //If puzzle file is named correctly, generate the output name based on the input puzzle filename
                    string strInputFile = Path.GetFileNameWithoutExtension(f.ToString());
                    string strOutputFile = f.Directory + "\\" + strInputFile + ".sln.txt";

                    //initialize containers and interfaces
                    var container = InterfaceInit.start();
                    ISolution solver = (ISolution)container.Resolve(typeof(ISolution));

                    var fileName = f.FullName;
                    var filesToSolve = new List<string>();

                    //Go through files that need to be solved in the directory
                    if (args.Length > 0)
                    {

                        for (var i = 0; i < args.Length; i++)
                        {
                            filesToSolve.Add(args[i]);
                        }
                    }
                    else
                    {
                        filesToSolve.Add(fileName);
                    }

                    //Solve each puzzle in the list and output the solution to a text file
                    foreach (var fileToSolve in filesToSolve)
                    {
                        var grid = solver.SolvePuzzle(fileToSolve);
                        StringBuilder sb = new StringBuilder();

                        //Build a string of solution files and output to each output file
                        foreach (var row in grid)
                        {

                            foreach (var col in row)
                            {
                                if (col.Count == 1)
                                {
                                    Console.Write(col.Single());
                                    if (!String.IsNullOrEmpty(col.Single().ToString()))
                                    { 
                                        sb.Append(col.Single());
                                    }
                                }
                                else
                                {
                                    sb.Append('X');
                                }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        //Finish writing to each file and save before going to the next puzzle
                        using (StreamWriter swriter = new StreamWriter(strOutputFile))
                        {
                            swriter.Write(sb.ToString());
                        }
                    }
                }
            }
        }
    }
}
