
using HandsomePattern.Infrastructure.Data;
using HandsomePattern.Core.Interfaces.Repositories
using Microsoft.EntityFrameworkCore;

namespace HandsomePattern.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        #region Variables
        protected readonly HandsomePatternContext _context;
        protected readonly DbSet<T> _entities;
        #endregion

        #region Constructor
        public BaseRepository(HandsomePatternContext context)
        {
            this._context = context ?? throw new System.ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }
        #endregion

        public IQueryable<T> GetAllQuery()
        {
            return _entities;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AddList(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void UpdateList(IEnumerable<T> entity)
        {
            _entities.UpdateRange(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}
