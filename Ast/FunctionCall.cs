namespace interpreter_from_scratch.Ast;

public class FunctionCall : Expression
{
    public Identifier Function { get; set; }
    public IEnumerable<Expression> Parameters { get; set; }

    public FunctionCall(Token token, Identifier function, IEnumerable<Expression> parameters)
    {
        Function = function;
        Parameters = parameters;
        Token = token;
    }
}

