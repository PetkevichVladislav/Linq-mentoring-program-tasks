using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Expressions.Task3.StringExtensions
{
    public static class StringExtensionMethods
    {
        public static string ExtractDigits(this string line)
        {
            var inputParam = Expression.Parameter(typeof(string), nameof(line));

            // Define string builder
            var newStringBuilder = Expression.New(typeof(StringBuilder));
            var stringBuilderVariable = Expression.Variable(typeof(StringBuilder), "numbers");
            var initStringBuilder = Expression.Assign(stringBuilderVariable, newStringBuilder);

            // Define loop
            var loopVariable = Expression.Variable(typeof(int), "i");
            var initLoopIndex = Expression.Assign(loopVariable, Expression.Constant(0));
            var loopCondition = Expression.LessThan(loopVariable, Expression.Property(inputParam, "Length"));
            
            // Define if block
            var getCharAction = Expression.Call(inputParam, typeof(string).GetMethod("get_Chars"), loopVariable);
            var checkIsDigitAction = Expression.Call(typeof(char).GetMethod("IsDigit", new[] { typeof(char) }), getCharAction);
            var appendToStringBuilderAction = Expression.Call(stringBuilderVariable, typeof(StringBuilder).GetMethod("Append", new[] { typeof(char) }), getCharAction);
            var blockIf = Expression.IfThen(checkIsDigitAction, appendToStringBuilderAction);

            // Combine members
            var loopContent = Expression.Block(blockIf, Expression.PreIncrementAssign(loopVariable));
            var loopBreakLabel = Expression.Label();
            var loop = Expression.Loop(
                Expression.IfThenElse(
                    loopCondition,
                    loopContent,
                    Expression.Break(loopBreakLabel)
                ),
                loopBreakLabel
            );

            var stringBuilderToString = Expression.Call(stringBuilderVariable, typeof(object).GetMethod("ToString"));
            var block = Expression.Block(new[] { stringBuilderVariable, loopVariable }, initStringBuilder, initLoopIndex, loop, stringBuilderToString);

            var lambda = Expression.Lambda<Func<string, string>>(block, inputParam);
            var extractDigitFunction = lambda.Compile();
            var result = extractDigitFunction(line);

            return result;
        }

        public static string RemoveTrailingSpaces(this string line)
        {
            var input = Expression.Parameter(typeof(string), nameof(line));

            var lambda = Expression.Lambda<Func<string, string>>(
                Expression.Call(typeof(Regex), "Replace", null, input,
                    Expression.Constant(@"\s{2,}", typeof(string)),
                    Expression.Constant(" ", typeof(string))
                ),
                input
            );

            var removeTrailingSpacesFunction = lambda.Compile();
            var result = removeTrailingSpacesFunction(line);

            return result;
        }
    }
}
