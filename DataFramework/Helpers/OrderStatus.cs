using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Helpers
{
    public enum OrderStatus
    {
        undefined,
        [Display(Name = "Payment Expected")]
        PaymentExpected,
        [Display(Name = "Payment Accepted")]
        PaymentAccepted,
        Preparing,
        Ready,
        Completed,
        Cancelled
    }
}
