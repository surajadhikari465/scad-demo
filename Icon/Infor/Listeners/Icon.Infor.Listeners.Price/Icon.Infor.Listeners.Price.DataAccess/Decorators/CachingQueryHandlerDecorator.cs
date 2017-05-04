using Icon.Caching;
using Mammoth.Common.DataAccess.CommandQuery;
using System;

namespace Icon.Infor.Listeners.Price.DataAccess.Decorators
{
    public class CachingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private ICache cache;
        private ICachePolicy<TQuery> policy;

        public CachingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated,
            ICache cache,
            ICachePolicy<TQuery> policy)
        {
            this.decorated = decorated;
            this.cache = cache;
            this.policy = policy;
        }

        public TResult Search(TQuery parameters)
        {
            var key = this.GetType().GetGenericArguments()[0].Name;
            var result = (TResult)this.cache.Get(key);

            if (result == null)
            {
                result = this.decorated.Search(parameters);
                this.cache.Set(key, result, this.policy.AbsoluteExpiration);
            }

            return result;
        }
    }
}
