namespace interpreter_from_scratch.Ast
{
    public class Var : Statement
    {
        public Identifier Identifier { get; set; }
        public Expression Value { get; set; }

        public Var(Token token, Identifier identifier, Expression value)
        {
            Token = token;
            Identifier = identifier;
            Value = value;
        }
    }
}
