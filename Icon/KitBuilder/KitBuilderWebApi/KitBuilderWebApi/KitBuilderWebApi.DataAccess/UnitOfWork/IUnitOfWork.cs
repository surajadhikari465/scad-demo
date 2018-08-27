using KitBuilderWebApi.DatabaseModels;
using System;

namespace KitBuilderWebApi.DataAccess.UnitOfWork
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