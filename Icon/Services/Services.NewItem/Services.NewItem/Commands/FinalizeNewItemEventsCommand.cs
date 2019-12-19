﻿using Services.NewItem.Infrastructure;
using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Commands
{
    public class FinalizeNewItemEventsCommand : IRegionalParameter
    {
        public bool ErrorOccurred { get; set; }
        public int Instance { get; set; }
        public IEnumerable<NewItemModel> NewItems { get; set; }
        public string Region { get; set; }
    }
}
