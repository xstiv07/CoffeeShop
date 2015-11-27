using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFramework;
using DataFramework.MetaData;
using System.Runtime.Serialization;

namespace DataFramework
{
    [MetadataType(typeof (ItemMetaData))]
    public partial class Item
    {
    }

    [MetadataType(typeof(OrderMetaData))]
    public partial class Order
    {
    }
}
