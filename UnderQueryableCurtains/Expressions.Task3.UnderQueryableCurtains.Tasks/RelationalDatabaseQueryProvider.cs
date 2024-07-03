using System.Linq.Expressions;
using Expressions.Task3.UnderQueryableCurtains.Databases;

namespace Expressions.Task3.UnderQueryableCurtains
{
    public class RelationalDatabaseQueryProvider<TEntity> : IQueryProvider where TEntity : class
    {
        private RelationalDatabase _database;

        public RelationalDatabaseQueryProvider(RelationalDatabase db)
        {
            _database = db;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new RelationalDatabaseQueryable<TEntity>(this, expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new RelationalDatabaseQueryable<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            var visitor = new RelationalDatabaseExpressionVisitor();
            visitor.Visit(expression);
            var sql = visitor.GetExpressionSql();

            return _database.Query(sql);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }
    }
}