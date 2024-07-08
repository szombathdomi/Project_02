using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    abstract class Jarmu
    {
        public string JárműNév { get; set; }
        public double Hatotav { get; set; }
        public double ÜzemanyagSzint { get; set; }

        public Jarmu(string nev, double hatotav, double uzemanyagSzint)
        {
            JárműNév = nev;
            Hatotav = hatotav;
            ÜzemanyagSzint = uzemanyagSzint;
        }

    }
}
