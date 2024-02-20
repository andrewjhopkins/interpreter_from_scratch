using interpreter_from_scratch.Ast;
using interpreter_from_scratch.Evaluation;

namespace interpreter_from_scratch;

public class Evaluator
{
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
            default:
                return null;
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
                return EvaluateBinaryExpression(binaryExpression);
            default:
                return null;
        }
    }

    public InterpreterObject EvaluateBinaryExpression(BinaryExpression expression)
    {
        if (expression.Left is not Integer || expression.Right is not Integer)
        {
            throw new Exception($"Only integers are capable of binary expressions. Got {expression.Left.GetType()}, {expression.Right.GetType()}");
        }

        var left = (Integer)expression.Left;
        var right = (Integer)expression.Right;

        switch(expression.Operation)
        {
            case TokenType.PLUS:
                return new IntegerObject(left.Value + right.Value);
            case TokenType.MINUS:
                return new IntegerObject(left.Value - right.Value);
            case TokenType.ASTERISK:
                return new IntegerObject(left.Value * right.Value);
            case TokenType.SLASH:
                return new IntegerObject(left.Value / right.Value);
            case TokenType.GREATERTHAN:
                return new BoolObject(left.Value > right.Value);
            case TokenType.LESSTHAN:
                return new BoolObject(left.Value < right.Value);
            case TokenType.EQUALS:
                return new BoolObject(left.Value == right.Value);
            case TokenType.DOESNOTEQUAL:
                return new BoolObject(left.Value != right.Value);
            default:
                return null;
        }
    }
}