using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IBI.<%= Name %>.Service.Core.Context
{
    public partial class MainContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Creates the transaction to wrap the updates/inserts/deletes in and around a transaction to make it a single unit
        /// </summary>
        public IDbContextTransaction Transaction { get; set; }

        //protected DbSet<ErpActiveDirectory> ErpActiveDirectory { get; set; }

        #endregion Properties

        #region Constructors

        public MainContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructors

        #region Transaction

        /// <summary>
        /// Commits the transaction that was created and clears it out
        /// </summary>
        public void CommitTransaction()
        {
            if (this.Transaction != null) this.Transaction.Commit();
            this.Transaction = null;
        }

        /// <summary>
        /// Rolls back the transaction that was created and clears it out
        /// </summary>
        public void RollBackTransaction()
        {
            if (this.Transaction != null) this.Transaction.Rollback();
            this.Transaction = null;
        }

        public int SaveChanges() => base.SaveChanges();

        /// <summary>
        /// Creates a db Transaction to run the updates in
        /// </summary>
        public void StartTransaction()
        {
            if (this.Transaction != null) this.Transaction = this.Database.BeginTransaction();
        }

        #endregion Transaction
    }
}