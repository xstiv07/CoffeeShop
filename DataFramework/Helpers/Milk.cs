﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Helpers
{
    public enum Milk
    {
        None,
        Whole,
        [Display(Name="Low Fat")]
        Lowfat
    }
}
