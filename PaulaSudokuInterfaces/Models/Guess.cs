using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudukoSolverLib.Models
{
    public class Guess
    {
        public Guess()
        {
            Options = new HashSet<int>();
        }

        public int Value { get; set; }
        public HashSet<int> Options { get; set; }
    }
}
