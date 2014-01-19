using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLinqProvider
{
    using System.Data;
    using System.Data.Common;
    using System.Linq.Expressions;
    using System.Reflection;

    public class DbQueryProvider : QueryProvider
    {
        IDbConnection connection;

        public DbQueryProvider(IDbConnection connection)
        {
            this.connection = connection;
        }

        public override string GetQueryText(Expression expression)
        {
            return this.Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            IDbCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = this.Translate(expression);
            IDataReader reader = cmd.ExecuteReader();
            Type elementType = TypeSystem.GetElementType(expression.Type);
            return Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { reader },
                null);
        }

        private string Translate(Expression expression)
        {
            return new QueryTranslator().Translate(expression);
        }
    }
}
