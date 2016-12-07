using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using ShedlR.Domain.Interfaces;

namespace ShedlR.Domain.Repository
{
    public class EFGenericRepository<T> : IEFRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private DbContext _context;

        public EFGenericRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != "")
            {
                query = query.Include(includeProperties);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(c => orderBy);
            }
            return query;
        }

        public IList<T> Get(out int fullCount, int startIndex, int count, System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string sorting = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (sorting != "")
            {
                query = query.OrderBy(sorting);
            }

            fullCount = query.Count();

            return count > 0
                       ? query.Skip(startIndex).Take(count).ToList() // Paging
                       : query.ToList(); // No paging

        }

        //public IList<T> GetSqlQuery(string query, IDictionary<string, object> parameters, out int fullCount, string sorting = "", int count = 0, int startIndex = 0)
        //{
        //    IQueryable<T> current_query = _dbSet;

        //    //object[] sqlparameters;
        //    List<ObjectParameter> sqlparameters;

        //    if (parameters.Count != 0)
        //    {
        //        int i = 0;
        //        //sqlparameters = new object[parameters.Count];
        //        sqlparameters = new List<ObjectParameter>();//new ObjectParameter[parameters.Count];

        //        foreach (var param in parameters)
        //        {
        //            //sqlparameters.SetValue(param.Value, i);
        //            //i++;
        //            var par = new ObjectParameter(param.Key.ToString(), param.Value);
        //            sqlparameters.Add(par);
        //            i++;
        //        }
        //        current_query = _dbSet.SqlQuery(query, sqlparameters.ToArray()).AsQueryable();
        //    }
        //    fullCount = current_query.Count();
        //    return current_query.ToList<T>();
        //}
        public IEnumerable<T> GetSqlQuery(string sqlName, SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[] { };
            var currentQuery = _context.Database.SqlQuery<T>(sqlName, parameters);
            return currentQuery.ToArray();
        }

        public IEnumerable<T> FunctionTableValue(string functionName, SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[] { };

            string commandText = String.Format("SELECT * FROM dbo.{0}", String.Format("{0}({1})", functionName, String.Join(",", parameters.Select(x => x.ParameterName))));
            ObjectContext.CommandTimeout = 360;
            var t = ObjectContext.ExecuteStoreQuery<T>(commandText, parameters).ToArray();
            return t;
        }

        public IEnumerable<T> SPTableValue(string functionName, SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[] { };

            string commandText = String.Format("EXECUTE dbo.{0}", String.Format("{0} {1}", functionName, String.Join(",", parameters.Select(x => x.ParameterName))));
            ObjectContext.CommandTimeout = 360;
            var t = ObjectContext.ExecuteStoreQuery<T>(commandText, parameters).ToArray();
            return t;
        }

        private ObjectContext ObjectContext
        {
            get { return (_context as IObjectContextAdapter).ObjectContext; }
        }

        public T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public int Insert(T entityToInsert)
        {
            _dbSet.Add(entityToInsert);

            return 1;
        }

        public void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public void Update(T entityToUpdate)
        {
            var updatedEntity = _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
