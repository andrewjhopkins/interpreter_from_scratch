namespace interpreter_from_scratch.Ast;

public class FunctionCall : Expression
{
    public Expression Function { get; set; }
    public IEnumerable<Expression> Parameters { get; set; }
}

