using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, Users>
    {
        private readonly IrmaContext context;

        public GetUserQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public Users Handle(GetUserQuery parameters)
        {
            return this.context.Users.FirstOrDefault(u => u.UserName == parameters.UserName);
        }
    }
}
