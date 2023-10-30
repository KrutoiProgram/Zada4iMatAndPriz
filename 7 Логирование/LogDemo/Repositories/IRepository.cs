using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Linq.Expressions;

namespace LogDemo.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T? Find(int id);

        List<T> Where(Expression<Func<T, bool>> predicate);

        void Insert(T entity);
        void Update(int id, T entity);
        void Delete(int id);
    }
}
