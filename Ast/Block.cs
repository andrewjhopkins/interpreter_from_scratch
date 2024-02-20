namespace interpreter_from_scratch.Ast
{
    public class Block : Statement
    {
        public IEnumerable<Statement> Statements { get; set; }

        public Block(IEnumerable<Statement> statements)
        {
            Statements = statements;
        }
    }
}
