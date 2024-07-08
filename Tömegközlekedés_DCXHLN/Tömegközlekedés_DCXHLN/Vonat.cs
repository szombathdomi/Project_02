using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class Vonat : Jarmu
    {
        public Vonat(string nev, double hatotav, double uzemanyagSzint) : base(nev, hatotav, uzemanyagSzint)
        {
        }
    }
}
