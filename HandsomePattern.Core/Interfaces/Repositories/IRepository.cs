
using HandsomePattern.Core.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandsomePattern.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task Add(T entity);

        void Update(T entity);

        Task Delete(int id);

        Task AddList(IEnumerable<T> entities);

        void UpdateList(IEnumerable<T> entity);
        System.Linq.IQueryable<T> GetAllQuery();
    }
}
