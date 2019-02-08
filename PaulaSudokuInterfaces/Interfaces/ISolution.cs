using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudokuInterfaces.Interfaces
{
    public interface ISolution
    {
        //Create lists to hold the file names and solution grids
        List<List<HashSet<int>>> SolvePuzzle(string fileName);
        List<List<HashSet<int>>> SolvePuzzle(List<List<HashSet<int>>> grid);
    }
}
