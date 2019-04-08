using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common.Cache;
using System;

namespace Icon.Web.DataAccess.Decorators
{
    public class CachingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private ICache cache;
        private ICachePolicy<TQuery> policy;
        private ILogger logger;

        public CachingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated,
            ICache cache,
            ICachePolicy<TQuery> policy,
            ILogger logger)
        {
            this.decorated = decorated;
            this.cache = cache;
            this.policy = policy;
            this.logger = logger;
        }

        public TResult Search(TQuery parameters)
        {
            var key = this.GetType().GetGenericArguments()[0].Name;
            var result = (TResult)this.cache.Get(key);
            
            if (result == null)
            {
                this.logger.Debug(String.Format("There is no Cache available for the {0} key.  Querying database...", key));
                result = this.decorated.Search(parameters);
                this.cache.Set(key, result, this.policy.AbsoluteExpiration);
            }

            return result;
        }
    }
}
