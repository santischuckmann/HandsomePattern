
using HandsomePattern.Core.Interfaces.Repositories;
using HandsomePattern.Infrastructure.Data;

//comentario para que no se recree
namespace HandsomePattern.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly HandsomePatternContext _context;
        #region RepositoriesAttributes


        #endregion

        public UnitOfWork(HandsomePatternContext context)
        {
            _context = context;
        }

        #region RepositoriesProperties


        #endregion

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

