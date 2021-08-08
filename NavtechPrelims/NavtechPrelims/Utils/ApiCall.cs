using System.IO;
using System.Net;

namespace NavtechPrelims.Utils
{
    public static class ApiCall
    {
        /// <summary>
        /// GET API Call
        /// </summary>
        /// <param name="ApiUrl"></param>
        /// <returns></returns>
        public static string GetApi(string ApiUrl)
        {
            try
            {
                var responseString = "";
                var request = (HttpWebRequest)WebRequest.Create(ApiUrl);
                request.Method = "GET";
                request.ContentType = "application/json";

                using (var objResponse = request.GetResponse())
                {
                    using (var reader = new StreamReader(objResponse.GetResponseStream()))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            catch
            {
                throw;
            }

        }
    }
}
