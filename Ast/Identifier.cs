namespace interpreter_from_scratch.Ast
{
    public class Identifier : Expression
    {
        public string Value { get; set; }
        public Identifier(Token token)
        {
            Token = token;
            Value = token.Literal;
        }
    }
}
