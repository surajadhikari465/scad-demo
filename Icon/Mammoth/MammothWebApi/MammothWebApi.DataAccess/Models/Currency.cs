using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Currency
    {
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyDesc { get; set; }
    }
}
