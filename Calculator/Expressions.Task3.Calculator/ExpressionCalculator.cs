using System.Linq.Expressions;

namespace Expressions.Task3.Calculator
{
    public class ExpressionCalculator
    {
        public double Calculate(string expressionLine)
        {
            var expression = ExpressionConverter.TranslateLineToExpression(expressionLine);
            var compiledExpression = Expression.Lambda<Func<double>>(expression).Compile();

            return compiledExpression();
        }
    }
}
