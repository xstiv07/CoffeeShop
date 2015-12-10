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

        [Route("api/order/getpending")]
        public List<Order> GetPending()
        {
            var pendingOrders = db.Orders.Where(x => x.Status != OrderStatus.Cancelled && x.Status != OrderStatus.Deleted).ToList();
            return pendingOrders;
        }

        [Route("api/order/getall")]
        public List<Order> GetAll()
        {
            var allOrders = db.Orders.OrderByDescending(x => x.Id).ToList();
            return allOrders;
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
        public Order Post(XElement orderXML)
        {
            OrderLocation loc;
            OrderStatus stat;

            Enum.TryParse(orderXML.Element("Location").Value, out loc);
            Enum.TryParse(orderXML.Element("Status").Value, out stat);

            var order = new Order()
            {
                UniqueId = Guid.NewGuid(),
                CustomerFirstName = orderXML.Element("CustomerFirstName").Value,
                CustomerLastName = orderXML.Element("CustomerLastName").Value,
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
                order.CustomerFirstName = orderXML.Element("CustomerFirstName").Value;
                order.CustomerLastName = orderXML.Element("CustomerLastName").Value;

                var orderItems = orderXML.Element("Items");

                //if there are items coming together with an order
                if (orderItems != null)
                {
                    order.Total = 0;
                    //remove all existing order items;
                    var existingOrderLines = db.Lines.Where(x => x.OrderUniqueId == order.UniqueId).ToList();
                    existingOrderLines.ForEach(x => x.isDeleted = true);
                    db.SaveChanges();

                    manageOrder(orderXML, order);
                }

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

        [HttpGet]
        [Route("api/order/acceptpayment/{id}")]
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

        [HttpGet]
        [Route("api/order/getlineitems/{id}")]
        public List<Line> GetLineItems(Guid id)
        {
            var orderItems = db.Lines.Where(x => x.OrderUniqueId == id && !x.isDeleted).ToList();

            return orderItems;
        }

        [HttpDelete]
        [Route("api/order/deleteOrderItem/{itemId}/{orderUnique}")]
        public void DeleteOrderItem(int itemId, Guid orderUnique)
        {
            
            var line = db.Lines.Where(x => x.ItemId == itemId && x.OrderUniqueId == orderUnique && !x.isDeleted).FirstOrDefault();
            line.isDeleted = true;

            var order = db.Orders.Where(x => x.UniqueId == orderUnique).FirstOrDefault();
            if (order != null)
            {
                order.Total -= line.LinePrice;
            }

            db.SaveChanges();
        }

        [HttpPost]
        [Route("api/order/addOrderItem/{itemId}/{orderUnique}/{lineQty}")]
        public void AddOrderitem(int itemId, Guid orderUnique, int lineQty)
        {
            var item = db.Items.Where(x => x.Id == itemId).FirstOrDefault();
            var order = db.Orders.Where(x => x.UniqueId == orderUnique).FirstOrDefault();

            var line = new Line()
            {
                ItemId = itemId,
                OrderUniqueId = orderUnique,
                LineQty = lineQty,
                LinePrice = lineQty * item.Price
            };

            if (order != null)
            {
                order.Total += line.LinePrice;
            }

            db.Lines.Add(line);
            db.SaveChanges();
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

        [HttpDelete]
        [Route("api/order/markdeleted/{id}")]
        // DELETE api/order/5
        public Order MarkDeleted(int id)
        {
            var order = db.Orders.Where(x => x.Id == id).FirstOrDefault();

            if (order != null)
            {
                order.Status = OrderStatus.Deleted;
                order.isDeleted = true;
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
                    var x = itemToAdd.Price * Int32.Parse(item.Element("Quantity").Value);
                    var line = new Line()
                    {
                        ItemId = Int32.Parse(item.Element("Id").Value),
                        OrderUniqueId = order.UniqueId,
                        LineQty = Int32.Parse(item.Element("Quantity").Value),
                        LinePrice = x
                    };
                    db.Lines.Add(line);
                    order.Total += Convert.ToDecimal(line.LinePrice);
                }
            }

            return order;
        }

    }
}
