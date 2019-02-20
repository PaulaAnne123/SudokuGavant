using PaulaSudokuInterfaces.Interfaces;
using PaulaSudokuInterfaces.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudokuInterfaces
{
    public class SudokuSolver : ISolution
    {
        private ISearch searchGrid;

        public SudokuSolver(ISearch searchGrid)
        {
            this.searchGrid = searchGrid;
        }

        //Read the sudoku puzzle into an original grid for data manipulation
        //Returns a list of solved puzzles
        public List<List<HashSet<int>>> SolvePuzzle(string fileName)
        {
            var grid = readFileIntoGrid(fileName);

            //Send initialized grid to be solved
            return SolvePuzzle(grid);
        }

        public List<List<HashSet<int>>> SolvePuzzle(List<List<HashSet<int>>> grid)
        {
            var numOfOptions = numberOfOptions(grid);

            //Puzzle is not complete until total number of options exhausted
            var Done = false;

            while (!Done)
            {
                //running total of number of options on sudoku board
                numOfOptions = numberOfOptions(grid);

                //use these techniques to slim down the choices available until all have been used utilizing sudoku techniquess
                searchGrid.gridSearch(grid);
                searchGrid.SinglesSearch(grid);
                searchGrid.BlockSearch(grid);
                searchGrid.SearchTuples(grid);

                Done = isSolved(grid) || numOfOptions == numberOfOptions(grid);
            }

            return grid;
        }

        public List<List<HashSet<int>>> readFileIntoGrid(string fileName)
        {
            //Create input Grid as list to store data efficiently and read file into it
            //Depending on if the character is X or fixed. If fixed, there is one solution
            //for that square, if it is X, the solutions begin at 1-9 for options to be
            //trimmed down
            var grid = new List<List<HashSet<int>>>();
            var lines = File.ReadAllLines(fileName);

            foreach (var line in lines)
            {
                var row = new List<HashSet<int>>();

                foreach (var box in line)
                {
                    if (box == 'X')
                    {
                        row.Add(new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                    }
                    else
                    {
                        row.Add(new HashSet<int> { int.Parse(box.ToString()) });
                    }
                }

                grid.Add(row);
            }
            //Grid is initialized with givens accounted for
            return grid;
        }

        public bool isSolved(List<List<HashSet<int>>> grid)
        {
            foreach (var row in grid)
            {
                foreach (var col in row)
                {
                    if (col.Count != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int numberOfOptions(List<List<HashSet<int>>> grid)
        {
            var result = 0;

            //scope of possibilities in grid
            foreach (var row in grid)
            {
                foreach (var col in row)
                {
                    result += col.Count;
                }
            }

            return result;
        }
    }
}
