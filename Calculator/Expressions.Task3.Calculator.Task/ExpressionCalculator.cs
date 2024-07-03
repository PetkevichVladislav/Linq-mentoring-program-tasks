using System.Linq.Expressions;

namespace Expressions.Task3.Calculator
{
    public class ExpressionCalculator
    {
        public double Calculate(string expressionLine)
        {
            Expression expression = null!; //TODO: Implement string to expression converter.
            var compiledExpression = Expression.Lambda<Func<double>>(expression).Compile();

            return compiledExpression();
        }
    }
}
