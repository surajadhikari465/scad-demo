using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSupport.DataAccess.Models;
using WebSupport.ViewModels;

namespace WebSupport.Models
{
    public class CheckPointRequestBuilderModel: CheckPointRequestViewModel
    {
       public PriceResetPrice getCurrentPriceInfo { get; set; }
    }
}