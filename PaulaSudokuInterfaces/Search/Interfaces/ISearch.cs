using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudokuInterfaces.Search.Interfaces
{
    public interface ISearch
    {
        bool gridSearch(List<List<HashSet<int>>> grid);
        bool SinglesSearch(List<List<HashSet<int>>> grid);
        bool NonetSearch(List<List<HashSet<int>>> grid);
        bool SearchTuples(List<List<HashSet<int>>> grid);

        void RemoveRowNakedPairs(List<HashSet<int>> row, int n);
        bool RowContains(List<HashSet<int>> row, int value);
        List<HashSet<int>> gridFlatten(List<List<HashSet<int>>> grid, int x, int y);
        void RemovePointingPairRow(List<HashSet<int>> row, int indexOfBox, int need);
    }
}
