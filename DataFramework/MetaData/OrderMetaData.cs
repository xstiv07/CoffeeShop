using DataFramework.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace DataFramework.MetaData
{
    public class OrderMetaData
    {
        [Range(1, double.MaxValue, ErrorMessage = "Required")]
        public OrderStatus Status;
    }
}