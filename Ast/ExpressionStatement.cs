namespace interpreter_from_scratch.Ast
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }

        public ExpressionStatement(Token token, Expression expression)
        {
            Token = token;
            Expression = expression;
        }
    }
}
