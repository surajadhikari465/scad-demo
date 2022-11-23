﻿using GPMService.Producer.Model.DBModel;
using Mammoth.Framework;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface IActivePriceProcessorDAL
    {
        IEnumerable<GetActivePricesQueryModel> GetActivePrices(MammothContext mammothContext, string region);
    }
}
