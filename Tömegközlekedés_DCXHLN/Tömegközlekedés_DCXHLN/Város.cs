using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    public class Város : ITelepülés
    {
        public string TelepülésNév { get; set; }
        public string Megye { get; set; }

        public Város(string nev, string megye)
        {
            TelepülésNév = nev;
            Megye = megye;
        }
    }
}
