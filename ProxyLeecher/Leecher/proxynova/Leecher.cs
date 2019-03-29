using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyLeecher.Leecher.proxynova
{
    class Leecher : ILeecher
    {
        public string address => "https://www.proxynova.com/proxy-server-list/";

        public List<String> StartLeech()
        {
            var g = XNetHttpRequest.Get(address);

            if (!g.Item1)
                return null;

            List<String> fl = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(g.Item2);

            var s = doc.DocumentNode.SelectNodes("//div[@id=\"bla\"]")[0].SelectNodes("//table")[0].SelectNodes("//tbody")[0]/*.SelectNodes("//tr")*/;

            foreach (var ns in s.ChildNodes.Where(a => a.OriginalName == "tr"))
            {
                var wd1 = ns.SelectNodes("//td");
                foreach (var ns2 in wd1)
                    fl.Add(ns2.InnerText.Trim().Replace("document.write('12345678", "").Replace("'.substr(8) + '", "").Replace("');", "") + ":" + ns.SelectNodes("//td")[1].InnerText.Trim());
            }

            return MatchWithFilters(fl);
        }

        private List<String> MatchWithFilters(List<String> proxys)
        {
            List<String> outProxys = new List<string>();

            if (Statics.PortFilter != "None")
                foreach (var s in proxys)
                {
                    if (outProxys.Count == Statics.added + Statics.Count)
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
                    if (outProxys.Count == Statics.added + Statics.Count)
                        break;
                    else
                        outProxys.Add(s);
                }

            return outProxys;
        }
    }
}
