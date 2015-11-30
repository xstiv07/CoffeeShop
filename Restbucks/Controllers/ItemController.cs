using DataFramework;
using DataFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml.Linq;

namespace Restbucks.Controllers
{
    public class ItemController : BaseController
    {
        //produces a list of all existing items
        public List<Item> Get()
        {
            var existingItems = db.Items.Where(x => x.isDeleted != true).ToList();

            return existingItems;
        }

        public Item Get(int id)
        {
            var item = db.Items.Where(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                return item;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }


        public Item Post(XElement itemXML)
        {
            try
            {
                Milk milk;
                Size size;

                if (itemXML.Element("Milk") != null)
                {
                    Enum.TryParse(itemXML.Element("Milk").Value, out milk);
                }
                else
                {
                    milk = Milk.undefined;
                }

                Enum.TryParse(itemXML.Element("Size").Value, out size);

                var item = new Item()
                {
                    UniqueId = Guid.NewGuid(),
                    Name = itemXML.Element("Name").Value,
                    Description = itemXML.Element("Description").Value,
                    Price = Decimal.Parse(itemXML.Element("Price").Value),
                    Quantity = Int32.Parse(itemXML.Element("Quantity").Value),
                    ImageURL = itemXML.Element("ImageURL").Value,
                    Milk = milk,
                    Size = size
                };

                db.Items.Add(item);
                db.SaveChanges();

                var createdItem = db.Items.Where(x => x.UniqueId == item.UniqueId).FirstOrDefault();

                return createdItem;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.InnerException.ToString()),
                    ReasonPhrase = "Critical Exception"
                });
            }
        }

        public Item Put(int id, XElement itemXML)
        {
            var itemToChange = db.Items.Where(x => x.Id == id).FirstOrDefault();

            if (itemToChange != null)
            {
                try
                {
                    Milk milk;
                    Size size;

                    if (itemXML.Element("Milk") != null)
                    {
                        Enum.TryParse(itemXML.Element("Milk").Value, out milk);
                    }
                    else
                    {
                        milk = Milk.undefined;
                    }
                    Enum.TryParse(itemXML.Element("Size").Value, out size);

                    itemToChange.Name = itemXML.Element("Name").Value;
                    itemToChange.Description = itemXML.Element("Description").Value;
                    itemToChange.Price = Decimal.Parse(itemXML.Element("Price").Value);
                    itemToChange.Quantity = Int32.Parse(itemXML.Element("Quantity").Value);
                    itemToChange.ImageURL = itemXML.Element("ImageURL").Value;
                    itemToChange.Milk = milk;
                    itemToChange.Size = size;

                    db.SaveChanges();

                    return itemToChange;
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(ex.InnerException.ToString()),
                        ReasonPhrase = "Critical Exception"
                    });
                }
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }

        public Item Delete(int id)
        {
            var item = db.Items.Where(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                item.isDeleted = true;
                db.SaveChanges();

                return item;
            }
            else
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
        }
    }
}
