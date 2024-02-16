namespace interpreter_from_scratch.Ast
{
    public class Block
    {
        public IEnumerable<Statement> Statements { get; set; }

        public Block(IEnumerable<Statement> statements)
        {
            Statements = statements;
        }
    }
}
