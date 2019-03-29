using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLeecher.Leecher
{
    interface ILeecher
    {
        String address { get; }
        List<String> StartLeech();
    }
}
