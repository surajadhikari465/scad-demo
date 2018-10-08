using System;
using KitBuilder.DataAccess.DatabaseModels;

namespace KitBuilder.DataAccess.UnitOfWork
{
    /// <summary>
    /// Unit of work abstraction for use with repo
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets or sets Context
        /// </summary>
        KitBuilderContext Context { get; set; }

        /// <summary>
        /// Commits context items to data store
        /// </summary>
        void Commit();
    }
}