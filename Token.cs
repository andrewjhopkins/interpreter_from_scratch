namespace interpreter_from_scratch
{
    public class Token
    {
        public string Literal { get; set; }
        public TokenType Type { get; set; }

        public Token(TokenType type, string literal)
        { 
            Literal = literal;
            Type = type;
        }
    }
}
