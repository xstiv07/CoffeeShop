using DataFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CustomerStatusApp.Controllers
{
    public class HomeController : Controller
    {
        string _restBucksAPIURL;

        public HomeController()
        {
            _restBucksAPIURL = "http://localhost:2873/api/";
        }
        public ActionResult Index()
        {
            var orders = new RestBucksAPIWrapper.Orders();
            var ord = orders.GetPendingOrders(_restBucksAPIURL);

            JavaScriptSerializer js = new JavaScriptSerializer();

            var serializedOrders = js.Deserialize<List<Order>>(ord);

            return View(serializedOrders);
        }

        [HttpPost]
        public void getRidOfCompleted(List<int> ids)
        {
            var orders = new RestBucksAPIWrapper.Orders();

            if (ids != null)
            {
                foreach (var id in ids)
                {
                    orders.MarkDeleted(_restBucksAPIURL, id);
                }
            }
        }
    }
}