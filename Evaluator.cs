using interpreter_from_scratch.Ast;
using interpreter_from_scratch.Evaluation;

namespace interpreter_from_scratch;

public class Evaluator
{
    private readonly HashSet<TokenType> _integerOnlyOperators = new HashSet<TokenType>{ 
        TokenType.ASTERISK, 
        TokenType.PLUS, 
        TokenType.MINUS, 
        TokenType.SLASH ,
        TokenType.GREATERTHAN,
        TokenType.LESSTHAN
    };
    
    public InterpreterObject Evaluate(InterpreterProgram program)
    {
        InterpreterObject result = null;

        for (var i = 0; i < program.Statements.Count(); i++)
        {
            result = Evaluate(program.Statements.ElementAt(i));
        }

        return result;
    }

    public InterpreterObject Evaluate(Statement statement)
    {
        switch (statement)
        {
            case ExpressionStatement expressionStatement:
               return Evaluate(expressionStatement.Expression);
        }
    }

    public InterpreterObject Evaluate(Expression expression)
    {
        switch(expression)
        {
            case Integer integer:
                return new IntegerObject(integer.Value);
            case Bool boolean:
                return new BoolObject(boolean.Value);
            case BinaryExpression binaryExpression:

            /*
            case Identifier identifier:
            case FunctionCall functionCall:
            case BinaryExpression binaryExpression:
            */
        }
    }

    public InterpreterObject EvaluateBinaryExpression(BinaryExpression expression)
    {
        if (_integerOnlyOperators.Contains(expression.Operation) && (expression.Left is not Integer || expression.Right is not Integer))
        {
            throw new Exception($"Only integers are capable of binary expression with the selected operator {expression.Operation}. Got {expression.Left.GetType()}, {expression.Right.GetType()}");
        }

        switch(expression.Operation)
        {
            case TokenType.PLUS:
                // var left = (Integer)expression.Left;
                var right = (Integer)expression.Right;
                return new IntegerObject(((Integer)expression.Left).Value + right.Value);
            case TokenType.MINUS:
            case TokenType.ASTERISK:
            case TokenType.SLASH:

        }

    }
}