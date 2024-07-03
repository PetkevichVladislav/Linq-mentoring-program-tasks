using System.Linq.Expressions;
using System.Text;

public class RelationalDatabaseExpressionVisitor : ExpressionVisitor
{
    private readonly StringBuilder sqlQuery;

    public RelationalDatabaseExpressionVisitor()
    {
        sqlQuery = new StringBuilder();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);

        switch (node.NodeType)
        {
            case ExpressionType.Equal:
                ValidateBinaryNode(node);
                sqlQuery.Append(" = ");
                break;
            case ExpressionType.NotEqual:
                ValidateBinaryNode(node);
                sqlQuery.Append(" != ");
                break;
            case ExpressionType.LessThanOrEqual:
                ValidateBinaryNode(node);
                sqlQuery.Append(" <= ");
                break;
            case ExpressionType.LessThan:
                ValidateBinaryNode(node);
                sqlQuery.Append(" < ");
                break;
            case ExpressionType.GreaterThan:
                ValidateBinaryNode(node);
                sqlQuery.Append(" > ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                ValidateBinaryNode(node);
                sqlQuery.Append(" >= ");
                break;
            case ExpressionType.AndAlso:
                sqlQuery.Append(" AND ");
                break;
            default:
                throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
        };

        Visit(node.Right);

        return node;
    }

    private static void ValidateBinaryNode(BinaryExpression node)
    {
        if (node.Left.NodeType != ExpressionType.MemberAccess)
        {
            throw new NotSupportedException($"Left operand should be property or field: {node.NodeType}");
        }

        if (node.Right.NodeType != ExpressionType.Constant)
        {
            throw new NotSupportedException($"Right operand should be constant: {node.NodeType}");
        }
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        sqlQuery.Append($"[{node.Member.Name}]");
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value?.GetType() == typeof(string))
            sqlQuery.Append($"'{node.Value}'");
        else
            sqlQuery.Append(node.Value);

        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
        {
            string tableName = GetTableNameFromExpression(node);
            sqlQuery.Append($"SELECT * FROM [dbo].[{tableName}] WHERE ");

            var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            Visit(lambda.Body);
            return node;
        }

        return base.VisitMethodCall(node);
    }

    private static Expression StripQuotes(Expression expression)
    {
        while (expression.NodeType == ExpressionType.Quote)
        {
            expression = ((UnaryExpression)expression).Operand;
        }

        return expression;
    }

    private string GetTableNameFromExpression(MethodCallExpression node)
    {
        var dataSource = node.Arguments[0];
        var entityType = dataSource.Type.GetGenericArguments()[0];
        return entityType.Name.ToLower() + 's';
    }

    public string GetExpressionSql()
    {
        return sqlQuery.ToString();
    }
}