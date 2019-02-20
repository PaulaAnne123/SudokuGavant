using Microsoft.Practices.Unity;
using PaulaSudokuInterfaces;
using PaulaSudokuInterfaces.Interfaces;
using PaulaSudokuInterfaces.Search;
using PaulaSudokuInterfaces.Search.Interfaces;

namespace PaulaSudoku
{
    public class InterfaceInit
    {
        //create injection container to resolve dependencies, easier to instantiate
        public static UnityContainer start()
        {
            var container = new UnityContainer();
            //Map ISolution with SudokuSolver and ISearch with SudokuBoard
            container.RegisterType<ISolution, SudokuSolver>();
            container.RegisterType<ISearch, SudokuBoard>();

            return container;
        }
    }
}
