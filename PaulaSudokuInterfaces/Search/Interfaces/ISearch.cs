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
        bool singlesSearch(List<List<HashSet<int>>> grid);
        bool nonetSearch(List<List<HashSet<int>>> grid);
        bool searchTuples(List<List<HashSet<int>>> grid);

        void removeRowNakedPairs(List<HashSet<int>> row, int n);
        bool rowContains(List<HashSet<int>> row, int value);
        List<HashSet<int>> gridFlatten(List<List<HashSet<int>>> grid, int x, int y);
        void removePointingPairRow(List<HashSet<int>> row, int indexOfBox, int need);
    }
}
