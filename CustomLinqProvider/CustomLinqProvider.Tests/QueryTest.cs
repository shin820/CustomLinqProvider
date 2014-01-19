using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomLinqProvider.Tests
{
    using System.Linq;

    using CustomLinqProvider.Tests.Entity;
    using CustomLinqProvider.Tests.Fake;

    [TestClass]
    public class QueryTest
    {
        [TestMethod]
        public void ShouldTranslateWhereClause()
        {
            Query<Customer> customers = new Query<Customer>(new FakeQueryProvider());
            IQueryable<Customer> query = customers.Where(c => c.City == "London");

            string expected = "SELECT * FROM (SELECT * FROM Customer) AS T WHERE (City = 'London')";
            Assert.AreEqual(expected, query.ToString());
        }

        [TestMethod]
        public void ShouldTranslatingLocalVariableReferences()
        {
            Query<Customer> customers = new Query<Customer>(new FakeQueryProvider());

            string london = "London";
            IQueryable<Customer> query = customers.Where(c => c.City == london);

            string expected = "SELECT * FROM (SELECT * FROM Customer) AS T WHERE (City = 'London')";
            Assert.AreEqual(expected, query.ToString());
        }
    }
}
