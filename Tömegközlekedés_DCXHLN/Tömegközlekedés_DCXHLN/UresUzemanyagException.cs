using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class UresUzemanyagException : Exception
    {
        public UresUzemanyagException() : base("\tA járműnek nincs ekkora üzemanyagkapacitása")
        {
        }
    }
}
