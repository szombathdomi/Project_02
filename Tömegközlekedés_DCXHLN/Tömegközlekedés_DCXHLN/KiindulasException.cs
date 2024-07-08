using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class KiindulasException : Exception
    {
        public KiindulasException() : base("A kiindulási település nincs az adatbázisban")
        {
        }
    }
}
