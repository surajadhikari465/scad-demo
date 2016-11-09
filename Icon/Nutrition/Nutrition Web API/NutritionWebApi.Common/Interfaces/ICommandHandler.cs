using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionWebApi.Common.Interfaces
{
    public interface ICommandHandler<TCommand>
    {
        string Execute(TCommand commandData);
    }
}
