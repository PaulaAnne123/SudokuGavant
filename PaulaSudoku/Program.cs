using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudoku
{
    class Program
    {
        static void Main(string[] args)
        {
                DirectoryInfo dir = new DirectoryInfo("../../Puzzles/");
                FileInfo[] info = dir.GetFiles("*puzzle*");

                foreach (FileInfo f in info)
                {
                    string strInputFile = Path.GetFileNameWithoutExtension(f.ToString());
                    string strOutputFile = f.Directory + "\\" + strInputFile + ".sln.txt";
                    Solve(strInputFile, strOutputFile);
                }
            
        }

        private static void Solve(string strInputSolve, string strOutputSolve)
        {
            
            if (!File.Exists(strOutputSolve))
            {
                File.Create(strOutputSolve);
            }
            else
            {
                Console.WriteLine("File {0} already exists", strOutputSolve);
            }
            
        }
    }
}
