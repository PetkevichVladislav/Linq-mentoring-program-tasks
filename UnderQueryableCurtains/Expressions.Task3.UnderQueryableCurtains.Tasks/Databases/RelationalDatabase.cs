using System.Linq.Expressions;
using Expressions.Task3.UnderQueryableCurtains.Models;

namespace Expressions.Task3.UnderQueryableCurtains.Databases
{
    public class RelationalDatabase
    {
        private List<User> _users;

        public RelationalDatabase(List<User> users)
        {
            _users = users;
        }

        private Dictionary<string, Func<User, string, string, bool>> Operators = new Dictionary<string, Func<User, string, string, bool>>
        {
            ["="] = (user, fieldName, value) => Convert.ToString(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) == value,
            ["!="] = (user, fieldName, value) => Convert.ToString(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) != value,
            [">"] = (user, fieldName, value) => Convert.ToInt32(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) > Convert.ToInt32(value),
            ["<"] = (user, fieldName, value) => Convert.ToInt32(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) < Convert.ToInt32(value),
            ["<="] = (user, fieldName, value) => Convert.ToInt32(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) <= Convert.ToInt32(value),
            [">="] = (user, fieldName, value) => Convert.ToInt32(user.GetType().GetProperty(fieldName)!.GetValue(user, null)) >= Convert.ToInt32(value),
        };

        private Func<User, bool> ParseCondition(string condition)
        {
            foreach (var op in Operators.Keys)
            {
                if (condition.Contains(op))
                {
                    var split = condition.Split(new[] { op }, StringSplitOptions.None);
                    var fieldName = split[0].Trim('[', ' ', ']');
                    var value = split[1].Trim(' ', '\'');
                    return user => Operators[op].Invoke(user, fieldName, value);
                }
            }

            throw new Exception($"Condition {condition} could not be parsed.");
        }

        public IEnumerable<User> Query(string sql)
        {
            var whereIndex = sql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);
            if (whereIndex >= 0)
            {
                var conditions = sql.Substring(whereIndex + 5).Split(new[] { "AND", "OR" }, StringSplitOptions.None).Select(cond => cond.Trim());

                foreach (var condition in conditions)
                {
                    var func = ParseCondition(condition);
                    _users = _users.Where(func.Invoke).ToList();
                }
            }

            return _users.AsEnumerable();
        }

        public RelationalDatabaseQueryable<User> AsQueryable()
        {
            var customProvider = new RelationalDatabaseQueryProvider<User>(this);
            var expression = Expression.Constant(_users.AsQueryable());
            return new RelationalDatabaseQueryable<User>(customProvider, expression);
        }
    }
}
