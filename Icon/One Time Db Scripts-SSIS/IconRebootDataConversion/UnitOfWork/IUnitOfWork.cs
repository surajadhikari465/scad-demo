using System;
using ConvertData;

namespace DataAccess.UnitOfWork
{
    /// <summary>
    /// Unit of work abstraction for use with repo
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets or sets Context
        /// </summary>
        iCONDevEntities Context { get; set; }

        /// <summary>
        /// Commits context items to data store
        /// </summary>
        void Commit();
    }
}