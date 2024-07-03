using System.Linq.Expressions;

namespace Expressions.Task3.Calculator
{
    internal static class ExpressionConverter
    {
        private static readonly Dictionary<string, int> operatorsPrecedence = new Dictionary<string, int>
        {
            { "-", 1 },
            { "+", 1 },
            { "/", 2 },
            { "*", 2 },
        };

        public static Expression TranslateLineToExpression(string line)
        {
            var reversedPolishNotation = TranslateStringToReversedPolishNotation(line);
            var expression = EvaluateReversedPolishNotation(reversedPolishNotation);

            return expression;
        }

        private static Expression EvaluateReversedPolishNotation(Queue<string> reversedPolishNotation)
        {
            var expressions = new Stack<Expression>();
            foreach (var token in reversedPolishNotation)
            {
                if (double.TryParse(token.ToString(), out double operand))
                {
                    expressions.Push(Expression.Constant(operand));
                }
                else
                {
                    var rightOperand = expressions.Pop();
                    var leftOperand = expressions.Pop();
                    var expression = TranslateOperation(token, leftOperand, rightOperand);
                    expressions.Push(expression);
                }
            }

            return expressions.Pop();
        }

        private static Queue<string> TranslateStringToReversedPolishNotation(string line)
        {
            var operators = new Stack<string>();
            var reversedPolishNotation = new Queue<string>();
            var expression = line.Replace(" ", "");
            for (int currentSymbolIndex = 0; currentSymbolIndex < expression.Length; currentSymbolIndex++)
            {
                var token = expression[currentSymbolIndex].ToString();
                HandleOperand(reversedPolishNotation, expression, ref currentSymbolIndex);
                HandleOperator(operators, reversedPolishNotation, token);
                HandleParenthesis(operators, reversedPolishNotation, token);
            }

            while (operators.Count > 0)
            {
                reversedPolishNotation.Enqueue(operators.Pop());
            }
            var a = string.Join("",reversedPolishNotation.Select(x => x));

            return reversedPolishNotation;
        }

        private static void HandleParenthesis(Stack<string> operators, Queue<string> reversedPolishNotation, string token)
        {

            if (token == "(")
            {
                operators.Push(token);
            }
            if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                {
                    reversedPolishNotation.Enqueue(operators.Pop());
                }
                operators.Pop();
            }
        }

        private static void HandleOperator(Stack<string> operators, Queue<string> reversedPolishNotation, string token)
        {
            if (operatorsPrecedence.ContainsKey(token))
            {
                while (IsOperatorInStackHasGreaterPrecedence(operators, token))
                {
                    reversedPolishNotation.Enqueue(operators.Pop());
                }
                operators.Push(token);
            }
        }

        private static bool IsOperatorInStackHasGreaterPrecedence(Stack<string> operators, string token)
        {
            return operators.Count > 0 &&
                                operatorsPrecedence.ContainsKey(operators.Peek()) &&
                                operatorsPrecedence[operators.Peek()] >= operatorsPrecedence[token];
        }

        private static void HandleOperand(Queue<string> reversedPolishNotation, string expression, ref int currentSymbolIndex)
        {
            if (char.IsDigit(expression[currentSymbolIndex]))
            {
                var numberStartIndex = currentSymbolIndex;
                while (IsNextCharacterNumberPart(expression, currentSymbolIndex))
                {
                    currentSymbolIndex++;
                }
                var extractedNumber = expression.Substring(numberStartIndex, currentSymbolIndex - numberStartIndex + 1);
                reversedPolishNotation.Enqueue(extractedNumber);
            }
        }

        private static bool IsNextCharacterNumberPart(string expression, int tokenIndex)
        {
            return tokenIndex + 1 < expression.Length && (char.IsDigit(expression[tokenIndex + 1]) || expression[tokenIndex + 1] == '.');
        }

        private static Expression TranslateOperation(string operation, Expression leftParameter, Expression rightParameter)
        {
            var expression = operation switch
            {
                "+" => Expression.Add(leftParameter, rightParameter),
                "-" => Expression.Subtract(leftParameter, rightParameter),
                "*" => Expression.Multiply(leftParameter, rightParameter),
                "/" => Expression.Divide(leftParameter, rightParameter),
                _ => throw new Exception($"Unexpected operator: {operation}"),
            };

            return expression;
        }
    }
}