using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain.Interfaces;

namespace UMS.Infrastructure.Data.Repositories
{
    public class unitOfWork : IunitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public unitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
          
        }

        public IUserRepository User { get; }
       

        public int Complete()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return _context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> RtCompleteAsync()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            // Save changes asynchronously and return the number of entities saved
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            return await _context.Database.BeginTransactionAsync(isolationLevel);
        }

        public void Dispose()
        {
            Dispose(true);
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }
    }
}
