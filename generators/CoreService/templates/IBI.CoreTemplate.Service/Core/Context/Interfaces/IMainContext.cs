using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IBI.<%= Name %>.Service.Core.Context.Interfaces
{
    public interface IMainContext
    {
        #region Properties

        IDbContextTransaction Transaction { get; set; }

        #endregion Properties

        #region Methods

        void CommitTransaction();

        DbContext GetContext();

        void RollBackTransaction();

        int SaveChanges();

        void StartTransaction();

        #endregion Methods
    }
}