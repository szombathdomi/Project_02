using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    public class Falu : ITelepülés
    {
        public string TelepülésNév { get; set; }
        public string Megye { get; set; }

        public Falu(string nev, string megye)
        {
            TelepülésNév = nev;
            Megye = megye;
        }
    }
}
