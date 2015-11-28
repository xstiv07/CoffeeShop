using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBucksAPIWrapper
{
    public class Items
    {
        const string connectTo = "item";

        public string GetItems(String apiUrl)
        {
            return BaseClass.CallApi(apiUrl + connectTo, "GET");
        }

        public string GetItem(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "GET");
        }

        public string PostItem(String apiUrl, string content)
        {
            return BaseClass.CallApi(apiUrl + connectTo, "POST", content);
        }

        public string UpdateItem(String apiUrl, string content, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "PUT", content);
        }

        public string DeleteItem(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "DELETE");
        }
    }
}
