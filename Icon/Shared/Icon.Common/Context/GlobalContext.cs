using System;
using System.Data;
using System.Data.Entity;

namespace Icon.Common.Context
{
    public class GlobalContext<T> : IRenewableContext<T>
        where T : DbContext, new()
    {
        private T context;

        public T Context
        {
            get
            {
                if (context == null)
                {
                    context = new T();
                }
                return context;
            }
            set { context = value; }
        }

        public GlobalContext() { }

        public GlobalContext(T context)
        {
            this.context = context;
            if (this.context.Database.Connection.State == ConnectionState.Closed)
            {
                this.context.Database.Connection.Open();
            }
        }

        public void Refresh()
        {
            if(this.context != null)
            {
                this.context.Dispose();
            }
            this.context = new T();
            this.context.Database.Connection.Open();
        }

        public void Refresh(object parameters)
        {
            throw new NotImplementedException();
        }
    }
}
