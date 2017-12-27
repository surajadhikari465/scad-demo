using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Esb.Core.MessageBuilders;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess;

namespace WebSupport.MessageBuilders
{
    public class PriceRefreshMessageBuilderFactory : IPriceRefreshMessageBuilderFactory
    {
        public IMessageBuilder<List<GpmPrice>> CreateMessageBuilder(string system)
        {
            switch(system)
            {
                case PriceRefreshConstants.R10:
                case PriceRefreshConstants.IRMA:
                    return new PriceRefreshMessageBuilder();
                default: throw new ArgumentException($"{system} is not a valid downstream system for price refreshes.", nameof(system));
            }
        }
    }
}