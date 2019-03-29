using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLeecher.Leecher.proxyserverlist24
{
    class Leecher : ILeecher
    {
        public string address => "http://www.proxyserverlist24.top/?m=1";

        public List<String> StartLeech()
        {
            String Link = GetLastLink();
            var proxys = XNetHttpRequest.Get(Link);

            if (!proxys.Item1)
                return null;
            else
            {
                List<String> Proxys = GetProxys(proxys.Item2);
                var yourProxys = MatchWithFilters(Proxys);

                return yourProxys;
            }
        }

        private List<String> MatchWithFilters(List<String> proxys)
        {
            List<String> outProxys = new List<string>();

            if (Statics.PortFilter != "None")
                foreach (var s in proxys)
                {
                    if (outProxys.Count == Statics.Count)
                        break;
                    else
                    {
                        if (s.Split(':')[1] == Statics.PortFilter)
                            outProxys.Add(s);
                    }
                }
            else
                foreach (var s in proxys)
                {
                    if (outProxys.Count == Statics.Count)
                        break;
                    else
                        outProxys.Add(s);
                }

            return outProxys;
        }

        private List<string> GetProxys(string l)
        {
            List<String> fl = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(l);

            var s = doc.DocumentNode.SelectNodes("//pre")[0].ChildNodes[0].InnerText;

            foreach (String str in s.Split('\n'))
                fl.Add(str);

            return fl;
        }

        private String GetLastLink()
        {
            var g = XNetHttpRequest.Get(address);

            if (!g.Item1)
                return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(g.Item2);

            var divs = doc.DocumentNode.SelectNodes("//div[@class='mobile-post-outer']");
            return divs[0].SelectNodes("//a[@href]")[1].Attributes[0].Value;
        }
    }
}
