using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBucksAPIWrapper
{
    public class Orders
    {
        const string connectTo = "order";

        public string GetOrders(String apiUrl)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "/getall", "GET");
        }

        public string GetPendingOrders(String apiUrl)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "/getpending", "GET");
        }

        public string GetOrder(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "GET");
        }

        public string PostOrder(String apiUrl, string content)
        {
            return BaseClass.CallApi(apiUrl + connectTo, "POST", content);
        }

        public string ChangeOrder(String apiUrl, string content, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "PUT", content);
        }

        public string AcceptOrderPayment(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "AcceptPayment/?id=" + id, "POST");
        }

        public string DeleteOrder(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "?id=" + id, "DELETE");
        }

        public string MarkDeleted(String apiUrl, int id)
        {
            return BaseClass.CallApi(apiUrl + connectTo + "/markdeleted/" + id, "DELETE");
        }
    }
}
