namespace CustomLinqProvider.Tests.Fake
{
    using System.Linq.Expressions;

    public class FakeQueryProvider : QueryProvider
    {
        public override string GetQueryText(Expression expression)
        {
            return this.Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            return null;
        }

        private string Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new QueryTranslator().Translate(expression);
        }
    }
}
