using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShedlR.Domain.Repository;

namespace ShedlR.Domain.Interfaces
{
    public interface IEFRepository<TEntity> : IRepository
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        string includeProperties = "");
        IList<TEntity> Get(out int fullCount, int startIndex, int count,
                                    Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string sorting = "");
        //IList<TEntity> GetSqlQuery(string query, IDictionary<string, object> parameters, out int fullCount, string sorting = "", int count = 0, int startIndex = 0);
        IEnumerable<TEntity> GetSqlQuery(string sqlName, SqlParameter[] parameters);
        IEnumerable<TEntity> FunctionTableValue(string functionName, SqlParameter[] parameters);
        IEnumerable<TEntity> SPTableValue(string functionName, SqlParameter[] parameters);
        TEntity GetByID(object id);
        int Insert(TEntity entityToInsert);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}
