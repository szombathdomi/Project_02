using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    class HatotavException : System.Exception
    {
        public HatotavException() : base("\tA járműnek nincs ekkora hatótávja")
        {
        }
    }
}
