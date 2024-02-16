namespace interpreter_from_scratch.Ast
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public TokenType Operation { get; set; }
        public Expression Right { get; set; }

        public BinaryExpression(Expression left, TokenType operation, Expression right)
        {
            Left = left;
            Operation = operation;
            Right = right;
        }
    }
}
