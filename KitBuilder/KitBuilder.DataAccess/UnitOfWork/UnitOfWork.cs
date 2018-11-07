using KitBuilder.DataAccess.DatabaseModels;

namespace KitBuilder.DataAccess.UnitOfWork
{
    /// <summary>
    /// UnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the EFUnitOfWork class
        /// </summary>
        public UnitOfWork(KitBuilderContext kitBuilderContext)
        {
            this.Context = kitBuilderContext;
        }

        /// <summary>
        /// Gets or sets Context
        /// </summary>
        public KitBuilderContext Context { get; set; }

        /// <summary>
        /// Commit to repo
        /// </summary>
        public void Commit()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Properly dispose of underlying context
        /// </summary>
        public void Dispose()
        {
            this.Context.Dispose();
        }

    }
}