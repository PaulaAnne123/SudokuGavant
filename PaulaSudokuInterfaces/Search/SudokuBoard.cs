﻿using PaulaSudokuInterfaces.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudokuInterfaces.Search
{
    public class SudokuBoard : ISearch
    {
        public bool gridSearch(List<List<HashSet<int>>> grid)
        {
            var foundValue = false;

            //Final checks to take care of any values that have not been matched
            for (var x = 0; x < grid.Count; x++)
            {
                for (var y = 0; y < grid[x].Count; y++)
                {
                    if (grid[x][y].Count > 1)
                    {
                        for (var value = 1; value <= grid.Count; value++)
                        {
                            //Check Row
                            var rowContainsResult = rowContains(grid[x], value);

                            //Check Column
                            var colummContainsResult = columnContains(grid, y, value);

                            //Check Nondrant
                            var nondrantResult = nondrantContains(grid, x, y, value);

                            if (rowContainsResult || colummContainsResult || nondrantResult)
                            {
                                //if value exists, it had already been used, so eliminate it
                                grid[x][y].Remove(value);

                                if (grid[x][y].Count == 1)
                                {
                                    foundValue = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return foundValue;
        }

        public bool singlesSearch(List<List<HashSet<int>>> grid)
        {
            foreach(var row in grid)
            {
                findRowHiddenSigles(row);
            }

            for(var i = 0; i<grid.Count; i++)
            {
                var pivotRow = pivotColumn(grid, i);
                findRowHiddenSigles(pivotRow);
            }

            return true;
        }

        public bool findRowHiddenSigles(List<HashSet<int>> row)
        {
            var rowNeeds = GetRowNeeds(row);

            foreach (var need in rowNeeds)
            {
                var placesWeCouldPutNeed = new List<HashSet<int>>();

                foreach(var col in row)
                {
                    if (col.Count != 1 && col.Contains(need))
                    {
                        placesWeCouldPutNeed.Add(col);
                    }
                }

                if(placesWeCouldPutNeed.Count == 1)
                {
                    placesWeCouldPutNeed.Single().Clear();
                    placesWeCouldPutNeed.Single().Add(need);
                }
            }

            return true;
        }

        public bool nonetSearch(List<List<HashSet<int>>> grid)
        {
            var foundValue = false;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var nondrantNeeds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    //Get needed values depending on proximity to cell in question
                    for (var x = i * 3; x < i * 3 + 3; x++)
                    {
                        for (var y = j * 3; y < j * 3 + 3; y++)
                        {
                            if (grid[x][y].Count == 1)
                            {
                                nondrantNeeds.Remove(grid[x][y].Single());
                            }
                        }
                    }

                    //See where we could put needed values
                    foreach (var need in nondrantNeeds)
                    {
                        var potentialValueLocations = new List<Tuple<int, int>>();

                        for (var x = i * 3; x < i * 3 + 3; x++)
                        {
                            for (var y = j * 3; y < j * 3 + 3; y++)
                            {
                                if (grid[x][y].Count != 1 && !rowContains(grid[x], need) && !columnContains(grid, y, need) && !nondrantContains(grid, x, y, need) && grid[x][y].Contains(need))
                                {
                                    potentialValueLocations.Add(new Tuple<int, int>(x, y));
                                }
                            }
                        }

                        if (potentialValueLocations.Count == 1)
                        {
                            grid[potentialValueLocations.Single().Item1][potentialValueLocations.Single().Item2].Clear();
                            grid[potentialValueLocations.Single().Item1][potentialValueLocations.Single().Item2].Add(need);

                            foundValue = true;
                        }
                        else if (potentialValueLocations.Count == 2 || potentialValueLocations.Count == 3)
                        {
                            var firstX = potentialValueLocations.First().Item1;
                            var firstY = potentialValueLocations.First().Item2;

                            if (potentialValueLocations.All(p => p.Item1 == firstX))
                            {
                                removePointingPairRow(grid[firstX], j, need);
                            }
                            else if (potentialValueLocations.All(p => p.Item2 == firstY))
                            {
                                var pivotRow = pivotColumn(grid, firstY);
                                removePointingPairRow(pivotRow, i, need);
                            }
                        }
                    }
                }
            }

            return foundValue;
        }

        public void removePointingPairRow(List<HashSet<int>> row, int indexOfBox, int need)
        {
            for(var i = 0; i<row.Count; i++)
            {
                if(i/3 != indexOfBox)
                {
                    row[i].Remove(need);
                }
            }
        }

        private bool nondrantContains(List<List<HashSet<int>>> grid, int x, int y, int value)
        {
            var gridFlat = gridFlatten(grid, x, y);
            return rowContains(gridFlat, value);
        }

        private bool columnContains(List<List<HashSet<int>>> grid, int y, int value)
        {
            var pivotedColumn = pivotColumn(grid, y);
            return rowContains(pivotedColumn, value);
        }

        public bool searchTuples(List<List<HashSet<int>>> grid)
        {
            foreach (var row in grid)
            {
                removeRowNakedPairs(row, 2);
                removeRowNakedPairs(row, 3);
            }

            for (var columnIndex = 0; columnIndex < grid.First().Count; columnIndex++)
            {
                var pivotedColumn = pivotColumn(grid, columnIndex);
                removeRowNakedPairs(pivotedColumn, 2);
                removeRowNakedPairs(pivotedColumn, 3);
            }

            for (var x = 0; x < 3; x++)
            {
                for(var y = 0; y < 3; y++)
                {
                    var gridFlat = gridFlatten(grid, x, y);
                    removeRowNakedPairs(gridFlat, 2);
                    removeRowNakedPairs(gridFlat, 3);
                }
            }

            return true;
        }

        public List<HashSet<int>> gridFlatten(List<List<HashSet<int>>> grid, int x, int y)
        {
   
            var gridFlat = new List<HashSet<int>>();

            var searchRowIndex = (x/3) * 3;
            var searchColumnIndex = (y/3) * 3;

            for (var i = searchRowIndex; i < searchRowIndex + 3; i++)
            {
                for (var j = searchColumnIndex; j < searchColumnIndex + 3; j++)
                {
                    gridFlat.Add(grid[i][j]);
                }
            }

            return gridFlat;
        }

        private List<HashSet<int>> pivotColumn(List<List<HashSet<int>>> grid, int columnIndex)
        {
            //search column in question if row does not contain the value being considered
            var pivotedColumn = new List<HashSet<int>>();

            foreach (var row in grid)
            {
                pivotedColumn.Add(row[columnIndex]);
            }

            return pivotedColumn;
        }

        public void removeRowNakedPairs(List<HashSet<int>> row, int n)
        {
            foreach (var checkCol in row)
            {
                if (checkCol.Count == n && howManySubsets(row, checkCol) == n)
                {
                    foreach (var removeCol in row.Where(c => !c.IsSubsetOf(checkCol)))
                    {
                        foreach (var opt in checkCol)
                        {
                            removeCol.Remove(opt);
                        }
                    }
                }
            }
        }

        private int howManySubsets(List<HashSet<int>> row, HashSet<int> options)
        {
            var result = 0;

            foreach (var col in row)
            {
                if (col.IsSubsetOf(options))
                {
                    result++;
                }
            }

            return result;
        }

        public bool rowContains(List<HashSet<int>> row, int value)
        {
            //review the existing row occupants and see where the new candidate fits
            foreach (var box in row)
            {
                if (box.Count == 1 && box.Single() == value)
                    return true;
            }

            return false;
        }

        public List<int> GetRowNeeds(List<HashSet<int>> row)
        {
            var rowNeeds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            foreach (var col in row)
            {
                if (col.Count == 1)
                {
                    rowNeeds.Remove(col.Single());
                }
            }

            return rowNeeds;
        }
    }
}
