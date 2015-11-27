using DataFramework;
using DataFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace Restbucks.Controllers
{
    public class OrderController : BaseController
    {
        // GET api/order
        // Produces a list of all orders that are not completed and are not cancelled
        public List<Order> Get()
        {
            var completedOrders = db.Orders.Where(x => x.Status != OrderStatus.Completed && x.Status != OrderStatus.Cancelled).ToList();
            return completedOrders;
        }

        // GET api/order/5
        // get the specific order
        public Order Get(int id)
        {
            var order = db.Orders.Find(id);

            if (order != null)
            {
                return order;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }

        // POST api/order
        //[ValidateModelState]
        //no validation yet
        public Order Post(XElement orderXML)
        {
            OrderLocation loc;
            OrderStatus stat;

            Enum.TryParse(orderXML.Element("Location").Value, out loc);
            Enum.TryParse(orderXML.Element("Status").Value, out stat);

            var order = new Order()
            {
                UniqueId = Guid.NewGuid(),
                Location = loc,
                Status = stat,
                Total = 0
            };

            manageOrder(orderXML, order);
            
            db.Orders.Add(order);
            db.SaveChanges();

            var createdOrder = db.Orders.Where(x => x.UniqueId == order.UniqueId).FirstOrDefault();
            return createdOrder;
        }

        // PUT api/order/5
        public Order Put(int id, [FromBody]XElement orderXML)
        {
            var order = db.Orders.Where(x => x.Id == id).FirstOrDefault();

            if (order != null)
            {
                OrderLocation loc;
                OrderStatus stat;

                Enum.TryParse(orderXML.Element("Location").Value, out loc);
                Enum.TryParse(orderXML.Element("Status").Value, out stat);

                order.Location = loc;
                order.Status = stat;
                order.Total = 0;

                //remove all existing order items;
                var existingOrderLines = db.Lines.Where(x => x.OrderUniqueId == order.UniqueId).ToList();
                existingOrderLines.ForEach(x => x.isDeleted = true);
                db.SaveChanges();

                manageOrder(orderXML, order);

                db.SaveChanges();

                var updatedOrder = db.Orders.Where(x => x.UniqueId == order.UniqueId).FirstOrDefault();
                return updatedOrder;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }

        [HttpPost]
        public Order AcceptPayment(int id)
        {
            var order = db.Orders.Where(x => x.Id == id).FirstOrDefault();

            if (order != null)
            {
                order.Status = OrderStatus.PaymentAccepted;
                db.SaveChanges();

                return order;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }

        // DELETE api/order/5
        public Order Delete(int id)
        {
            var order = db.Orders.Where(x => x.Id == id).FirstOrDefault();

            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                db.SaveChanges();

                return order;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }

        private Order manageOrder(XElement orderXML, Order order)
        {
            var orderItems = orderXML.Element("Items").Elements("Item");

            foreach (var item in orderItems)
            {
                var itemId = Int32.Parse(item.Element("Id").Value);

                var itemToAdd = db.Items.Where(x => x.Id == itemId).FirstOrDefault();

                if (itemToAdd != null)
                {
                    var line = new Line()
                    {
                        ItemId = Int32.Parse(item.Element("Id").Value),
                        OrderUniqueId = order.UniqueId,
                        LineQty = Int32.Parse(item.Element("Quantity").Value),
                        LinePrice = itemToAdd.Price * Int32.Parse(item.Element("Quantity").Value)
                    };
                    db.Lines.Add(line);
                    order.Total += Convert.ToDecimal(line.LinePrice);
                }
            }

            return order;
        }

    }
}
