using Expressions.Task3.UnderQueryableCurtains.Databases;
using Expressions.Task3.UnderQueryableCurtains.Models;

namespace Expressions.Task3.UnderQueryableCurtains.Tests
{

    [TestClass]
    public class RelationalDatabaseTests
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
        public void AsQueryable_WhenQueryingExistingData_ThenReturnsCorrectResults()
        {
            var results = _database.AsQueryable().Where(user => user.Name == "Michael" && user.Id == 2).ToList();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Michael", results.First().Name);
            Assert.AreEqual(2, results.First().Id);
        }

        [TestMethod]
        public void AsQueryable_WhenQueryingNonExistingData_ThenReturnsEmpty()
        {
            var results = _database.AsQueryable().Where(user => user.Name == "NonExistent" && user.Id == 0).ToList();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Query_WhenCalledWithSqlQuery_ThenReturnsCorrectData()
        {
            var results = _database.Query("SELECT * FROM [dbo].[users] WHERE [Name] = 'Michael' AND [Id] = 2");

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Michael", results.First().Name);
            Assert.AreEqual(2, results.First().Id);
        }

        [TestMethod]
        public void Query_WhenCalledWithNonExistingSqlQuery_ThenReturnsEmpty()
        {
            var results = _database.Query("SELECT * FROM [dbo].[users] WHERE [Name] = 'NonExistent' AND [Id] = 0");

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}