using RestBucksAPIWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Items();

            var itemString = @"<Item xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Id>4</Id><UniqueId>c5e1b9c1-0a16-47b9-85da-5cdfdd3de5a5</UniqueId><Name>Cappuccino</Name><Description>This is cappuccino description</Description><Price>18.18</Price><Milk>whole</Milk><Size>small</Size><isDeleted>false</isDeleted></Item>";

            //var t = x.PostItem("http://localhost:2873/api/", itemString);

            var t = x.UpdateItem("http://localhost:2873/api/", itemString, 6);

            var tt = t;
        }
    }
}
