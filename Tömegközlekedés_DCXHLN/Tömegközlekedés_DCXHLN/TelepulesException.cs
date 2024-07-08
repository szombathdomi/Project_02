using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class TelepulesException : Exception
    {
        public TelepulesException() : base("A települések nincsenek az adatbázisban")
        {
        }
    }
}
