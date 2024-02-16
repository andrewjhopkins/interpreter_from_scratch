namespace interpreter_from_scratch.Ast
{
    public class InterpreterProgram
    {
        public IEnumerable<Statement> Statements { get; set; }

        public InterpreterProgram(IEnumerable<Statement> statements)
        {
            Statements = statements;
        }
    }
}
