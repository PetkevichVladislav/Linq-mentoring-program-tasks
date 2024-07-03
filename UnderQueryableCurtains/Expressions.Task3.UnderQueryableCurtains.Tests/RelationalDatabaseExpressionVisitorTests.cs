using Expressions.Task3.UnderQueryableCurtains.Databases;
using Expressions.Task3.UnderQueryableCurtains.Models;

namespace Expressions.Task3.UnderQueryableCurtains.Tests
{
    [TestClass]
    public class RelationalDatabaseExpressionVisitorTests
    {
        private RelationalDatabase _database;

        [TestInitialize]
        public void SetUp()
        {
            var users = new List<User>
            {
                new() { Name = "Michael", Id = 2, Surname = "Lee" },
                new() { Name = "John", Id = 3, Surname = "Jhonson"  }
            };

            _database = new RelationalDatabase(users);
        }

        [TestMethod]
        public void Visit_WhenLessThanExpression_ThenGeneratesCorrectSql()
        {
            var customQuery = _database.AsQueryable().Where(user => user.Id < 3);
            var visitor = new RelationalDatabaseExpressionVisitor();

            visitor.Visit(customQuery.Expression);

            string expected = "SELECT * FROM [dbo].[users] WHERE [Id] < 3";
            Assert.AreEqual(expected, visitor.GetExpressionSql());
        }

        [TestMethod]
        public void Visit_WhenGreaterThanExpression_ThenGeneratesCorrectSql()
        {
            var customQuery = _database.AsQueryable().Where(user => user.Id > 1);
            var visitor = new RelationalDatabaseExpressionVisitor();

            visitor.Visit(customQuery.Expression);

            string expected = "SELECT * FROM [dbo].[users] WHERE [Id] > 1";
            Assert.AreEqual(expected, visitor.GetExpressionSql());
        }

        [TestMethod]
        public void Visit_WhenNotEqualsExpression_ThenGeneratesCorrectSql()
        {
            var customQuery = _database.AsQueryable().Where(user => user.Name != "John");
            var visitor = new RelationalDatabaseExpressionVisitor();

            visitor.Visit(customQuery.Expression);

            string expected = "SELECT * FROM [dbo].[users] WHERE [Name] != 'John'";
            Assert.AreEqual(expected, visitor.GetExpressionSql());
        }

        [TestMethod]
        public void Visit_WhenAndAlsoExpression_ThenGeneratesCorrectSql()
        {
            var customQuery = _database.AsQueryable().Where(user => user.Name == "Michael" && user.Id == 2);
            var visitor = new RelationalDatabaseExpressionVisitor();

            visitor.Visit(customQuery.Expression);

            string expected = "SELECT * FROM [dbo].[users] WHERE [Name] = 'Michael' AND [Id] = 2";
            Assert.AreEqual(expected, visitor.GetExpressionSql());
        }
    }
}