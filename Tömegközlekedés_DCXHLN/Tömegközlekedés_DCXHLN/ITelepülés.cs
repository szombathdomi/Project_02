using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    interface ITelepülés
    {
        string TelepülésNév { get; set; }
        string Megye { get; set; }
    }
}
