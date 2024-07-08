using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class CelException : Exception
    {
        public CelException() : base("A cél település nincs az adatbázisban")
        {
        }
    }
}
