using System.Linq.Expressions;
using System.Collections;

namespace Expressions.Task3.UnderQueryableCurtains
{
    public class RelationalDatabaseQueryable<TElement> : IQueryable<TElement>
    {
        private readonly IQueryProvider _provider;
        private readonly Expression _expression;

        public RelationalDatabaseQueryable(IQueryProvider provider, Expression expression)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public Type ElementType => typeof(TElement);

        public Expression Expression => _expression;

        public IQueryProvider Provider => _provider;

        public IEnumerator<TElement> GetEnumerator()
        {
            return ((IEnumerable<TElement>)_provider.Execute(_expression)!).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_provider.Execute(_expression)!).GetEnumerator();
        }
    }
}