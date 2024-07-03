using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.UnderQueryableCurtains
{
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
            Visit(node.Right);

            return node;
        }

        public string GetExpressionSql()
        {
            // return sql query here. 
            return sqlQuery.ToString();
        }
    }
}
