using LogDemo.Data;
using LogDemo.Errors;
using System.Linq.Expressions;

namespace LogDemo.Repositories
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly TracksContext context;
        public Repository(TracksContext context) 
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            var toDelete = Find(id);
            if (toDelete is null)
            {
                throw new EntityNotFoundException("Invalid id");
            }
            context.Set<T>().Remove(toDelete);
            context.SaveChanges();
        }

        public T? Find(int id) => context.Set<T>().Find(id);

        public List<T> GetAll() => context.Set<T>().ToList();

        public void Insert(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(int id, T entity)
        {
            throw new NotImplementedException("Ошибка! Не реализовано намеренно. Так и задумано.");
        }

        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }
    }
}
