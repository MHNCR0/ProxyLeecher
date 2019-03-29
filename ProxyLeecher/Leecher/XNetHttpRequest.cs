using System;
using xNet;

namespace ProxyLeecher.Leecher
{
    public static class XNetHttpRequest
    {
        public static Tuple<Boolean, String> Get(String url)
        {
            Tuple<Boolean, String> result = null;

            try
            {
                HttpRequest req = new HttpRequest();
                var response = req.Get(url);

                if (response.IsOK && !response.HasError)
                    return result = new Tuple<bool, string>(true, response.ToString());
                else
                    return result = new Tuple<bool, string>(false, null);
            }
            catch (Exception ex)
            {
                return result = new Tuple<bool, string>(false, ex.Message);
            }
        }
    }
}
