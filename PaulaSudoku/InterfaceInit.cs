using Microsoft.Practices.Unity;
using PaulaSudokuInterfaces;
using PaulaSudokuInterfaces.Interfaces;
using PaulaSudokuInterfaces.Search;
using PaulaSudokuInterfaces.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaulaSudoku
{
    public class InterfaceInit
    {
        //create injection container to resolve dependencies
        public static UnityContainer start()
        {
            var container = new UnityContainer();
            container.RegisterType<ISolution, SudokuSolver>();
            container.RegisterType<ISearch, SudokuBoard>();

            return container;
        }
    }
}
