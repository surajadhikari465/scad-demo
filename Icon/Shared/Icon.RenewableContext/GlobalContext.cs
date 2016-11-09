using System;
using System.Data;
using System.Data.Entity;

namespace Icon.RenewableContext
{
    public class GlobalContext<T> : IRenewableContext<T>
        where T : DbContext, new()
    {
        public GlobalContext() { }

        public GlobalContext(T context)
        {
            Context = context;
            if (context.Database.Connection.State == ConnectionState.Closed)
            {
                Context.Database.Connection.Open();
            }
        }

        public T Context { get; set; }

        public void Refresh()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
            Context = new T();
            Context.Database.Connection.Open();
        }

        public void Refresh(object parameters)
        {
            throw new NotImplementedException();
        }
    }
}
