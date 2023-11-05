using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;

namespace FastBank.Infrastructure.Repository
{
    public class Repository : IRepository
    {
        public Repository(
            FastBankDbContext context)
        {
            _context =  ;
        }

        readonly FastBankDbContext _context;

        public IQueryable<T> Set<T>() where T : class
        {
            return _context.Set<T>().AsQueryable<T>();
        }

        public IQueryable<T> Set<T>(params string[] includes) where T : class
        {
            var query = _context.Set<T>().AsQueryable<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        public IQueryable<T> SetNoTracking<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public IQueryable<T> SetNoTracking<T>(params string[] includes) where T : class
        {
            var query = _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        public void Add<T>(T obj) where T : class
        {
            _context.Set<T>().Add(obj);
            SaveChanges();
        }

        public void Update<T>(T obj) where T : class
        {
            _context.Entry<T>(obj).State = EntityState.Modified;
            SaveChanges();
        }

        public void BulkUpdate<T>(List<T> list) where T : class
        {
            _context.UpdateRange(list);
            SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            _context.Entry<T>(obj).State = EntityState.Deleted;
            SaveChanges();
        }

        void SaveChanges()
        {
            _context.SaveChanges();
        }

        T Materialize<T>(IDataRecord record) where T : new()
        {
            var t = new T();

            foreach (var prop in typeof(T).GetProperties())
            {
                // If collection, bypass it.
                if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    continue;
                }

                // If property is NotMapped, bypass it.
                if (Attribute.IsDefined(prop, typeof(NotMappedAttribute)))
                {
                    continue;
                }

                var dbValue = record[prop.Name];

                if (dbValue is DBNull)
                {
                    continue;
                }


                if (prop.PropertyType.IsConstructedGenericType
                    && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var baseType = prop.PropertyType.GetGenericArguments()[0];
                    var baseValue = Convert.ChangeType(dbValue, baseType);
                    var value = Activator.CreateInstance(prop.PropertyType, baseValue);
                    prop.SetValue(t, value);
                }
                else
                {
                    var value = Convert.ChangeType(dbValue, prop.PropertyType);
                    prop.SetValue(t, value);
                }
            }

            return t;
        }

        IList<T> Translate<T>(DbDataReader reader) where T : new()
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var record = Materialize<T>((IDataRecord)reader);

                results.Add(record);
            }

            return results;
        }
    }
}
