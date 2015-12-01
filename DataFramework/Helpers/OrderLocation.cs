using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Helpers
{
    public enum OrderLocation
    {
        undefined,
        [Display(Name="Order In")]
        orderIn,
        [Display(Name="Order Out")]
        orderOut
    }
}
