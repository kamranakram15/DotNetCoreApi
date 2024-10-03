using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Interfaces
{
    public interface IunitOfWork : IDisposable
    {

        IUserRepository User { get; }
      






        int Complete();
        Task CompleteAsync();
        Task<int> RtCompleteAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    }
}
