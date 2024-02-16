namespace interpreter_from_scratch.Ast
{
    public class Function : Statement
    {
        public Identifier Identifier { get; set; }
        public IEnumerable<Identifier> Parameters { get; set; }
        public Block Body { get; set; }

        public Function(Token token, Identifier identifier, IEnumerable<Identifier> parameters, Block body)
        {
            Token = token;
            Identifier = identifier;
            Parameters = parameters;
            Body = body;
        }
    }
}
