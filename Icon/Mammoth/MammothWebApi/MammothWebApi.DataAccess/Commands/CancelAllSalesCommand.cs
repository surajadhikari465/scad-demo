using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Commands
{
    public class CancelAllSalesCommand
    {
        public String Region { get; set; }
        public List<CancelAllSalesModel> CancelAllSalesModelList { get; set; }
        public DateTime Timestamp { get; set; }
        public int MessageActionId { get; set; }
    }
}