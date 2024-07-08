using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    public class Megyeszékhely : ITelepülés
    {
        public string TelepülésNév { get; set; }
        public string Megye { get; set; }

        public Megyeszékhely(string nev, string megye)
        {
            TelepülésNév = nev;
            Megye = megye;
        }
    }
}
