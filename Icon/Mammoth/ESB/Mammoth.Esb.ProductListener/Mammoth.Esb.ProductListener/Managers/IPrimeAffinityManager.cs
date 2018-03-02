using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Managers
{
    public interface IPrimeAffinityManager
    {
        void SendPrimeAffinityMessages(IEnumerable<ItemModel> items);
    }
}