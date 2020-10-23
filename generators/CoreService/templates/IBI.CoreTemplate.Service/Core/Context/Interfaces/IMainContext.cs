using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IBI.<%= Name %>.Service.Core.Context.Interfaces
{
    /// <summary>
    /// Interface of the main dbcontext
    /// </summary>
    public interface IMainContext
    {
        #region Properties

        /// <summary>
        /// Transaction for the database
        /// </summary>
        IDbContextTransaction Transaction { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Get the current context
        /// </summary>
        /// <returns><see cref="DbContext"/></returns>
        DbContext GetContext();

        /// <summary>
        /// Roll the transaction back
        /// </summary>
        void RollBackTransaction();

        ///// <summary>
        ///// Save
        ///// </summary>
        ///// <returns></returns>
        //int SaveChanges();

        /// <summary>
        /// Starts a database transaction
        /// </summary>
        void StartTransaction();

        #endregion Methods
    }
}