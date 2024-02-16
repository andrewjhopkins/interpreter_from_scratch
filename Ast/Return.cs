namespace interpreter_from_scratch.Ast
{
    public class Return : Statement
    {
        public Expression Value { get; set; }

        public Return(Token token, Expression value)
        {
            Token = token;
            Value = value;
        }
    }
}
